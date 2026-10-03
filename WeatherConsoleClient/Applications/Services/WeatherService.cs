using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherConsoleClient.Applications.DTOs;
using WeatherConsoleClient.Applications.Interfaces;

namespace WeatherConsoleClient.Applications.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherApiClient _weatherApiClient;

        public WeatherService(
            IWeatherApiClient weatherApiClient)
        {
            _weatherApiClient = weatherApiClient;
        }

        public async Task<CurrentWeatherDto?> GetCurrentWeatherAsync(
            string city,
            CancellationToken cancellationToken)
        {
            ValidateCity(city);

            return await _weatherApiClient
                .GetCurrentWeatherAsync(
                    city,
                    cancellationToken);
        }

        public async Task<ForecastDto?> GetForecastAsync(
            string city,
            CancellationToken cancellationToken)
        {
            ValidateCity(city);

            return await _weatherApiClient
                .GetForecastAsync(
                    city,
                    cancellationToken);
        }

        private static void ValidateCity(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                throw new ArgumentException(
                    "City is required.");
            }
        }
    }
}
