using System.Text.Json.Serialization;

namespace WeatherAPI.Models
{
    public class WeatherResponseDto
    {
        [JsonPropertyName("location")]
        public string Location { get; set; } = string.Empty;
        [JsonPropertyName("conditions")]
        public string Conditions { get; set; } = string.Empty;
        [JsonPropertyName("temperature")]
        public double Temperature { get; set; }
        [JsonPropertyName("humidity")]
        public double Humidity { get; set; }
        [JsonPropertyName("windspeed")]
        public double WindSpeed { get; set; }
        [JsonPropertyName("day")]
        public string Date { get; set; } = string.Empty;
        [JsonPropertyName("source")]
        public string Source { get; set; } = string.Empty; // api or cache
    }
}
