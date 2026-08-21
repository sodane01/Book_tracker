using Book_tracker.Models;
using Book_tracker.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Book_tracker.Authorization
{
    public class ActiveUserHandler
        : AuthorizationHandler<ActiveUserRequirement>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ActiveUserHandler(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ActiveUserRequirement requirement)
        {
            if (context.User.Identity?.IsAuthenticated != true)
            {
                return;
            }

            var user = await _userManager.GetUserAsync(context.User);

            if (user?.AccountStatus == AccountStatus.Active)
            {
                context.Succeed(requirement);
            }
        }
    }
}