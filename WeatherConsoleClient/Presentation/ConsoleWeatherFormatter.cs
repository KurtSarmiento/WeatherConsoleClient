using System.Text;
using WeatherConsoleClient.Applications.DTOs;
using WeatherConsoleClient.Applications.Interfaces;

namespace WeatherConsoleClient.Presentation
{
    public class ConsoleWeatherFormatter : IWeatherFormatter
    {
        public string FormatCurrentWeather(
            CurrentWeatherDto weather,
            string unit)
        {
            var temperature =
                ConvertTemperature(
                    weather.Main.Temperature,
                    unit);

            var feelsLike =
                ConvertTemperature(
                    weather.Main.FeelsLike,
                    unit);

            var condition =
                weather.Weather.Count > 0
                    ? weather.Weather[0].Description
                    : "Unknown";

            var builder =
                new StringBuilder();

            builder.AppendLine();

            builder.AppendLine(
                "========================================");

            builder.AppendLine(
                "CURRENT WEATHER");

            builder.AppendLine(
                "========================================");

            builder.AppendLine();

            builder.AppendLine(
                $"City        : {weather.Name}");

            builder.AppendLine(
                $"Temperature : {temperature:F2} °{unit}");

            builder.AppendLine(
                $"Feels Like  : {feelsLike:F2} °{unit}");

            builder.AppendLine(
                $"Condition   : {condition}");

            builder.AppendLine(
                $"Humidity    : {weather.Main.Humidity} %");

            builder.AppendLine(
                $"Pressure    : {weather.Main.Pressure} hPa");

            builder.AppendLine(
                $"Wind Speed  : {weather.Wind.Speed:F2} m/s");

            if (weather.Main.Temperature > 35)
            {
                builder.AppendLine();

                builder.AppendLine(
                    "WEATHER ALERT:");

                builder.AppendLine(
                    "High temperature detected.");
            }

            return builder.ToString();
        }

        public string FormatForecast(
            ForecastDto forecast,
            string unit)
        {
            var builder =
                new StringBuilder();

            builder.AppendLine();

            builder.AppendLine(
                "========================================");

            builder.AppendLine(
                "5-DAY / 3-HOUR FORECAST");

            builder.AppendLine(
                "========================================");

            builder.AppendLine();

            builder.AppendLine(
                $"City: {forecast.City.Name}");

            builder.AppendLine();

            builder.AppendLine(
                "Date & Time          Temp      Condition          Rain");

            builder.AppendLine(
                "--------------------------------------------------------");

            foreach (var item in forecast.Items)
            {
                var condition =
                    item.Weather.Count > 0
                        ? item.Weather[0].Description
                        : "Unknown";

                var precipitation =
                    item.ProbabilityOfPrecipitation * 100;

                var temperature =
                    ConvertTemperature(
                        item.Main.Temperature,
                        unit);

                builder.AppendLine(
                    $"{item.DateTimeText,-20}" +
                    $"{temperature,7:F1} °{unit}   " +
                    $"{condition,-18}" +
                    $"{precipitation,5:F0}%");

                if (precipitation >= 60)
                {
                    builder.AppendLine(
                        "RAIN ALERT: High probability of precipitation.");
                }

                if (item.Main.Temperature > 35)
                {
                    builder.AppendLine();

                    builder.AppendLine(
                        "WEATHER ALERT:");

                    builder.AppendLine(
                        "High temperature detected.");
                }
            }

            return builder.ToString();
        }

