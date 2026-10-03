using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherConsoleClient.Applications.DTOs;
using WeatherConsoleClient.Applications.Interfaces;

namespace WeatherConsoleClient.Presentation
{
    public class ConsoleMenu
    {
        private readonly IWeatherService _weatherService;
        private readonly IWeatherFormatter _weatherFormatter;

        public ConsoleMenu(
            IWeatherService weatherService,
            IWeatherFormatter weatherFormatter)
        {
            _weatherService = weatherService;
            _weatherFormatter = weatherFormatter;
        }

        public async Task RunAsync(
            CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                DisplayMenu();

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await ShowCurrentWeatherAsync(
                            cancellationToken);
                        break;

                    case "2":
                        await ShowForecastAsync(
                            cancellationToken);
                        break;

                    case "3":
                        await ShowDashboardAsync(
                            cancellationToken);
                        break;

                    case "0":
                        return;

                    default:
                        Console.WriteLine(
                            "Invalid option.");
                        break;
                }

                Console.WriteLine();
                Console.WriteLine(
                    "Press ENTER to continue...");

                Console.ReadLine();
            }
        }

        private static void DisplayMenu()
        {
            Console.Clear();

            Console.WriteLine(
                "========================================");

            Console.WriteLine(
                "       WEATHER CONSOLE CLIENT");

            Console.WriteLine(
                "========================================");

            Console.WriteLine();

            Console.WriteLine(
                "1. Current Weather");

            Console.WriteLine(
                "2. 5-Day / 3-Hour Forecast");

            Console.WriteLine(
                "3. Weather Dashboard");

            Console.WriteLine(
                "0. Exit");

            Console.WriteLine();

            Console.Write(
                "Enter your choice: ");
        }
        private static string SelectTemperatureUnit()
        {
            Console.WriteLine();
            Console.WriteLine("TEMPERATURE UNIT");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("1. Celsius");
            Console.WriteLine("2. Fahrenheit");
            Console.Write("Enter your choice: ");

            var choice = Console.ReadLine();

            return choice == "2" ? "F" : "C";
        }

        private static decimal ConvertTemperature(
            decimal celsius,
            string unit)
        {
            if (unit == "F")
            {
                return (celsius * 9 / 5) + 32;
            }

            return celsius;
        }
        private async Task ShowCurrentWeatherAsync(
    CancellationToken cancellationToken)
        {
            Console.WriteLine();

            Console.Write("Enter city: ");

            var city = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(city))
            {
                Console.WriteLine(
                    "City is required.");

                return;
            }

            var unit =
                SelectTemperatureUnit();

            var weather =
                await _weatherService
                    .GetCurrentWeatherAsync(
                        city,
                        cancellationToken);

            if (weather is null)
            {
                Console.WriteLine(
                    "City not found.");

                return;
            }

            Console.WriteLine(
                _weatherFormatter.FormatCurrentWeather(
                    weather,
                    unit));
        }

        private async Task ShowForecastAsync(
    CancellationToken cancellationToken)
        {
            Console.WriteLine();

            Console.Write("Enter city: ");

            var city = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(city))
            {
                Console.WriteLine(
                    "City is required.");

                return;
            }

            var unit =
                SelectTemperatureUnit();

            var forecast =
                await _weatherService
                    .GetForecastAsync(
                        city,
                        cancellationToken);

            if (forecast is null)
            {
                Console.WriteLine(
                    "City not found.");

                return;
            }

            var filteredItems =
                SelectForecastFilter(
                    forecast.Items);

            var filteredForecast =
                new ForecastDto
                {
                    City = forecast.City,
                    Items = filteredItems
                };

            Console.WriteLine(
                _weatherFormatter.FormatForecast(
                    filteredForecast,
                    unit));

            Console.WriteLine(
                _weatherFormatter.FormatForecastSummary(
                    filteredItems,
                    unit));
        }

        private static List<ForecastItemDto> SelectForecastFilter(
            List<ForecastItemDto> items)
        {
            Console.WriteLine();
            Console.WriteLine(
                "FORECAST FILTER");

            Console.WriteLine(
                "----------------------------------------");

            Console.WriteLine(
                "1. Show all forecast entries");

            Console.WriteLine(
                "2. Show today's forecast");

            Console.WriteLine(
                "3. Show tomorrow's forecast");

            Console.Write(
                "Enter your choice: ");

            var choice = Console.ReadLine();

            if (choice == "1")
            {
                return items;
            }

            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            if (choice == "2")
            {
                return items
                    .Where(item =>
                    {
                        if (DateTime.TryParse(
                            item.DateTimeText,
                            out var date))
                        {
                            return date.Date == today;
                        }

                        return false;
                    })
                    .ToList();
            }

            if (choice == "3")
            {
                return items
                    .Where(item =>
                    {
                        if (DateTime.TryParse(
                            item.DateTimeText,
                            out var date))
                        {
                            return date.Date == tomorrow;
                        }

                        return false;
                    })
                    .ToList();
            }

            Console.WriteLine(
                "Invalid filter. Showing all forecast entries.");

            return items;
        }
        
        private async Task ShowDashboardAsync(
            CancellationToken cancellationToken)
        {
            Console.WriteLine();

            Console.Write("Enter city: ");

            var city = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(city))
            {
                Console.WriteLine(
                    "City is required.");
                return;
            }

            var currentWeatherTask =
                _weatherService.GetCurrentWeatherAsync(
                    city,
                    cancellationToken);

            var forecastTask =
                _weatherService.GetForecastAsync(
                    city,
                    cancellationToken);

            await Task.WhenAll(
                currentWeatherTask,
                forecastTask);

            var currentWeather =
                await currentWeatherTask;

            var forecast =
                await forecastTask;

            if (currentWeather is null ||
                forecast is null)
            {
                Console.WriteLine(
                    "Unable to retrieve weather information.");
                return;
            }

            DisplayDashboard(
                currentWeather,
                forecast);
        }
        private static void DisplayDashboard(
            CurrentWeatherDto currentWeather,
            ForecastDto forecast)
        {
            Console.WriteLine();

            Console.WriteLine(
                "========================================");

            Console.WriteLine(
                "WEATHER DASHBOARD");

            Console.WriteLine(
                "========================================");

            Console.WriteLine();

            Console.WriteLine(
                $"City: {currentWeather.Name}");

            Console.WriteLine();

            Console.WriteLine(
                "CURRENT WEATHER");

            Console.WriteLine(
                "----------------------------------------");

            Console.WriteLine(
                $"Temperature : " +
                $"{currentWeather.Main.Temperature:F1} °C");

            Console.WriteLine(
                $"Feels Like  : " +
                $"{currentWeather.Main.FeelsLike:F1} °C");

            Console.WriteLine(
                $"Humidity    : " +
                $"{currentWeather.Main.Humidity}%");

            if (currentWeather.Weather.Count > 0)
            {
                Console.WriteLine(
                    $"Condition   : " +
                    $"{currentWeather.Weather[0].Description}");
            }

            Console.WriteLine();

            Console.WriteLine(
                "FORECAST");

            Console.WriteLine(
                "----------------------------------------");

            foreach (var item in forecast.Items)
            {
                var condition =
                    item.Weather.Count > 0
                        ? item.Weather[0].Description
                        : "Unknown";

                var precipitation =
                    item.ProbabilityOfPrecipitation * 100;

                Console.WriteLine(
                    $"{item.DateTimeText,-20}" +
                    $"{item.Main.Temperature,6:F1} °C   " +
                    $"{condition,-18}" +
                    $"{precipitation,4:F0}%");
            }
        }
    }
}