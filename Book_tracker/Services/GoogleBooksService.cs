using System.Text.Json;
using Book_tracker.Models;
using Book_tracker.Models.GoogleBooks;
using Book_tracker.Models.Enums;

namespace Book_tracker.Services
{
    public class GoogleBooksService : IGoogleBooksService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public GoogleBooksService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<BookSearchResult>> SearchBooksAsync(
            string query,
            BookSearchType searchType)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return new List<BookSearchResult>();
            }

            var apiKey = _configuration["GoogleBooks:ApiKey"];

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return new List<BookSearchResult>();
            }

            try
            {
                string googleQuery;

                if (searchType == BookSearchType.Title)
                {
                    googleQuery = $"intitle:{query}";
                }
                else if (searchType == BookSearchType.Author)
                {
                    googleQuery = $"inauthor:{query}";
                }
                else if (searchType == BookSearchType.Genre)
                {
                    googleQuery = $"subject:{query}";
                }
                else
                {
                    googleQuery = query;
                }
                var encodedQuery = Uri.EscapeDataString(googleQuery);

                var response = await _httpClient.GetAsync(
                    $"volumes?q={encodedQuery}&key={apiKey}");

                if (!response.IsSuccessStatusCode)
                {
                    return new List<BookSearchResult>();
                }

                var json = await response.Content.ReadAsStringAsync();

                var googleResponse =
                    JsonSerializer.Deserialize<GoogleBooksResponse>(
                        json,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                if (googleResponse?.Items == null)
                {
                    return new List<BookSearchResult>();
                }

                var books = googleResponse.Items
                    .Select(volume => new BookSearchResult
                    {
                        ExternalBookId = volume.Id,
                        Title = volume.VolumeInfo.Title,
                        Author = string.Join(
                            ", ",
                            volume.VolumeInfo.Authors),

                        CoverImageUrl =
                            volume.VolumeInfo.ImageLinks?.Thumbnail,

                        AverageRating =
                            volume.VolumeInfo.AverageRating
                    })
                    .ToList();

                return books;
            }
            catch (HttpRequestException)
            {
                return new List<BookSearchResult>();
            }
            catch (JsonException)
            {
                return new List<BookSearchResult>();
            }
        }
    }
}