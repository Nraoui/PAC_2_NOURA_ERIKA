using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WPF_MVVM_SPA_Template.Models;
using WPF_MVVM_SPA_Template.Views;

namespace WPF_MVVM_SPA_Template.ViewModels
{
    class BankInfoFormsViewModel : INotifyPropertyChanged
    {
        private readonly MainViewModel _mainViewModel;
        private readonly Option2ViewModel _option2ViewModel;
        private readonly Option3ViewModel _option3ViewModel;


        public ObservableCollection<Client> AvailableClients { get; set; }

        
        private Client? _selectedClient;
        public Client? SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                _selectedClient = value;
                OnPropertyChanged();
                UpdateSelectedClientInfo();
            }
        }

        private BankClientInfo? _newInfo;
        public BankClientInfo? NewInfo
        {
            get { return _newInfo; }
            set { _newInfo = value; OnPropertyChanged(); }
        }

        private string? _ibanError;
        public string? IBANError
        {
            get { return _ibanError; }
            set
            {
                _ibanError = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IBANErrorVisibility));
                OnPropertyChanged(nameof(IBANErrorVisibility));
            }
        }

        private string? _incomeError;
        public string? IncomeError
        {
            get { return _incomeError; }
            set
            {
                _incomeError = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IncomeErrorVisibility));
            }
        }

        private string? _pinError;
        public string? PinError
        {
            get { return _pinError; }
            set
            {
                _pinError = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PinErrorVisibility));
            }
        }


        public Visibility IBANErrorVisibility => string.IsNullOrWhiteSpace(IBANError) ? Visibility.Collapsed : Visibility.Visible;
        public Visibility PinErrorVisibility => string.IsNullOrWhiteSpace(PinError) ? Visibility.Collapsed : Visibility.Visible;
        public Visibility IncomeErrorVisibility => string.IsNullOrWhiteSpace(IncomeError) ? Visibility.Collapsed : Visibility.Visible;
        //public Brush PinErrorForeground => PinError == 0 ? Brushes.Black : Brushes.Red;

        
        private bool ValidateInfo()
        {
            bool isValid = true;

            IBANError = string.IsNullOrWhiteSpace(NewInfo?.IBAN) ? "El IBAN és obligatori" : null;
            if (IBANError != null)
            {
                isValid = false;
            }else if (!IsValidIbanFormat(NewInfo.IBAN))
            {
                IBANError = "El format del IBAN no és correcte";
                isValid = false;
            }
            PinError = string.IsNullOrWhiteSpace(NewInfo?.Pin) ? "El PIN és obligatori" : null;
            if (PinError != null)
            {
                isValid = false;
            }else if (!IsValidPinFormat(NewInfo.Pin))
            {
                PinError = "El format de PIN no és correcte";
                isValid = false;
            }
            IncomeError = string.IsNullOrWhiteSpace(NewInfo?.SavedIncome) ? "Has d'afegir dades als diners!" : null;
            if (IncomeError != null)
            {
                isValid = false;
            }


            return isValid;
        }

        private bool IsValidIbanFormat(string format)
        {
            string pattern = @"^\d{24}$";
            return Regex.IsMatch(format, pattern);
        }
        private bool IsValidPinFormat(string format)
        {
            string pattern = @"^\d{4}$";
            return Regex.IsMatch(format, pattern);
        }



        public RelayCommand SaveBCInfo { get; set; }
        public RelayCommand CancelBCInfo { get; set; }

        public BankInfoFormsViewModel(MainViewModel mainViewModel, Option3ViewModel option3ViewModel, BankClientInfo? infoToEdit = null)
        {
            _mainViewModel = mainViewModel;
            _option2ViewModel = mainViewModel.Option2VM;
            _option3ViewModel = mainViewModel.Option3VM;

            AvailableClients = new ObservableCollection<Client>(_option2ViewModel.Clients);

            if (infoToEdit != null)
            {
                NewInfo = new BankClientInfo
                {
                    Id = infoToEdit.Id,
                    IBAN = infoToEdit.IBAN,
                    ClientName = infoToEdit.ClientName,
                    SavedIncome = infoToEdit.SavedIncome,
                    Debt = infoToEdit.Debt,
                    Pin = infoToEdit.Pin
                };
                SelectedClient = AvailableClients.FirstOrDefault(c => c.Id == NewInfo.Id);
            }
            else
            {
                //NewInfo = new BankClientInfo { Id = GetNextInfoId(), ClientName = _option2ViewModel.GetNomById(GetNextInfoId()) };
                NewInfo = new BankClientInfo();
            }


            SaveBCInfo = new RelayCommand(x => Save());
            CancelBCInfo = new RelayCommand(x => Cancel());
        }

        private void UpdateSelectedClientInfo()
        {
            if (SelectedClient != null)
            {
                NewInfo.Id = SelectedClient.Id;
                NewInfo.ClientName = SelectedClient.Name; // Assuming 'Name' is the client's name
                OnPropertyChanged(nameof(NewInfo.Id));
                OnPropertyChanged(nameof(NewInfo.ClientName));
            }
            else
            {
                NewInfo.Id = 0;
                NewInfo.ClientName = string.Empty;
                OnPropertyChanged(nameof(NewInfo.Id));
                OnPropertyChanged(nameof(NewInfo.ClientName));
            }
        }

        private void Save()
        {
            if (!ValidateInfo())
            {
                return;
            }

            if (NewInfo != null)
            {
                if (_option3ViewModel.BankClientInfo.Any(c => c.Id == NewInfo.Id))
                {
                    var existingInfo = _option3ViewModel.BankClientInfo.First(c => c.Id == NewInfo.Id);
                    existingInfo.IBAN = NewInfo.IBAN;
                    existingInfo.SavedIncome = NewInfo.SavedIncome;
                    existingInfo.Debt = NewInfo.Debt;
                    existingInfo.Pin = NewInfo.Pin;
                }
                else
                {
                    _option3ViewModel.BankClientInfo.Add(NewInfo);
                }
            }

            _mainViewModel.SelectedView = "Option3";
            ClearForm();
        }

        private void ClearForm()
        {
            NewInfo = new BankClientInfo();
            NewInfo.Id = _option3ViewModel.BankClientInfo.Count + 1;
        }

        private void Cancel()
        {
            // Show a MessageBox with Yes and No buttons
            MessageBoxResult result = MessageBox.Show(
                "Vols cancel·lar?",
                "Confirm Cancel",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                ClearForm(); 
                _mainViewModel.SelectedView = "Option3"; 
            }
            // If 'No' is clicked, do nothing and remain in the current view.
        }




        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}