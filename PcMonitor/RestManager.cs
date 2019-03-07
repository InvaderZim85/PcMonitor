using System;
using PcMonitor.DataObjects.Weather;
using RestSharp;

namespace PcMonitor
{
    public static class RestManager
    {
        /// <summary>
        /// Loads the weather from openweathermap.org
        /// </summary>
        /// <returns>The current weather</returns>
        public static WeatherMainModel GetWeather()
        {
            if (!Helper.SettingsLoaded)
                return new WeatherMainModel();
            try
            {
                var client = new RestClient("http://api.openweathermap.org");

                var request = new RestRequest("data/2.5/weather");
                request.AddQueryParameter("appid", Helper.Settings.ApiKey); // API Key
                request.AddQueryParameter("q", Helper.Settings.Location);
                request.AddQueryParameter("units", "metric");

                var response = client.Execute<WeatherMainModel>(request);

                response.Data.TimeStamp = DateTime.Now;

                return response.Data;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
