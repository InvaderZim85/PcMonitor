using System;
using System.Collections.Generic;

namespace PcMonitor.DataObjects.Weather
{
    public class WeatherMainModel
    {
        /// <summary>
        /// Gets or sets the coordinates
        /// </summary>
        public CoordModel Coord { get; set; }

        /// <summary>
        /// Gets or sets the weather informations
        /// </summary>
        public List<WeatherModel> Weather { get; set; }

        /// <summary>
        /// Gets or sets base parameter (internal)
        /// </summary>
        public string Base { get; set; }

        /// <summary>
        /// Gets or sets the main values
        /// </summary>
        public MainModel Main { get; set; }

        /// <summary>
        /// Gets or sets the visibility in meters
        /// </summary>
        public int Visibility { get; set; }

        /// <summary>
        /// Gets or sets the wind information
        /// </summary>
        public WindModel Wind { get; set; }

        /// <summary>
        /// Gets or sets the cloud information
        /// </summary>
        public CloudModel Clouds { get; set; }

        /// <summary>
        /// Gets or sets the time of data calculation (unix, utc)
        /// </summary>
        public int Dt { get; set; }

        /// <summary>
        /// Gets or sets the sys information
        /// </summary>
        public SysModel Sys { get; set; }

        /// <summary>
        /// Gets or sets the city id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the city name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the cod (internal parameter)
        /// </summary>
        public int Cod { get; set; }

        /// <summary>
        /// Gets or sets the rain values
        /// </summary>
        public RainSnowModel Rain { get; set; }

        /// <summary>
        /// Gets or sets the snow values
        /// </summary>
        public RainSnowModel Snow { get; set; }

        /// <summary>
        /// Gets or sets the time stamp
        /// </summary>
        public DateTime TimeStamp { get; set; }
    }
}
