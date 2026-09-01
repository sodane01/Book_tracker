using Book_tracker.Authorization;
using Book_tracker.Data;
using Book_tracker.Models;
using Book_tracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(
    options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ActiveUser", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddRequirements(new ActiveUserRequirement());
    });
});

builder.Services.AddScoped<IAuthorizationHandler, ActiveUserHandler>();

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<IGoogleBooksService, GoogleBooksService>(
    client =>
    {
        client.BaseAddress =
            new Uri("https://www.googleapis.com/books/v1/");
    });
builder.Services.AddScoped<IUserBookService, UserBookService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;

    await SeedData.SeedRolesAsync(serviceProvider);

    if (app.Environment.IsDevelopment())
    {
        await SeedData.SeedAdminAsync(
            serviceProvider,
            builder.Configuration);
    }
}

app.Run();