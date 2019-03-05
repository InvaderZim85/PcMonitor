using System.Windows.Controls;
using PcMonitor.DataObjects.Weather;

namespace PcMonitor.Ui.Controls
{
    /// <summary>
    /// Interaction logic for WeatherControl.xaml
    /// </summary>
    public partial class WeatherControl : UserControl
    {
        /// <summary>
        /// Creates a new instance of the <see cref="WeatherControl"/>
        /// </summary>
        public WeatherControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the weather 
        /// </summary>
        /// <param name="weather">The current weather</param>
        public void SetWeather(WeatherMainModel weather)
        {
            if (DataContext is WeatherControlViewModel viewModel)
                viewModel.SetWeather(weather);
        }
    }
}
