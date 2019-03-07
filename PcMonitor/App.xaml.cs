using System;
using System.Windows;
using PcMonitor.Data;

namespace PcMonitor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Occurs at the startup of the application
        /// </summary>
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            try
            {
                Helper.LoadSettings();

                Connector.CheckConnection();

                Helper.SettingsLoaded = true;
            }
            catch (Exception ex)
            {
                Helper.SettingsLoaded = false;
            }
            
        }
    }
}
