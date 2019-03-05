using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MahApps.Metro.IconPacks;
using PcMonitor.DataObjects.Weather;
using WpfBase;

namespace PcMonitor.Ui.Controls
{
    public class WeatherControlViewModel : ObservableObject
    {
        /// <summary>
        /// Contains the current weather
        /// </summary>
        private WeatherMainModel _weather;

        /// <summary>
        /// Contains the weather dictionary
        /// </summary>
        private Dictionary<int, WeatherMainModel> _weatherDictionary = new Dictionary<int, WeatherMainModel>();

        /// <summary>
        /// Backing field for <see cref="UpdateTime"/>
        /// </summary>
        private string _updateTime = "Last update: xx:xx:xx";

        /// <summary>
        /// Gets or sets the last update time
        /// </summary>
        public string UpdateTime
        {
            get => _updateTime;
            set => SetField(ref _updateTime, value);
        }

        /// <summary>
        /// Backing field for <see cref="SunRiseSet"/>
        /// </summary>
        private string _sunRiseSet;

        /// <summary>
        /// Gets or sets the sunrise / sunset time
        /// </summary>
        public string SunRiseSet
        {
            get => _sunRiseSet;
            set => SetField(ref _sunRiseSet, value);
        }

        /// <summary>
        /// Backing field for <see cref="Temperature"/>
        /// </summary>
        private string _temperature;

        /// <summary>
        /// Gets or sets the current temperature
        /// </summary>
        public string Temperature
        {
            get => _temperature;
            set => SetField(ref _temperature, value);
        }

        /// <summary>
        /// Backing field for <see cref="Clouds"/>
        /// </summary>
        private string _clouds;

        /// <summary>
        /// Gets or sets the current cloudiness (in percent)
        /// </summary>
        public string Clouds
        {
            get => _clouds;
            set => SetField(ref _clouds, value);
        }

        /// <summary>
        /// Backing field for <see cref="Humidity"/>
        /// </summary>
        private string _humidity;

        /// <summary>
        /// Gets or sets the humidity (in percent)
        /// </summary>
        public string Humidity
        {
            get => _humidity;
            set => SetField(ref _humidity, value);
        }

        /// <summary>
        /// Backing field for <see cref="Wind"/>
        /// </summary>
        private string _wind;

        /// <summary>
        /// Gets or sets the current wind values
        /// </summary>
        public string Wind
        {
            get => _wind;
            set => SetField(ref _wind, value);
        }

        /// <summary>
        /// Backing field for <see cref="Pressure"/>
        /// </summary>
        private string _pressure;

        /// <summary>
        /// Gets or sets the current pressure (in hPa)
        /// </summary>
        public string Pressure
        {
            get => _pressure;
            set => SetField(ref _pressure, value);
        }

        /// <summary>
        /// Backing field for <see cref="Image"/>
        /// </summary>
        private string _image = "http://openweathermap.org/img/w/10d.png";

        /// <summary>
        /// Gets or sets the weather image
        /// </summary>
        public string Image
        {
            get => _image;
            set => SetField(ref _image, value);
        }

        /// <summary>
        /// Backing field for <see cref="ShowError"/>
        /// </summary>
        private bool _showError;

        /// <summary>
        /// Gets or sets the value which indicates if the error message should be shown
        /// </summary>
        public bool ShowError
        {
            get => _showError;
            set => SetField(ref _showError, value);
        }

        /// <summary>
        /// Backing field for <see cref="Location"/>
        /// </summary>
        private string _location = "Weather for ...";

        /// <summary>
        /// Gets or sets the location
        /// </summary>
        public string Location
        {
            get => _location;
            set => SetField(ref _location, value);
        }

        /// <summary>
        /// Backing field for <see cref="LogWeather"/>
        /// </summary>
        private bool _logWeather;

