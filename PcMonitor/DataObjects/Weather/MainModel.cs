using RestSharp.Deserializers;

namespace PcMonitor.DataObjects.Weather
{
    public class MainModel
    {
        /// <summary>
        /// Gets or sets the current temperature
        /// </summary>
        public double Temp { get; set; }

        /// <summary>
        /// Gets or sets the pressure in hPa
        /// </summary>
        public int Pressure { get; set; }

        /// <summary>
        /// Gets or sets the humidity (in percent)
        /// </summary>
        public int Humidity { get; set; }

        /// <summary>
        /// Gets or sets the minimal temperature
        /// </summary>
        [DeserializeAs(Name = "temp_min")]
        public double TempMin { get; set; }

        /// <summary>
        /// Gets or sets the maximal temperature
        /// </summary>
        [DeserializeAs(Name = "temp_max")]
        public double TempMax { get; set; }

        /// <summary>
        /// Gets or sets the pressure at sea level (in hPa)
        /// </summary>
        [DeserializeAs(Name = "sea_level")]
        public int SeaLevel { get; set; }

        /// <summary>
        /// Gets or sets the pressure at ground level (in hPa)
        /// </summary>
        [DeserializeAs(Name = "grnd_level")]
        public int GroundLevel { get; set; }
    }
}
