using System.Text;
using WeatherConsoleClient.Applications.DTOs;
using WeatherConsoleClient.Applications.Interfaces;

namespace WeatherConsoleClient.Presentation;

public class ConsoleWeatherFormatter : IWeatherFormatter
{
    public string FormatCurrentWeather(
        CurrentWeatherDto weather)
    {
        return FormatCurrentWeather(weather, "C");
    }

    public string FormatCurrentWeather(
        CurrentWeatherDto weather,
        string unit)
    {
        var condition =
            weather.Weather.Count > 0
                ? weather.Weather[0].Description
                : "Unknown";

        var temperature =
            ConvertTemperature(
                weather.Main.Temperature,
                unit);

        var feelsLike =
            ConvertTemperature(
                weather.Main.FeelsLike,
                unit);

        var builder = new StringBuilder();

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
            $"Temperature : {temperature:F1} °{unit}");

        builder.AppendLine(
            $"Feels Like  : {feelsLike:F1} °{unit}");

        builder.AppendLine(
            $"Condition   : {condition}");

        builder.AppendLine(
            $"Humidity    : {weather.Main.Humidity} %");

        builder.AppendLine(
            $"Pressure    : {weather.Main.Pressure} hPa");

        builder.AppendLine(
            $"Wind Speed  : {weather.Wind.Speed:F1} m/s");

        return builder.ToString();
    }

    public string FormatForecast(
        ForecastDto forecast)
    {
        return FormatForecast(forecast, "C");
    }

    public string FormatForecast(
        ForecastDto forecast,
        string unit)
    {
        var builder = new StringBuilder();

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
            return string.Empty;
        }

        var highestTemperature =
            items.Max(item =>
                item.Main.Temperature);

        var lowestTemperature =
            items.Min(item =>
                item.Main.Temperature);

        var averageTemperature =
            items.Average(item =>
                item.Main.Temperature);

        var highestRainProbability =
            items.Max(item =>
                item.ProbabilityOfPrecipitation * 100);

        var builder = new StringBuilder();

        builder.AppendLine();
        builder.AppendLine(
            "FORECAST SUMMARY");

        builder.AppendLine(
            "----------------------------------------");

        builder.AppendLine(
            $"Highest Temperature : " +
            $"{ConvertTemperature(highestTemperature, unit):F1} °{unit}");

        builder.AppendLine(
            $"Lowest Temperature  : " +
            $"{ConvertTemperature(lowestTemperature, unit):F1} °{unit}");

        builder.AppendLine(
            $"Average Temperature : " +
            $"{ConvertTemperature(averageTemperature, unit):F1} °{unit}");

        builder.AppendLine(
            $"Highest Rain Chance : " +
            $"{highestRainProbability:F0} %");

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