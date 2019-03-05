namespace PcMonitor.DataObjects.Weather
{
    public class SysModel
    {
        /// <summary>
        /// Gets or sets the internal type
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets the internal id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the internal message
        /// </summary>
        public double Message { get; set; }

        /// <summary>
        /// Gets or sets the country code (GB, JP, etc.)
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the sunrise (unix, utc)
        /// </summary>
        public int Sunrise { get; set; }

        /// <summary>
        /// Gets or sets the sunset (unix, utc)
        /// </summary>
        public int Sunset { get; set; }
    }
}
