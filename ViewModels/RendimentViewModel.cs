
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WPF_MVVM_SPA_Template.Models;

namespace WPF_MVVM_SPA_Template.ViewModels
{
    class RendimentViewModel : INotifyPropertyChanged
    {
        private readonly Client _selectedClient;
        private double[] _deposits; // Store deposits

        public SeriesCollection ClientPerformanceSeries { get; set; }
        public ObservableCollection<string> Months { get; set; }
        private readonly MainViewModel _mainViewModel;

        private string _selectedChartType;
        public string SelectedChartType
        {
            get { return _selectedChartType; }
            set
            {
                if (_selectedChartType != value) // Only update if the value has changed
                {
                    _selectedChartType = value;
                    OnPropertyChanged();
                    Console.WriteLine($"SelectedChartType changed to: {_selectedChartType}"); // Debug output
                    UpdateClientPerformance(); // Redraw the chart on type change
                }
            }
        }

        public RelayCommand BarChartCommand { get; set; }
        public RelayCommand LineChartCommand { get; set; }
        public RelayCommand ReturnCommand { get; set; }

        public RendimentViewModel(Client selectedClient, MainViewModel mainViewModel)
        {
            _selectedClient = selectedClient;
            _mainViewModel = mainViewModel;

            // Initialize months (last 12 months)
            Months = new ObservableCollection<string>(GenerateLast12Months());

            // Generate random deposits data once when the client is selected
            _deposits = GenerateRandomDeposits();

            // Default to Bar Chart
            SelectedChartType = "Bar Chart";
            UpdateClientPerformance(); // Initial performance data

            BarChartCommand = new RelayCommand(x => BarCommand());
            LineChartCommand = new RelayCommand(x => LineCommand());
            ReturnCommand = new RelayCommand(X => Return());
        }

        // Generate a list of the last 12 months as strings (e.g. "Jan", "Feb", etc.)
        private ObservableCollection<string> GenerateLast12Months()
        {
            var months = new ObservableCollection<string>();
            for (int i = 0; i < 12; i++)
            {
                months.Add(DateTime.Now.AddMonths(-i).ToString("MMM"));
            }
            return months;
        }

        // Generate random deposits data
        private double[] GenerateRandomDeposits()
        {
            Random random = new Random();
            var deposits = new double[12];
            for (int i = 0; i < 12; i++)
            {
                deposits[i] = random.Next(500, 5000);  // Random deposit data
            }
            return deposits;
        }

        private void BarCommand()
        {
            SelectedChartType = "Bar Chart";
        }

        private void LineCommand()
        {
            SelectedChartType = "Line Chart";
        }

        private void UpdateClientPerformance()
        {
            if (_deposits == null) return; // Ensure deposits have been generated

            if (SelectedChartType == "Bar Chart")
            {
                ClientPerformanceSeries = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Deposits",
                        Values = new ChartValues<double>(_deposits) // Use stored data
                    }
                };
            }
            else if (SelectedChartType == "Line Chart")
            {
                ClientPerformanceSeries = new SeriesCollection
                {
                    new LineSeries
                    {
                        Title = "Deposits",
                        Values = new ChartValues<double>(_deposits) // Use stored data
                    }
                };
            }

            OnPropertyChanged(nameof(ClientPerformanceSeries));
        }
        private void Return()
        {
            _mainViewModel.SelectedView = "Option2";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
