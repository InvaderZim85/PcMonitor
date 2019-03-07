using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using PcMonitor.DataObjects;
using PcMonitor.DataObjects.Weather;

namespace PcMonitor.Data
{
    public static class WeatherRepo
    {
        /// <summary>
        /// Saves the current weather
        /// </summary>
        /// <param name="weather">The weather</param>
        public static void InsertWeatherData(WeatherMainModel weather)
        {
            var calculationDate = Helper.FromUnixUtc(weather.Dt);
            if (ExistEntry(calculationDate))
                return;

            const string query =
                "INSERT INTO weatherData (location, weatherMain, weatherDescription, temperature, " +
                "temperaturemin, temperaturemax, pressure, humidity, calculationDate, rain, snow) " +
                "VALUES (@location, @main, @description, @temp, @min, @max, @pressure, @humidity, @date, @rain, @snow)";

            Connector.Connection.Execute(query, new
            {
                location = weather.Name,
                main = weather.Weather[0]?.Main ?? "",
                description = weather.Weather[0]?.Description ?? "",
                temp = weather.Main.Temp,
                min = weather.Main.TempMin,
                max = weather.Main.TempMax,
                pressure = weather.Main.Pressure,
                humidity = weather.Main.Humidity,
                date = calculationDate,
                rain = weather.Rain?.OneHour ?? 0,
                snow = weather.Snow?.OneHour ?? 0
            });
        }

        /// <summary>
        /// Checks if the entry already exists
        /// </summary>
        /// <param name="date">The calculation date of the entry</param>
        /// <returns>true when the entry already exists, otherwise false</returns>
        private static bool ExistEntry(DateTime date)
        {
            const string query = "SELECT COUNT(*) FROM weatherData WHERE calculationDate = @date";

            return Connector.Connection.QuerySingleOrDefault<int>(query, new {date}) > 0;
        }

        /// <summary>
        /// Loads the saved weather data
        /// </summary>
        /// <returns>The list with the data</returns>
        public static List<WeatherStatisticModel> LoadWeatherStatistics()
        {
            const string query = "SELECT id, location, weatherMain AS Weather, weatherDescription AS `Description`, " +
                                 "temperature, temperaturemin, temperaturemax, pressure, humidity, rain, snow, calculationDate " +
                                 "FROM weatherData";

            return Connector.Connection.Query<WeatherStatisticModel>(query).ToList();
        }
    }
}
