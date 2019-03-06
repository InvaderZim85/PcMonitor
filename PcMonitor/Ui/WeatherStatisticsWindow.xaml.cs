using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace PcMonitor.Ui
{
    /// <summary>
    /// Interaction logic for WeatherStatisticsWindow.xaml
    /// </summary>
    public partial class WeatherStatisticsWindow : MetroWindow
    {
        /// <summary>
        /// Creates a new instance of the <see cref="WeatherStatisticsWindow"/>
        /// </summary>
        public WeatherStatisticsWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Occurs when the window was loaded
        /// </summary>
        private void WeatherStatisticsWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is WeatherStatisticsWindowViewModel viewModel)
            {
                viewModel.InitViewModel(DialogCoordinator.Instance);
                viewModel.LoadData();
            }
        }
    }
}
