using RestSharp.Deserializers;

namespace PcMonitor.DataObjects.Weather
{
    public class SnowModel
    {
        /// <summary>
        /// Gets or sets the snow volume for the last hour (in mm)
        /// </summary>
        [DeserializeAs(Name = "1h")]
        public int OneHour { get; set; }

        /// <summary>
        /// Gets or sets the snow volume for the last three hours (in mm)
        /// </summary>
        [DeserializeAs(Name = "3h")]
        public int ThreeHour { get; set; }
    }
}
