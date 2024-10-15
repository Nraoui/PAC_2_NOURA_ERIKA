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

        // Validation methods
        
        private bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        private bool IsValidDniFormat(string dni)
        {
            string dniPattern = @"^\d{8}[A-Za-z]$";
            return Regex.IsMatch(dni, dniPattern);
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            string phonePattern = @"^\d{9}$";
            return Regex.IsMatch(phoneNumber, phonePattern);
        }

        // ICommand properties
        public ICommand ValidateDniCommand { get; private set; }
        public ICommand ValidateNameCommand { get; private set; }
        public ICommand ValidateSurnamesCommand { get; private set; }
        public ICommand ValidateEmailCommand { get; private set; }
        public ICommand ValidatePhoneNumberCommand { get; private set; }

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

            // Initialize commands
            ValidateDniCommand = new RelayCommand(x => ValidateDni((string)x));
            ValidateNameCommand = new RelayCommand(x => ValidateName((string)x));
            ValidateSurnamesCommand = new RelayCommand(x => ValidateSurnames((string)x));
            ValidateEmailCommand = new RelayCommand(x => ValidateEmail((string)x));
            ValidatePhoneNumberCommand = new RelayCommand(x => ValidatePhoneNumber((string)x));

            SaveCommand = new RelayCommand(x => Save());
            CancelCommand = new RelayCommand(x => Cancel());
        }

        // Save client logic
        private void Save()
        {
            ValidateDni(NewClient?.Dni ?? string.Empty);
            ValidateName(NewClient?.Name ?? string.Empty);
            ValidateSurnames(NewClient?.Surnames ?? string.Empty);
            ValidateEmail(NewClient?.Email ?? string.Empty);
            ValidatePhoneNumber(NewClient?.PhoneNumber ?? string.Empty);

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

        private void ValidateDni(string dni)
        {
            DniError = string.IsNullOrWhiteSpace(dni) ? "El camp DNI és obligatori." : null;
            if (DniError == null && !IsValidDniFormat(dni))
            {
                DniError = "El format del DNI és incorrecte.";
            }
        }

        private void ValidateName(string name)
        {
            NameError = string.IsNullOrWhiteSpace(name) ? "El camp Name és obligatori." : null;
            if (NameError == null && name.Length < 3)
            {
                NameError = "El nom ha de tenir al menys 3 caràcters.";
            }
        }

        private void ValidateSurnames(string surnames)
        {
            SurnamesError = string.IsNullOrWhiteSpace(surnames) ? "El camp Surnames és obligatori." : null;
            if (SurnamesError == null && surnames.Length < 3)
            {
                SurnamesError = "Els cognoms han de tenir al menys 3 caràcters.";
            }
        }

        private void ValidateEmail(string email)
        {
            EmailError = string.IsNullOrWhiteSpace(email) ? "El camp Email és obligatori." : null;
            if (EmailError == null && !IsValidEmail(email))
            {
                EmailError = "El format de l'email és incorrecte.";
            }
        }

        private void ValidatePhoneNumber(string phoneNumber)
        {
            PhoneNumberError = string.IsNullOrWhiteSpace(phoneNumber) ? "El camp PhoneNumber és obligatori." : null;
            if (PhoneNumberError == null && !IsValidPhoneNumber(phoneNumber))
            {
                PhoneNumberError = "El número de telèfon ha de tenir exactament 9 dígits.";
            }
        }

        private bool ValidateClient()
        {
            return string.IsNullOrEmpty(DniError) && string.IsNullOrEmpty(NameError) &&
                   string.IsNullOrEmpty(SurnamesError) && string.IsNullOrEmpty(EmailError) &&
                   string.IsNullOrEmpty(PhoneNumberError);
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
                "Vols cancel·lar?",
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
