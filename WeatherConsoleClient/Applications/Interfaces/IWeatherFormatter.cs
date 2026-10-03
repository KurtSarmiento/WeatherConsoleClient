using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherConsoleClient.Applications.DTOs;

namespace WeatherConsoleClient.Applications.Interfaces
{
    public interface IWeatherFormatter
    {
        string FormatCurrentWeather(
            CurrentWeatherDto weather);

        string FormatCurrentWeather(
            CurrentWeatherDto weather,
            string unit);

        string FormatForecast(
            ForecastDto forecast);

        string FormatForecast(
            ForecastDto forecast,
            string unit);

        string FormatForecastSummary(
            List<ForecastItemDto> items,
            string unit);
    }
}
