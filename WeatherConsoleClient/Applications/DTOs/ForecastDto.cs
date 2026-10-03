using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeatherConsoleClient.Applications.DTOs
{
    public class ForecastDto
    {
        [JsonPropertyName("cnt")]
        public int Count { get; set; }

        [JsonPropertyName("list")]
        public List<ForecastItemDto> Items { get; set; } = [];

        [JsonPropertyName("city")]
        public ForecastCityDto City { get; set; } = new();
    }
}
