using System;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.IconPacks;
using PcMonitor.DataObjects;
using PcMonitor.DataObjects.Weather;
using WpfBase;

namespace PcMonitor.Ui
{
    public class MainWindowViewModel : ObservableObject
    {
        /// <summary>
        /// Contains the mahapps dialog coordinator
        /// </summary>
        private IDialogCoordinator _dialogCoordinator;

        /// <summary>
        /// Contains the settings
        /// </summary>
        private SettingsModel _settings;

        /// <summary>
        /// Contains the action which sets the window state
        /// </summary>
        private Action<bool> _setWindow;

        /// <summary>
        /// Contains the action which sets the weather
        /// </summary>
        private Action<WeatherMainModel> _setWeather;

        /// <summary>
        /// Contains the counter for the weather data to load them only all 2 minutes
        /// </summary>
        private int _weatherCounter;

        /// <summary>
        /// Contains the timer
        /// </summary>
        private readonly Timer _timer = new Timer(500);

        /// <summary>
        /// Contains the timer for the clock
        /// </summary>
        private readonly Timer _clockTimer = new Timer(1000);

        /// <summary>
        /// Contains the value which indicates if the timer is currently running
        /// </summary>
        private bool _timerRunning;

        /// <summary>
        /// Contains the value which indicates if the application is closing
        /// </summary>
        private bool _close;

        /// <summary>
        /// Contains the value which indicates if the volume was muted
        /// </summary>
        private bool _volumeMuted;

        /// <summary>
        /// Backing field for <see cref="FullScreen"/>
        /// </summary>
        private bool _fullScreen;

        /// <summary>
        /// Gets or sets the value which indicates if the window should be shown in fullscreen
        /// </summary>
        public bool FullScreen
        {
            get => _fullScreen;
            set
            {
                SetField(ref _fullScreen, value);
                _setWindow(value);
            }
        }

        /// <summary>
        /// Backing field for <see cref="Clock"/>
        /// </summary>
        private string _clock = "Clock";

        /// <summary>
        /// Gets or sets the clock
        /// </summary>
        public string Clock
        {
            get => _clock;
            set => SetField(ref _clock, value);
        }

        /// <summary>
        /// Backing field for <see cref="CpuUsage"/>
        /// </summary>
        private string _cpuUsage = "Initialize...";

        /// <summary>
        /// Gets or sets the current cpu usage
        /// </summary>
        public string CpuUsage
        {
            get => _cpuUsage;
            set => SetField(ref _cpuUsage, value);
        }

        /// <summary>
        /// Backing field for <see cref="CpuUsageValue"/>
        /// </summary>
        private int _cpuUsageValue;

        /// <summary>
        /// Gets or sets the cpu usage in percent
        /// </summary>
        public int CpuUsageValue
        {
            get => _cpuUsageValue;
            set => SetField(ref _cpuUsageValue, value);
        }

        /// <summary>
        /// Backing field for <see cref="RamUsage"/>
        /// </summary>
        private string _ramUsage = "Initialize...";

        /// <summary>
        /// Gets or sets the ram usage
        /// </summary>
        public string RamUsage
        {
            get => _ramUsage;
            set => SetField(ref _ramUsage, value);
        }

        /// <summary>
        /// Backing field for <see cref="RamTotal"/>
        /// </summary>
        private double _ramTotal = 100;

        /// <summary>
        /// Gets or sets the total amount of available ram (in GB)
        /// </summary>
        public double RamTotal
        {
            get => _ramTotal;
            set => SetField(ref _ramTotal, value);
        }

        /// <summary>
        /// Backing field for <see cref="RamUsed"/>
        /// </summary>
        private double _ramUsed;

        /// <summary>
        /// Gets or sets the used ram (in gb)
        /// </summary>
        public double RamUsed
        {
            get => _ramUsed;
            set => SetField(ref _ramUsed, value);
        }

        /// <summary>
        /// Backing field for <see cref="Volume"/>
        /// </summary>
        private string _volume;

        /// <summary>
        /// Gets or sets the current master volume
        /// </summary>
        public string Volume
        {
            get => _volume;
            set => SetField(ref _volume, value);
        }

        /// <summary>
        /// Backing field for <see cref="VolumeLevel"/>
        /// </summary>
        private double _volumeLevel;

        /// <summary>
        /// Gets or sets the current master volume
        /// </summary>
        public double VolumeLevel
        {
            get => _volumeLevel;
            set => SetField(ref _volumeLevel, value);
        }

        /// <summary>
        /// Backing field for <see cref="VolumeImage"/>
        /// </summary>
        private PackIconMaterialKind _volumeImage = PackIconMaterialKind.VolumeHigh;

