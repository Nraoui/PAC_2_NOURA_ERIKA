using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF_MVVM_SPA_Template.Models;

namespace WPF_MVVM_SPA_Template.ViewModels
{
    class ClientFormViewModel : INotifyPropertyChanged
    {
        private readonly MainViewModel _mainViewModel;
        private readonly Option2ViewModel _option2ViewModel;

        private Client? _newClient;
        public Client? NewClient
        {
            get { return _newClient; }
            set
            {
                _newClient = value;
                OnPropertyChanged();
            }
        }

        private bool _isSurnameValid;
        public bool IsSurnameValid
        {
            get { return _isSurnameValid; }
            set
            {
                _isSurnameValid = value;
                OnPropertyChanged();
            }
        }

        private bool _isNameValid;
        public bool IsNameValid
        {
            get { return _isNameValid; }
            set
            {
                _isNameValid = value;
                OnPropertyChanged();
            }
        }

        private bool _isPhoneNumberValid;
        public bool IsPhoneNumberValid
        {
            get { return _isPhoneNumberValid; }
            set
            {
                _isPhoneNumberValid = value;
                OnPropertyChanged();
            }
        }

        private bool _isDniValid;
        public bool IsDniValid
        {
            get => _isDniValid;
            set
            {
                _isDniValid = value;
                OnPropertyChanged();

            }
        }

        private bool _isEmailValid;
        public bool IsEmailValid
        {
            get => _isEmailValid;
            set
            {
                _isEmailValid = value;
                OnPropertyChanged();
            }
        }






        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public ClientFormViewModel(MainViewModel mainViewModel, Option2ViewModel option2ViewModel, Client? clientToEdit = null)
        {
            _mainViewModel = mainViewModel;
            _option2ViewModel = option2ViewModel;

            // Initialize NewClient or load the client to edit
            if (clientToEdit != null)
            {
                NewClient = new Client
                {
                    Id = clientToEdit.Id,
                    Dni = clientToEdit.Dni,
                    Name = clientToEdit.Name,
                    Surnames = clientToEdit.Surnames,
                    Email = clientToEdit.Email,
                    PhoneNumber = clientToEdit.PhoneNumber,
                    RegestrationDate = clientToEdit.RegestrationDate
                };
            }
            else
            {
                NewClient = new Client { Id = GetNextClientId() };
            }



            SaveCommand = new RelayCommand(x => Save());
            CancelCommand = new RelayCommand(x => Cancel());
        }

        // Save client logic
        private void Save()
        {


            if (!ValidateClient())
            {
                return;
            }

            if (NewClient != null)
            {
                if (_option2ViewModel.Clients.Any(c => c.Id == NewClient.Id))
                {
                    var existingClient = _option2ViewModel.Clients.First(c => c.Id == NewClient.Id);
                    existingClient.Email = NewClient.Email;
                    existingClient.PhoneNumber = NewClient.PhoneNumber;
                }
                else
                {
                    _option2ViewModel.Clients.Add(NewClient);
                }
            }

            _mainViewModel.SelectedView = "Option2";
            ClearForm();
        }


        private bool ValidateClient()
        {
            return IsNameValid && IsPhoneNumberValid && IsSurnameValid && IsDniValid && IsEmailValid;
        }

        private void ClearForm()
        {
            NewClient = new Client()
            {
                Id = GetNextClientId()
            };
        }

        private void Cancel()
        {
            MessageBoxResult result = MessageBox.Show(
                "Do you want to cancel?",
                "Confirm Cancel",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                ClearForm();
                _mainViewModel.SelectedView = "Option2";
            }
        }

        private int GetNextClientId()
        {
            return _option2ViewModel.Clients.Any() ?
                _option2ViewModel.Clients.Max(client => client.Id) + 1 : 1;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}