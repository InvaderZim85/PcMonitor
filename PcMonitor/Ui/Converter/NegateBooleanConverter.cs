using System;
using System.Globalization;
using System.Windows.Data;

namespace PcMonitor.Ui.Converter
{
    public class NegateBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Converts a bool 
        /// </summary>
        /// <param name="value">The original value</param>
        /// <param name="targetType">The target type</param>
        /// <param name="parameter">The parameters</param>
        /// <param name="culture">The current culture</param>
        /// <returns>The converted value</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool tmpValue)
                return !tmpValue;

            return false;
        }

        /// <summary>
        /// Converts the value back
        /// </summary>
        /// <param name="value">The original value</param>
        /// <param name="targetType">The target type</param>
        /// <param name="parameter">The parameters</param>
        /// <param name="culture">The current culture</param>
        /// <returns>The converted value</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool tmpValue)
                return !tmpValue;

            return false;
        }
    }
}
