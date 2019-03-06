namespace PcMonitor.DataObjects
{
    public class DiskModel
    {
        /// <summary>
        /// Gets or sets the name of the disk
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the total space
        /// </summary>
        public ulong Total { get; set; }

        /// <summary>
        /// Gets or sets the free space
        /// </summary>
        public ulong Free { get; set; }

        /// <summary>
        /// Gets or sets the used space
        /// </summary>
        public ulong Used => Total - Free;

        /// <summary>
        /// Gets the values in a readable format for the view
        /// </summary>
        public string View =>
            $"{Used.GetDisplayValue(Helper.UnitType.Byte)} / {Total.GetDisplayValue(Helper.UnitType.Byte)} ({Helper.CalculatePercentage(Total, Used):N0}%)";

        /// <summary>
        /// Gets the current usage percentage
        /// </summary>
        public double UsagePercentage => Helper.CalculatePercentage(Total, Used);
    }
}
