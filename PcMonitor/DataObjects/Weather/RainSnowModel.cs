using RestSharp.Deserializers;

namespace PcMonitor.DataObjects.Weather
{
    public class RainSnowModel
    {
        /// <summary>
        /// Gets or sets the rain / snow volume of the last hour (in mm)
        /// </summary>
        [DeserializeAs(Name = "1h")]
        public double OneHour { get; set; }

        /// <summary>
        /// Gets or sets the rain / snow volume of the last three hours (in mm)
        /// </summary>
        [DeserializeAs(Name = "3h")]
        public double ThreeHour { get; set; }
    }
}
