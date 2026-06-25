using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WeatherAPI.Configurations;
using WeatherAPI.Models;

namespace WeatherAPI.Services
{
    public class WeatherService
    {
        private readonly HttpClient _client;
        private readonly WeatherApiSettings _options;
        private readonly IMemoryCache _cache;

        public WeatherService(
            HttpClient client,
            IOptions<WeatherApiSettings> options,
            IMemoryCache cache)
        {
            _client = client;
            _options = options.Value;
            _cache = cache;
        }

        public async Task<WeatherResponseDto?> GetWeatherAsync(string location)
        {
            // Sanitize input to create a safe, unique cache key
            string cacheKey = $"weather_{location.ToLower().Trim()}";

            // Look for data inside the memory cache
            if (_cache.TryGetValue(cacheKey, out string? cachedJsonWeather))
            {
                // Hit Cache and return data immediately without calling the external API
                var weatherDto = System.Text.Json.JsonSerializer.Deserialize<WeatherResponseDto>(cachedJsonWeather!);

                if (weatherDto != null)
                {
                    // Indicate that this data originated from the memory cache
                    weatherDto.Source = "cache";
                    return (weatherDto);
                }
            }

            try
            {
                // Build endpoint
                string url = $"{_options.BaseUrl}{location}?key={_options.ApiKey}";

                //Send request to Visualcrossing Weather API
                var response = await _client.GetAsync(url);

                // Invalid city — Visual Crossing returns 400 for unknown locations
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    return null;

                // API is down or returned an unexpected error
                if (!response.IsSuccessStatusCode)
                    throw new Exception($"External API returned {response.StatusCode}");

                var json = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(json);

                var root = doc.RootElement;
                var day = root.GetProperty("days")[0];

                var result = new WeatherResponseDto
                {
                    Location = root.GetProperty("resolvedAddress").GetString() ?? "",
                    Conditions = day.GetProperty("conditions").GetString() ?? "",
                    Temperature = day.TryGetProperty("temp", out var temp) && temp.ValueKind == JsonValueKind.Number ? temp.GetDouble() : 0,
                    Humidity = day.GetProperty("humidity").GetDouble(),
                    WindSpeed = day.GetProperty("windspeed").GetDouble(),
                    Date = day.GetProperty("datetime").GetString() ?? "",
                    Source = "api"
                };

                // Set configuration for cache lifecycle
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(20)) // Strictly evict after 20 minutes
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))   // Evict if not touched for 5 minutes
                    .SetPriority(CacheItemPriority.Normal);          // Priority level during memory pressure

                var jsonWeather = JsonSerializer.Serialize(result);

                // Commit to cache and return response
                _cache.Set(cacheKey, jsonWeather, cacheOptions);

                return result;
            }
            catch (HttpRequestException ex)
            {
                // network error handling — API is unreachable
                throw new Exception("Weather service is currently unavailable. Please try again later.", ex);
            }
            catch (JsonException ex)
            {
                // API returned something unexpected
                throw new Exception("Unexpected API response from weather service", ex);
            }
        }
    }
}
