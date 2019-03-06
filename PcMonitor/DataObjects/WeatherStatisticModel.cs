using System;

namespace PcMonitor.DataObjects
{
    public class WeatherStatisticModel
    {
        /// <summary>
        /// Gets or sets the id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the location
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the weather title
        /// </summary>
        public string Weather { get; set; }

        /// <summary>
        /// Gets or sets the description of the weather
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the temperature
        /// </summary>
        public double Temperature { get; set; }

        /// <summary>
        /// Gets or sets the min. temperature
        /// </summary>
        public double TemperatureMin { get; set; }

        /// <summary>
        /// Gets or sets the max. temperature
        /// </summary>
        public double TemperatureMax { get; set; }

        /// <summary>
        /// Gets or sets the pressure in hPa
        /// </summary>
        public int Pressure { get; set; }

        /// <summary>
        /// Gets or sets the humidity in percent
        /// </summary>
        public int Humidity { get; set; }

        /// <summary>
        /// Gets or sets the rain volume in mm
        /// </summary>
        public double Rain { get; set; }

        /// <summary>
        /// Gets or sets the snow volume in mm
        /// </summary>
        public double Snow { get; set; }

        /// <summary>
        /// Gets or sets the calculation date of the data
        /// </summary>
        public DateTime CalculationDate { get; set; }
    }
}
