using RestSharp.Deserializers;

namespace PcMonitor.DataObjects.Weather
{
    public class RainModel
    {
        /// <summary>
        /// Gets or sets the rain volume of the last hour (in mm)
        /// </summary>
        [DeserializeAs(Name = "1h")]
        public int OneHour { get; set; }

        /// <summary>
        /// Gets or sets the rain volume of the last three hours (in mm)
        /// </summary>
        [DeserializeAs(Name = "3h")]
        public int ThreeHour { get; set; }
    }
}
