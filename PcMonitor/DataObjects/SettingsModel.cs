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

        /// <summary>
        /// Gets or sets the db user
        /// </summary>
        public string DbServer { get; set; }

        /// <summary>
        /// Gets or sets the db port
        /// </summary>
        public int DbPort { get; set; }

        /// <summary>
        /// Gets or sets the db user
        /// </summary>
        public string DbUser { get; set; }

        /// <summary>
        /// Gets or sets the password of the db user
        /// </summary>
        public string DbPassword { get; set; }

        /// <summary>
        /// Gets or sets the name of the database
        /// </summary>
        public string DbDatabase { get; set; }
    }
}
