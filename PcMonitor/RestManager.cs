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
        /// <param name="apiKey">The api key</param>
        /// <param name="location">The desired location (CityName,Country)</param>
        /// <returns>The current weather</returns>
        public static WeatherMainModel GetWeather(string apiKey, string location)
        {
            try
            {
                var client = new RestClient("http://api.openweathermap.org");

                var request = new RestRequest("data/2.5/weather");
                request.AddQueryParameter("appid", apiKey); // API Key
                request.AddQueryParameter("q", location);
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
