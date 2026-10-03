using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeatherConsoleClient.Applications.DTOs
{
    public class CurrentWeatherDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("main")]
        public CurrentWeatherMainDto Main { get; set; } = new();

        [JsonPropertyName("weather")]
        public List<WeatherConditionDto> Weather { get; set; } = [];

        [JsonPropertyName("wind")]
        public WindDto Wind { get; set; } = new();
    }
}
