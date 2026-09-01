using System.Text.Json;
using Book_tracker.Models;
using Book_tracker.Models.GoogleBooks;
using Book_tracker.Models.Enums;
using Book_tracker.ViewModels;

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

        public async Task<BookSearchServiceResult> SearchBooksAsync(
            string query,
            BookSearchType searchType)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return new BookSearchServiceResult
                {
                    IsSuccess = true
                };
            }

            var apiKey = _configuration["GoogleBooks:ApiKey"];

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return new BookSearchServiceResult
                {
                    IsSuccess = false
                };
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
                else if (searchType == BookSearchType.Series)
                {
                    googleQuery = query;
                }
                else
                {
                    googleQuery = query;
                }

                var encodedQuery =
                    Uri.EscapeDataString(googleQuery);

                var response = await _httpClient.GetAsync(
                    $"volumes?q={encodedQuery}&key={apiKey}");

                if (!response.IsSuccessStatusCode)
                {
                    return new BookSearchServiceResult
                    {
                        IsSuccess = false
                    };
                }

                var json =
                    await response.Content.ReadAsStringAsync();

                var googleResponse =
                    JsonSerializer.Deserialize<GoogleBooksResponse>(
                        json,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                if (googleResponse?.Items == null)
                {
                    return new BookSearchServiceResult
                    {
                        IsSuccess = true
                    };
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

                return new BookSearchServiceResult
                {
                    IsSuccess = true,
                    Books = books
                };
            }
            catch (HttpRequestException)
            {
                return new BookSearchServiceResult
                {
                    IsSuccess = false
                };
            }
            catch (JsonException)
            {
                return new BookSearchServiceResult
                {
                    IsSuccess = false
                };
            }
        }

        public async Task<BookDetailsViewModel?> GetBookByIdAsync(
            string externalBookId)
        {
            if (string.IsNullOrWhiteSpace(externalBookId))
            {
                return null;
            }

            var apiKey = _configuration["GoogleBooks:ApiKey"];

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return null;
            }

            try
            {
                var encodedBookId =
                    Uri.EscapeDataString(externalBookId);

                var response = await _httpClient.GetAsync(
                    $"volumes/{encodedBookId}?key={apiKey}");

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var json =
                    await response.Content.ReadAsStringAsync();

                var volume =
                    JsonSerializer.Deserialize<GoogleBooksVolume>(
                        json,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                if (volume?.VolumeInfo == null)
                {
                    return null;
                }

                var book = new BookDetailsViewModel
                {
                    ExternalBookId = volume.Id,

                    Title = volume.VolumeInfo.Title,

                    Author = string.Join(
                        ", ",
                        volume.VolumeInfo.Authors),

                    CoverImageUrl =
                        volume.VolumeInfo.ImageLinks?.Thumbnail,

                    Description =
                        volume.VolumeInfo.Description,

                    Publisher =
                        volume.VolumeInfo.Publisher,

                    PublishedDate =
                        volume.VolumeInfo.PublishedDate,

                    AverageRating =
                        volume.VolumeInfo.AverageRating
                };

                return book;
            }
            catch (HttpRequestException)
            {
                return null;
            }
            catch (JsonException)
            {
                return null;
            }
        }
    }
}