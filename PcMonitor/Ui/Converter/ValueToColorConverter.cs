using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PcMonitor.Ui.Converter
{
    public class ValueToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = Colors.Green;
            switch (value)
            {
                case double tmpDouble:
                    if (tmpDouble >= 60 && tmpDouble <= 79)
                        color = Colors.Orange;
                    else if (tmpDouble > 80)
                        color = Colors.Red;
                    break;
                case ulong tmpUlong:
                    if (tmpUlong >= 60 && tmpUlong < 79)
                        color = Colors.Orange;
                    else if (tmpUlong > 80)
                        color = Colors.Red;
                    break;
                case int tmpInt:
                    if (tmpInt >= 60 && tmpInt <= 79)
                        color = Colors.Orange;
                    else if (tmpInt > 80)
                        color = Colors.Red;
                    break;
            }

            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