        public string FormatForecastSummary(
            List<ForecastItemDto> items,
            string unit)
        {
            if (items.Count == 0)
            {
                return
                    "No forecast entries available for the selected date.";
            }

            var highestTemperature =
                items.Max(
                    item => item.Main.Temperature);

            var lowestTemperature =
                items.Min(
                    item => item.Main.Temperature);

            var averageTemperature =
                items.Average(
                    item => item.Main.Temperature);

            var highestRainProbability =
                items.Max(
                    item =>
                        item.ProbabilityOfPrecipitation * 100);

            var highest =
                ConvertTemperature(
                    highestTemperature,
                    unit);

            var lowest =
                ConvertTemperature(
                    lowestTemperature,
                    unit);

            var average =
                ConvertTemperature(
                    averageTemperature,
                    unit);

            var builder =
                new StringBuilder();

            builder.AppendLine();

            builder.AppendLine(
                "FORECAST SUMMARY");

            builder.AppendLine(
                "----------------------------------------");

            builder.AppendLine(
                $"Highest Temperature : " +
                $"{highest:F1} °{unit}");

            builder.AppendLine(
                $"Lowest Temperature  : " +
                $"{lowest:F1} °{unit}");

            builder.AppendLine(
                $"Average Temperature : " +
                $"{average:F1} °{unit}");

            builder.AppendLine(
                $"Highest Rain Chance : " +
                $"{highestRainProbability:F0} %");

            return builder.ToString();
        }

        public string FormatDashboard(
            CurrentWeatherDto currentWeather,
            ForecastDto forecast,
            string unit)
        {
            var currentTemperature =
                ConvertTemperature(
                    currentWeather.Main.Temperature,
                    unit);

            var currentFeelsLike =
                ConvertTemperature(
                    currentWeather.Main.FeelsLike,
                    unit);

            var currentCondition =
                currentWeather.Weather.Count > 0
                    ? currentWeather.Weather[0].Description
                    : "Unknown";

            var builder =
                new StringBuilder();

            builder.AppendLine();

            builder.AppendLine(
                "========================================");

            builder.AppendLine(
                "WEATHER DASHBOARD");

            builder.AppendLine(
                "========================================");

            builder.AppendLine();

            builder.AppendLine(
                $"City: {currentWeather.Name}");

            builder.AppendLine();

            builder.AppendLine(
                "CURRENT WEATHER");

            builder.AppendLine(
                "----------------------------------------");

            builder.AppendLine(
                $"Temperature : " +
                $"{currentTemperature:F1} °{unit}");

            builder.AppendLine(
                $"Feels Like  : " +
                $"{currentFeelsLike:F1} °{unit}");

            builder.AppendLine(
                $"Condition   : " +
                $"{currentCondition}");

            builder.AppendLine(
                $"Humidity    : " +
                $"{currentWeather.Main.Humidity}%");

            builder.AppendLine(
                $"Pressure    : " +
                $"{currentWeather.Main.Pressure} hPa");

            builder.AppendLine(
                $"Wind Speed  : " +
                $"{currentWeather.Wind.Speed:F1} m/s");

            if (currentWeather.Main.Temperature > 35)
            {
                builder.AppendLine();

                builder.AppendLine(
                    "WEATHER ALERT:");

                builder.AppendLine(
                    "High temperature detected.");
            }

            builder.AppendLine();


            builder.AppendLine(
                "FORECAST");

            builder.AppendLine(
                "----------------------------------------");

            foreach (var item in forecast.Items)
            {
                var temperature =
                    ConvertTemperature(
                        item.Main.Temperature,
                        unit);

                var condition =
                    item.Weather.Count > 0
                        ? item.Weather[0].Description
                        : "Unknown";

                var precipitation =
                    item.ProbabilityOfPrecipitation * 100;

                builder.AppendLine(
                    $"{item.DateTimeText,-20}" +
                    $"{temperature,6:F1} °{unit}   " +
                    $"{condition,-18}" +
                    $"{precipitation,4:F0}%");

                if (precipitation >= 60)
                {
                    builder.AppendLine(
                        "RAIN ALERT: High probability of precipitation.");
                }

                if (item.Main.Temperature > 35)
                {
                    builder.AppendLine(
                        "WEATHER ALERT:");

                    builder.AppendLine(
                        "High temperature detected.");
                }
            }

            return builder.ToString();
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
    }
}