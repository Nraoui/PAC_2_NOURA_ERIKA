using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

using WPF_MVVM_SPA_Template.Models;
using WPF_MVVM_SPA_Template.Views;
//JSON serialization package
using System.IO;
using System.Text.Json;

namespace WPF_MVVM_SPA_Template.ViewModels
{
    class Option3ViewModel : INotifyPropertyChanged
    {
        private readonly MainViewModel _mainViewModel;
        private readonly Option2ViewModel _option2ViewModel;
        //private readonly BankInfoFormsViewModel _bankInfoFormsViewModel;


        public ObservableCollection<BankClientInfo> BankClientInfo { get; set; } = new ObservableCollection<BankClientInfo>();



        private BankClientInfo? _selectedInfo;
        public BankClientInfo? SelectedInfo
        {
            get { return _selectedInfo; }
            set { _selectedInfo = value; OnPropertyChanged(); }
        }

        public RelayCommand AddBCInfoCommand { get; set; }
        public RelayCommand RemoveBCInfoCommand { get; set; }
        public RelayCommand EditBCInfoCommand { get; set; }
        //Serialize data
        public RelayCommand ExportToJsonCommand { get; set; } 
        public RelayCommand LoadFromJsonCommand { get; set; }



        public Option3ViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _option2ViewModel = mainViewModel.Option2VM;

            //BankClientInfo.Add(new BankClientInfo { Id = 1, IBAN = "DE44 1234 1234 1234 1234 00", SavedIncome = "20000", Debt = "1654", Pin = "2323", ClientName = _option2ViewModel.GetNomById(1) });
            //BankClientInfo.Add(new BankClientInfo { Id = 2, IBAN = "GB29 NWBK 6016 1331 9268 19", SavedIncome = "5000", Debt = "0", Pin = "1111", ClientName = _option2ViewModel.GetNomById(2) });
            LoadFromJson();

            AddBCInfoCommand = new RelayCommand(x => AddBCInfo());
            EditBCInfoCommand = new RelayCommand(x => EditBCInfo());
            RemoveBCInfoCommand = new RelayCommand(x => RemBCInfo());
            ExportToJsonCommand = new RelayCommand(x => SaveToJson());
            LoadFromJsonCommand = new RelayCommand(x => LoadFromJson());
        }

        private void SaveToJson()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var jsonData = JsonSerializer.Serialize(BankClientInfo, options);

            // Specify the file path
            var filePath = "BankClientInfo.json";
            File.WriteAllText(filePath, jsonData);

            MessageBox.Show("Data saved to JSON successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void LoadFromJson()
        {
            var filePath = "BankClientInfo.json";
            if (File.Exists(filePath))
            {
                var jsonData = File.ReadAllText(filePath);
                var bankClientInfoFromJson = JsonSerializer.Deserialize<ObservableCollection<BankClientInfo>>(jsonData);

                // Replace existing collection with the loaded one
                BankClientInfo.Clear();
                foreach (var item in bankClientInfoFromJson)
                {
                    BankClientInfo.Add(item);
                }

            }
            else
            {
                MessageBox.Show("No s'ha trobat cap arxiu JSON per la informació bancària!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddBCInfo()
        {
           
            var BankInfoFormViewModel = new BankInfoFormsViewModel(_mainViewModel, this);
            _mainViewModel.CurrentView = new InfoBankFormsView { DataContext = BankInfoFormViewModel };

        }

        private void EditBCInfo()
        {
            if(SelectedInfo != null)
            {
                var infoFormViewModel = new BankInfoFormsViewModel(_mainViewModel, this);
                infoFormViewModel.NewInfo = new BankClientInfo
                {
                    Id = SelectedInfo.Id,
                    IBAN = SelectedInfo.IBAN,
                    ClientName = SelectedInfo.ClientName,
                    SavedIncome = SelectedInfo.SavedIncome,
                    Debt = SelectedInfo.Debt,
                    Pin = SelectedInfo.Pin
                };

                _mainViewModel.CurrentView = new InfoBankFormsView { DataContext= infoFormViewModel };
            }
        }


        private void RemBCInfo()
        {
            if (SelectedInfo != null)
                BankClientInfo.Remove(SelectedInfo);
        }

        public void RemoveBankInfoByClientName(string clientName)
        {
            // Find and remove all bank client info associated with the client's name and email
            var bankClientInfosToRemove = BankClientInfo
                .Where(info => info.ClientName == clientName)
                .ToList();

            foreach (var info in bankClientInfosToRemove)
            {
                BankClientInfo.Remove(info);
            }
        }



        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}