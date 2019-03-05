using System.ComponentModel;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using PcMonitor.DataObjects.Weather;

namespace PcMonitor.Ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the size of the window
        /// </summary>
        /// <param name="fullscreen">true for fullscreen, otherwise false</param>
        private void SetScreenSize(bool fullscreen)
        {
            WindowStyle = fullscreen ? WindowStyle.None : WindowStyle.SingleBorderWindow;
            WindowState = fullscreen ? WindowState.Maximized : WindowState.Normal;

            IgnoreTaskbarOnMaximize = fullscreen;
            ShowTitleBar = !fullscreen;
            ShowIconOnTitleBar = !fullscreen;
            ShowMaxRestoreButton = !fullscreen;
            ShowCloseButton = !fullscreen;
            ShowMinButton = !fullscreen;
        }

        /// <summary>
        /// Sets the weather control
        /// </summary>
        /// <param name="weather">The current weather</param>
        private void SetWeatherControl(WeatherMainModel weather)
        {
            Dispatcher.Invoke(() => WeatherView.SetWeather(weather));
        }

        /// <summary>
        /// Occurs when the window was loaded
        /// </summary>
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
                viewModel.InitViewModel(DialogCoordinator.Instance, SetScreenSize, SetWeatherControl);
        }

        /// <summary>
        /// Occurs when the window is closing
        /// </summary>
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
                viewModel.StopTimer();
        }
    }
}
