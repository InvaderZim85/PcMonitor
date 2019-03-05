namespace PcMonitor.DataObjects.Weather
{
    public class WindModel
    {
        /// <summary>
        /// Gets or sets the wind speed (m/s)
        /// </summary>
        public double Speed { get; set; }

        /// <summary>
        /// Gets or sets the wind direction (meteorological)
        /// </summary>
        public int Deg { get; set; }
    }
}