        /// <summary>
        /// Gets or sets the image for the volume button
        /// </summary>
        public PackIconMaterialKind VolumeImage
        {
            get => _volumeImage;
            set => SetField(ref _volumeImage, value);
        }

        /// <summary>
        /// Backing field for <see cref="HddList"/>
        /// </summary>
        private ObservableCollection<DiskModel> _hddList = new ObservableCollection<DiskModel>();

        /// <summary>
        /// Gets or sets the hdd list
        /// </summary>
        public ObservableCollection<DiskModel> HddList
        {
            get => _hddList;
            set => SetField(ref _hddList, value);
        }

        /// <summary>
        /// Init the view model
        /// </summary>
        /// <param name="dialogCoordinator">The mahapps dialog coordinator</param>
        /// <param name="setWindow">The action to set the window state</param>
        /// <param name="setWeather">The action to set the weather</param>
        public void InitViewModel(IDialogCoordinator dialogCoordinator, Action<bool> setWindow, Action<WeatherMainModel> setWeather)
        {
            _dialogCoordinator = dialogCoordinator;
            _setWindow = setWindow;
            _setWeather = setWeather;

            _settings = Helper.LoadSettings();

            StartTimer();
        }

        /// <summary>
        /// The command to set the volume
        /// </summary>
        public ICommand SetVolumeCommand => new DelegateCommand(SetVolumeLevel);

        /// <summary>
        /// Starts the timer
        /// </summary>
        private void StartTimer()
        {
            SetVolume();
            SetClock();
            SetWeather();

            _clockTimer.Elapsed += (s, e) =>
            {
                if (_close)
                {
                    _clockTimer.Stop();
                    return;
                }

                SetClock();
            };

            _timer.Elapsed += (s, e) =>
            {
                _timerRunning = true;
                _timer.Enabled = false;

                if (_close)
                    return;

                SetCpuUsage();
                SetRamUsage();
                SetHddList();
                SetVolume();

                if (_weatherCounter > 120)
                {
                    SetWeather();
                    _weatherCounter = 0;
                }
                _weatherCounter++;


                _timer.Enabled = true;
                _timerRunning = false;
            };

            _timer.Start();
            _clockTimer.Start();
        }

        /// <summary>
        /// Stops the timer
        /// </summary>
        public async void StopTimer()
        {
            var controller =
                await _dialogCoordinator.ShowProgressAsync(this, "Closing",
                    "Please wait while closing the application");

            controller.SetIndeterminate();

            while (_timerRunning)
            {
                // Wait while the timer is running...    
            }

            await controller.CloseAsync();
            _close = true;
            _timer?.Stop();
        }

        /// <summary>
        /// Sets the date and time
        /// </summary>
        private void SetClock()
        {
            Clock = DateTime.Now.ToString("G");
        }

        /// <summary>
        /// Sets the cpu usage
        /// </summary>
        private void SetCpuUsage()
        {
            CpuUsageValue = Helper.GetCpuUsage();
            CpuUsage = $"{CpuUsageValue}%";
        }

        /// <summary>
        /// Sets the ram usage
        /// </summary>
        private void SetRamUsage()
        {
            var (free, used, total) = Helper.GetRamUsage();

            RamTotal = total;
            RamUsed = used;

            RamUsage =
                $"{used.GetDisplayValue(Helper.UnitType.KiloByte, true)} / {total.GetDisplayValue(Helper.UnitType.KiloByte, true)} ({Helper.CalculatePercentage(total, used):N0}%)";
        }

        /// <summary>
        /// Sets the weather
        /// </summary>
        private void SetWeather()
        {
            var weather = RestManager.GetWeather(_settings.ApiKey, _settings.Location);
            _setWeather(weather);
        }

        /// <summary>
        /// Sets the hdd list
        /// </summary>
        private void SetHddList()
        {
            HddList = new ObservableCollection<DiskModel>(Helper.GetDiskUsage());
        }

        private void SetVolume()
        {
            VolumeLevel = AudioManager.GetMasterVolume();
            var muted = _volumeMuted ? " (muted)" : "";
            Volume = $"{VolumeLevel:N0}{muted}";

            if (_volumeMuted)
                VolumeImage = PackIconMaterialKind.VolumeMute;
            else
            {
                VolumeImage = VolumeLevel <= 0 ? PackIconMaterialKind.VolumeMute : PackIconMaterialKind.VolumeHigh;
                _volumeMuted = VolumeLevel <= 0;
            }
        }

        private void SetVolumeLevel()
        {
            AudioManager.SetMasterVolumeMute(!_volumeMuted);
            _volumeMuted = !_volumeMuted;

            VolumeImage = _volumeMuted ? PackIconMaterialKind.VolumeMute : PackIconMaterialKind.VolumeHigh;
        }
    }
}
