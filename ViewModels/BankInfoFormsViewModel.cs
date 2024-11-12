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
using System.Windows.Input;
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

        private bool _isPinValid;
        public bool IsPinValid
        {
            get => _isPinValid;
            set
            {
                _isPinValid = value;
                OnPropertyChanged();

            }
        }

        private bool _isSavedIncomeValid;
        public bool IsSavedIncomeValid
        {
            get => _isSavedIncomeValid;
            set
            {
                _isSavedIncomeValid = value;
                OnPropertyChanged();

            }
        }

        private bool _isIbanValid;
        public bool IsIbanValid
        {
            get => _isIbanValid;
            set
            {
                _isIbanValid = value;
                OnPropertyChanged();

            }
        }

        private bool _isIncomeValid;
        public bool IsIncomeValid
        {
            get => _isIncomeValid;
            set
            {
                _isIncomeValid = value;
                OnPropertyChanged();

            }
        }







        private bool ValidateInfo()
        {



            return IsPinValid && IsSavedIncomeValid && IsIbanValid && IsIncomeValid;
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
                "Do you want to cancel?",
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