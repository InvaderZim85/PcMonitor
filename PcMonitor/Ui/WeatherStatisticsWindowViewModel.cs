using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using LiveCharts;
using MahApps.Metro.Controls.Dialogs;
using PcMonitor.Data;
using PcMonitor.DataObjects;
using WpfBase;

namespace PcMonitor.Ui
{
    public class WeatherStatisticsWindowViewModel : ObservableObject
    {
        /// <summary>
        /// Contains the instance of the mahapps dialog coordinator
        /// </summary>
        private IDialogCoordinator _dialogCoordinator;

        /// <summary>
        /// Contains the complete statistic values
        /// </summary>
        private List<WeatherStatisticModel> _statistics;

        /// <summary>
        /// Backing field for <see cref="LocationList"/>
        /// </summary>
        private ObservableCollection<string> _locationList;

        /// <summary>
        /// Gets or sets the location list
        /// </summary>
        public ObservableCollection<string> LocationList
        {
            get => _locationList;
            set => SetField(ref _locationList, value);
        }

        /// <summary>
        /// Backing field for <see cref="SelectedLocation"/>
        /// </summary>
        private string _selectedLocation;

        /// <summary>
        /// Gets or sets the selected location
        /// </summary>
        public string SelectedLocation
        {
            get => _selectedLocation;
            set => SetField(ref _selectedLocation, value);
        }

        /// <summary>
        /// Backing field for <see cref="HasValues"/>
        /// </summary>
        private bool _hasValues;

        /// <summary>
        /// Gets or sets the value which indicates if the location has values
        /// </summary>
        public bool HasValues
        {
            get => _hasValues;
            set => SetField(ref _hasValues, value);
        }

        /// <summary>
        /// Backing field for <see cref="Labels"/>
        /// </summary>
        private string[] _labels;

        /// <summary>
        /// Gets or sets the label of the x axis
        /// </summary>
        public string[] Labels
        {
            get => _labels;
            set => SetField(ref _labels, value);
        }

        /// <summary>
        /// Backing field for <see cref="YFormatter"/>
        /// </summary>
        private Func<double, string> _yFormatter;

        /// <summary>
        /// Gets or sets the formatter of the series (y-axis)
        /// </summary>
        public Func<double, string> YFormatter
        {
            get => _yFormatter;
            set => SetField(ref _yFormatter, value);
        }

        /// <summary>
        /// Backing field for <see cref="TempValues"/>
        /// </summary>
        private IChartValues _tempValues;

        /// <summary>
        /// Gets or sets the temperature values
        /// </summary>
        public IChartValues TempValues
        {
            get => _tempValues;
            set => SetField(ref _tempValues, value);
        }

        /// <summary>
        /// Backing field for <see cref="PressureValues"/>
        /// </summary>
        private IChartValues _pressureValues;

        /// <summary>
        /// Gets or sets the pressure values
        /// </summary>
        public IChartValues PressureValues
        {
            get => _pressureValues;
            set => SetField(ref _pressureValues, value);
        }

        /// <summary>
        /// Backing field for <see cref="HumidityValues"/>
        /// </summary>
        private IChartValues _humidityValues;

        /// <summary>
        /// Gets or sets the humidity values
        /// </summary>
        public IChartValues HumidityValues
        {
            get => _humidityValues;
            set => SetField(ref _humidityValues, value);
        }

        /// <summary>
        /// Backing field for <see cref="ShowTemp"/>
        /// </summary>
        private bool _showTemp = true;

        /// <summary>
        /// Gets or sets the value which indicates if the temperature should be shown
        /// </summary>
        public bool ShowTemp
        {
            get => _showTemp;
            set => SetField(ref _showTemp, value);
        }

        /// <summary>
        /// Backing field for <see cref="ShowPressure"/>
        /// </summary>
        private bool _showPressure = true;

        /// <summary>
        /// Gets or sets the value which indicates if the pressure values should be shown
        /// </summary>
        public bool ShowPressure
        {
            get => _showPressure;
            set => SetField(ref _showPressure, value);
        }

        /// <summary>
        /// Backing field for <see cref="ShowHumidity"/>
        /// </summary>
        private bool _showHumidity = true;

        /// <summary>
        /// Gets or sets the value which indicates if the humidity values should be shown
        /// </summary>
        public bool ShowHumidity
        {
            get => _showHumidity;
            set => SetField(ref _showHumidity, value);
        }

        /// <summary>
        /// Backing field for <see cref="FromDate"/>
        /// </summary>
        private DateTime _fromDate;

        /// <summary>
        /// Gets or sets the from date
        /// </summary>
        public DateTime FromDate
        {
            get => _fromDate;
            set => SetField(ref _fromDate, value);
        }

        /// <summary>
        /// Backing field for <see cref="TillDate"/>
        /// </summary>
        private DateTime _tillDate;

        /// <summary>
        /// Gets or sets the till date
        /// </summary>
        public DateTime TillDate
        {
            get => _tillDate;
            set => SetField(ref _tillDate, value);
        }

        /// <summary>
        /// Init the view model
        /// </summary>
        /// <param name="dialogCoordinator">The dialog coordinator</param>
        public void InitViewModel(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;
        }

        /// <summary>
        /// The command to show the values for the selected location
        /// </summary>
        public ICommand ShowDataCommand => new DelegateCommand(ShowValues);

        /// <summary>
        /// The command to filter the values
        /// </summary>
        public ICommand ExecuteCommand => new DelegateCommand(ShowValues);

        /// <summary>
        /// Loads the statistic data
        /// </summary>
        public async void LoadData()
        {
            try
            {
                _statistics = WeatherRepo.LoadWeatherStatistics();

                LocationList = new ObservableCollection<string>(_statistics.Select(s => s.Location).GroupBy(g => g).Select(s => s.Key));
                FromDate = _statistics.Min(m => m.CalculationDate);
                TillDate = _statistics.Max(m => m.CalculationDate);

                if (LocationList.Count == 1)
                {
                    SelectedLocation = LocationList.FirstOrDefault();
                    ShowValues();
                }
            }
            catch (Exception ex)
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Error",
                    $"An error has occured.\r\n\r\nMessage: {ex.Message}");
            }
            
        }

        /// <summary>
        /// Shows the values
        /// </summary>
        private async void ShowValues()
        {
            if (string.IsNullOrEmpty(SelectedLocation))
                return;

            try
            {
                var values = _statistics.Where(w => w.Location.Equals(SelectedLocation) &&
                                                    w.CalculationDate >= FromDate &&
                                                    w.CalculationDate <= TillDate).ToList();

                HasValues = values.Any();

                TempValues = new ChartValues<double>(values.Select(s => s.Temperature));
                PressureValues = new ChartValues<int>(values.Select(s => s.Pressure));
                HumidityValues = new ChartValues<int>(values.Select(s => s.Humidity));

                Labels = values.Select(s => s.CalculationDate.ToString("G")).ToArray();
                YFormatter = value => value.ToString("N2");
            }
            catch (Exception ex)
            {
                await _dialogCoordinator.ShowMessageAsync(this, "Error",
                    $"An error has occured while preparing the values.\r\n\r\nMessage: {ex.Message}");
            }
        }
    }
}
