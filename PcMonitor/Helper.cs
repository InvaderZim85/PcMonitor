using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Windows.Documents;
using Newtonsoft.Json;
using PcMonitor.DataObjects;
using PcMonitor.DataObjects.Weather;
using RestSharp;

namespace PcMonitor
{
    public static class Helper
    {
        /// <summary>
        /// The different unit types
        /// </summary>
        public enum UnitType
        {
            Byte,
            KiloByte,
        }

        /// <summary>
        /// Loads the current cpu usage via WMI
        /// </summary>
        /// <returns>The cpu usage in percent</returns>
        public static int GetCpuUsage()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_PerfFormattedData_PerfOS_Processor"))
                {
                    var cpuTimes = searcher.Get().OfType<ManagementObject>().Select(s => new
                    {
                        Name = s["Name"],
                        Usage = s["PercentProcessorTime"]
                    }).ToList();

                    var total = cpuTimes.FirstOrDefault(f => f.Name.Equals("_Total"))?.Usage;

                    return total == null ? 0 : (int) (ulong) total;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// Loads the current ram usage
        /// </summary>
        /// <returns>The ram usage</returns>
        public static (ulong free, ulong used, ulong total) GetRamUsage()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_OperatingSystem"))
                {
                    var values = searcher.Get().OfType<ManagementObject>().Select(s => new
                    {
                        Free = s["FreePhysicalMemory"],
                        Total = s["TotalVisibleMemorySize"]
                    });

                    var value = values.FirstOrDefault();

                    if (value == null)
                        return (0, 0, 0);

                    var free = (ulong)value.Free;
                    var total = (ulong)value.Total;
                    var used = total - free;

                    return (free, used, total);


                }
            }
            catch (Exception)
            {
                return (0, 0, 0);
            }
        }

        /// <summary>
        /// Loads the current disk usage
        /// </summary>
        /// <returns>The list of disk usage</returns>
        public static List<DiskModel> GetDiskUsage()
        {
            try
            {
                using (var searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_LogicalDisk"))
                {
                    var values = searcher.Get().OfType<ManagementObject>().Select(s => new
                    {
                        Name = s["Name"],
                        Total = s["Size"],
                        Free = s["FreeSpace"],
                        MediaType = s["MediaType"]
                    }).ToList();

                    return values.Where(w => w.MediaType != null && (uint)w.MediaType == 12).Select(s => new DiskModel
                    {
                        Name = s.Name.ToString(),
                        Free = (ulong)s.Free,
                        Total = (ulong)s.Total
                    }).ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Converts a given byte or kilobyte value in a "normal" format for the view
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="initialUnit">The unit of the value</param>
        /// <param name="showDecimals">true to show decimal places, otherwise false</param>
        /// <returns>The converted value for the view</returns>
        public static string GetDisplayValue(this ulong value, UnitType initialUnit, bool showDecimals = false)
        {
            double kb;

            if (initialUnit == UnitType.Byte)
                kb = (double)value / 1024;
            else
                kb = value;

            if (kb < 1024)
            {
                return showDecimals ? $"{kb:N2}KB" : $"{kb:N0}KB";
            }

            var mb = kb / 1024;
            if (mb < 1024)
            {
                return showDecimals ? $"{mb:N2}MB" : $"{mb:N0}MB";
            }

            var gb = mb / 1024;
            return showDecimals ? $"{gb:N2}GB" : $"{gb:N0}GB";
        }

        /// <summary>
        /// Calculates the percentage
        /// </summary>
        /// <param name="total">The total value</param>
        /// <param name="used">The used value</param>
        /// <returns>The percentage</returns>
        public static double CalculatePercentage(double total, double used)
        {
            return Math.Abs(total) <= 0 ? 0 : 100 / total * used;
        }

        /// <summary>
        /// Converts a unix utc value into a date time
        /// </summary>
        /// <param name="value">The unix utc value</param>
        /// <returns>The converted date time</returns>
        public static DateTime FromUnixUtc(double value)
        {
            var unixStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return unixStart.AddSeconds(value).ToLocalTime();
        }

        /// <summary>
        /// Returns the base folder of the application
        /// </summary>
        /// <returns>The base folder</returns>
        public static string GetBaseFolder()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        /// <summary>
        /// Writes an object into a file
        /// </summary>
        /// <param name="file">The filepath</param>
        /// <param name="data">The data</param>
        /// <returns>true when successful, otherwise false</returns>
        public static bool WriteData(string file, object data)
        {
            if (string.IsNullOrEmpty(file))
                return false;

            if (data == null)
                return false;

            var content = JsonConvert.SerializeObject(data, Formatting.Indented);

            File.WriteAllText(file, content);

            return File.Exists(file);
        }

        /// <summary>
        /// Loads the settings
        /// </summary>
        /// <returns>The settings</returns>
        public static SettingsModel LoadSettings()
        {
            var baseDir = GetBaseFolder();

            if (string.IsNullOrEmpty(baseDir))
                return null;

            var file = Path.Combine(baseDir, "Settings.json");

            var content = File.ReadAllText(file);

            return JsonConvert.DeserializeObject<SettingsModel>(content);
        }
    }
}
