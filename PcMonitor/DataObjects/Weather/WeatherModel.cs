namespace PcMonitor.DataObjects.Weather
{
    public class WeatherModel
    {
        /// <summary>
        /// Gets or sets the weather condition id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the main group of weather (Rain, Snow, Extreme, etc)
        /// </summary>
        public string Main { get; set; }
        
        /// <summary>
        /// Gets or sets the weather condition description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the weather icon
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Gets the image path of the weather icon
        /// </summary>
        public string ImagePath => $"http://openweathermap.org/img/w/{Icon}.png";
    }
}
