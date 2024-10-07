using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
        private string? _dniError;
        public string? DniError
        {
            get { return _dniError; }
            set
            {
                _dniError = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DniErrorVisibility));
            }
        }

        private string? _nameError;
        public string? NameError
        {
            get { return _nameError; }
            set
            {
                _nameError = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(NameErrorVisibility));
            }
        }

        private string? _surnamesError;
        public string? SurnamesError
        {
            get { return _surnamesError; }
            set
            {
                _surnamesError = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SurnamesErrorVisibility));
            }
        }

        private string? _emailError;
        public string? EmailError
        {
            get { return _emailError; }
            set
            {
                _emailError = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(EmailErrorVisibility));
            }
        }

        private string? _phoneNumberError;
        public string? PhoneNumberError
        {
            get { return _phoneNumberError; }
            set
            {
                _phoneNumberError = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PhoneNumberErrorVisibility));
            }
        }


        public Visibility DniErrorVisibility => string.IsNullOrWhiteSpace(DniError) ? Visibility.Collapsed : Visibility.Visible;
        public Visibility NameErrorVisibility => string.IsNullOrWhiteSpace(NameError) ? Visibility.Collapsed : Visibility.Visible;
        public Visibility SurnamesErrorVisibility => string.IsNullOrWhiteSpace(SurnamesError) ? Visibility.Collapsed : Visibility.Visible;
        public Visibility EmailErrorVisibility => string.IsNullOrWhiteSpace(EmailError) ? Visibility.Collapsed : Visibility.Visible;
        public Visibility PhoneNumberErrorVisibility => string.IsNullOrWhiteSpace(PhoneNumberError) ? Visibility.Collapsed : Visibility.Visible;
        private bool ValidateClient()
        {
            bool isValid = true;

            DniError = string.IsNullOrWhiteSpace(NewClient?.Dni) ? "El camp DNI és obligatori." : null;
            if (DniError != null) isValid = false;

            NameError = string.IsNullOrWhiteSpace(NewClient?.Name) ? "El camp Name és obligatori." : null;
            if (NameError == null && NewClient.Name.Length < 3)
            {
                NameError = "El nom ha de tenir al menys 3 caràcters.";
                isValid = false;
            }

            SurnamesError = string.IsNullOrWhiteSpace(NewClient?.Surnames) ? "El camp Surnames és obligatori." : null;
            if (SurnamesError != null) isValid = false;

            EmailError = string.IsNullOrWhiteSpace(NewClient?.Email) ? "El camp Email és obligatori." : null;
            if (EmailError != null) isValid = false;

            PhoneNumberError = string.IsNullOrWhiteSpace(NewClient?.PhoneNumber) ? "El camp PhoneNumber és obligatori." : null;
            if (PhoneNumberError != null) isValid = false;

            return isValid;
        }


        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        

        public ClientFormViewModel(MainViewModel mainViewModel, Option2ViewModel option2ViewModel, Client? clientToEdit = null)
        {
            _mainViewModel = mainViewModel;
            _option2ViewModel = option2ViewModel;
            




            if (clientToEdit != null)
            {
                // If editing an existing client, populate NewClient
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
                // If adding a new client
                NewClient = new Client { Id = _option2ViewModel.Clients.Count + 1 };
            }



            SaveCommand = new RelayCommand(x => Save());
            CancelCommand = new RelayCommand(x => Cancel());



        }

        

        private void Save()
        {
            ValidateClient();

            if (!ValidateClient()) 
            {
                
                return; 
            }
            if (NewClient != null)
            {
                
                if (_option2ViewModel.Clients.Any(c => c.Id == NewClient.Id))
                {
                    // Update the existing client
                    var existingClient = _option2ViewModel.Clients.First(c => c.Id == NewClient.Id);
                    existingClient.Email = NewClient.Email;
                    existingClient.PhoneNumber = NewClient.PhoneNumber;
                    // Update other properties if needed, or leave them unchanged
                }
                else
                {
                    // Add new client logic, if needed
                    _option2ViewModel.Clients.Add(NewClient);
                }

            }
            _mainViewModel.SelectedView = "Option2";
            ClearForm();
        }
        private void ClearForm()
        {
            NewClient = new Client();
            NewClient.Id = _option2ViewModel.Clients.Count + 1;

        }
        private void Cancel()
        {
            ClearForm();
            _mainViewModel.SelectedView = "Option2";

        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


    }
    

    
}