        /// <summary>
        /// Gets or sets the value which indicates if the weather should be logged
        /// </summary>
        public bool LogWeather
        {
            get => _logWeather;
            set
            {
                SetField(ref _logWeather, value);
                SaveWeatherData();
            }
        }

        /// <summary>
        /// Backing field for <see cref="ImageToolTip"/>
        /// </summary>
        private string _imageToolTip = "";

        /// <summary>
        /// Gets or sets the image tool tip
        /// </summary>
        public string ImageToolTip
        {
            get => _imageToolTip;
            set => SetField(ref _imageToolTip, value);
        }

        /// <summary>
        /// Backing field for <see cref="TempImage"/>
        /// </summary>
        private PackIconMaterialKind _tempImage = PackIconMaterialKind.ArrowRight;

        /// <summary>
        /// Gets or sets the temperature arrow
        /// </summary>
        public PackIconMaterialKind TempImage
        {
            get => _tempImage;
            set => SetField(ref _tempImage, value);
        }

        /// <summary>
        /// Sets the weather
        /// </summary>
        /// <param name="weather">The current weather</param>
        public void SetWeather(WeatherMainModel weather)
        {
            _weather = weather;

            var nextId = _weatherDictionary.Count + 1;
            _weatherDictionary.Add(nextId, weather);

            SaveWeatherData();

            GetTempDirection(nextId);

            if (weather == null)
            {
                ShowError = true;
                return;
            }

            ShowError = false;

            Location = $"Weather for {_weather.Name}";

            var sunrise = Helper.FromUnixUtc(_weather.Sys.Sunrise);
            var sunset = Helper.FromUnixUtc(_weather.Sys.Sunset);

            SunRiseSet = $"{sunrise:HH:mm:ss} / {sunset:HH:mm:ss}";

            Temperature = $"{_weather.Main.Temp:N2}°C ({_weather.Main.TempMin}°C / {_weather.Main.TempMax}°C)";

            Clouds = $"{_weather.Clouds.All}%";

            Humidity = $"{weather.Main.Humidity}%";

            Wind = $"{_weather.Wind.Speed:N2}m/s ({_weather.Wind.Deg}°)";

            Pressure = $"{_weather.Main.Pressure}hPa";

            var lastCalcDate = Helper.FromUnixUtc(_weather.Dt);
            UpdateTime = $"Last update: {lastCalcDate:G}";

            if (_weather.Weather.Count > 0)
            {
                var values = _weather.Weather[0];

                Image = values.ImagePath;
                ImageToolTip = values.Main;
            }
            else
            {
                Image = "";
                ImageToolTip = "";
            }
        }

        /// <summary>
        /// Saves the weather data
        /// </summary>
        private void SaveWeatherData()
        {
            if (!LogWeather)
                return;

            var path = Path.Combine(Helper.GetBaseFolder(), $"WeatherData_{DateTime.Now:yyyyMMdd}.json");

            Helper.WriteData(path, _weatherDictionary.Select(s => s.Value).ToList());
        }

        /// <summary>
        /// Compares the current and the last weather to calculate the direction (up, down, etc.)
        /// </summary>
        /// <param name="lastId">The id of the last entry</param>
        private void GetTempDirection(int lastId)
        {
            if (lastId == 1)
            {
                TempImage = PackIconMaterialKind.ArrowRight;
                return;
            }

            if (_weatherDictionary.TryGetValue(lastId, out var current) &&
                _weatherDictionary.TryGetValue(lastId - 1, out var lastWeather))
            {
                var curTemp = (int) Math.Round(current.Main.Temp, 0);
                var lastTemp = (int) Math.Round(lastWeather.Main.Temp, 0);

                if (curTemp > lastTemp)
                    TempImage = PackIconMaterialKind.ArrowTopRight;
                else if (curTemp == lastTemp)
                    TempImage = PackIconMaterialKind.ArrowRight;
                else
                    TempImage = PackIconMaterialKind.ArrowBottomRight;
            }

        }
    }
}
