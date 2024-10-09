using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
            }
        }

        private int? _pinError;
        public int? PinError
        {
            get { return _pinError; }
            set
            {
                _pinError = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PinErrorForeground));
            }
        }


        public Visibility IBANErrorVisibility => string.IsNullOrWhiteSpace(IBANError) ? Visibility.Collapsed : Visibility.Visible;
        public Brush PinErrorForeground => PinError == 0 ? Brushes.Black : Brushes.Red;


        private bool ValidateInfo()
        {
            bool isValid = true;

            IBANError = string.IsNullOrWhiteSpace(NewInfo?.IBAN) ? "El IBAN és obligatori" : null;
            if (IBANError != null) isValid = false;

            PinError = NewInfo?.Pin == 0 ? 1 : 0;
            if (PinError != 0) isValid = false;


            return isValid;
        }


        public RelayCommand SaveBCInfo { get; set; }
        public RelayCommand CancelBCInfo { get; set; }

        public BankInfoFormsViewModel(MainViewModel mainViewModel, Option3ViewModel option3ViewModel, BankClientInfo? infoToEdit = null)
        {
            _mainViewModel = mainViewModel;
            _option2ViewModel = mainViewModel.Option2VM;
            _option3ViewModel = mainViewModel.Option3VM;
            var currentId = _option3ViewModel.BankClientInfo.Count;

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
            }
            else
            {
                currentId += 1;
                NewInfo = new BankClientInfo { Id = currentId, ClientName = _option2ViewModel.GetNomById(currentId) };
            }


            SaveBCInfo = new RelayCommand(x => Save());
            CancelBCInfo = new RelayCommand(x => Cancel());
        }

        private void Save()
        {

            ValidateInfo();

            if (!ValidateInfo())
            {
                return;
            }

            if (NewInfo != null)
            {
                if (_option3ViewModel.BankClientInfo.Any(c => c.Id == NewInfo.Id))
                {
                    //MessageBox.Show(_option3ViewModel.SelectedInfo.Id.ToString());

                    var existingInfo = _option3ViewModel.BankClientInfo.First(c => c.Id == NewInfo.Id);
                    existingInfo.SavedIncome = NewInfo.SavedIncome;
                    existingInfo.Debt = NewInfo.Debt;

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
            ClearForm();
            _mainViewModel.SelectedView = "Option3";
        }



        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}