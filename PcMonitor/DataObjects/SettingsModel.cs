namespace PcMonitor.DataObjects
{
    public class SettingsModel
    {
        /// <summary>
        /// Gets or sets the api key for the weather data
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets the location for the weather data
        /// </summary>
        public string Location { get; set; }
    }
}
