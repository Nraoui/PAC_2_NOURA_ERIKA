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
        private bool ValidateClient()
        {
            bool isValid = true;

            DniError = string.IsNullOrWhiteSpace(NewClient?.Dni) ? "El camp DNI és obligatori." : null;
            if (DniError != null)
            {
                isValid = false;
            }
            else if (!IsValidDniFormat(NewClient.Dni))
            {
                DniError = "El format del DNI és incorrecte.";
                isValid = false;
            }

            NameError = string.IsNullOrWhiteSpace(NewClient?.Name) ? "El camp Name és obligatori." : null;
            if (NameError != null)
            {
                isValid = false;
            }
            else if (NewClient.Name.Length < 3)
            {
                NameError = "El nom ha de tenir al menys 3 caràcters.";
                isValid = false;
            }

            SurnamesError = string.IsNullOrWhiteSpace(NewClient?.Surnames) ? "El camp Surnames és obligatori." : null;
            if (SurnamesError != null)
            {
                isValid = false;
            }
            else if (NewClient.Surnames.Length < 3)
            {
                SurnamesError = "Els cognoms han de tenir al menys 3 caràcters.";
                isValid = false;
            }
            EmailError = string.IsNullOrWhiteSpace(NewClient?.Email) ? "El camp Email és obligatori." : null;
            if (EmailError != null)
            {
                isValid = false;
            }
            else if (!IsValidEmail(NewClient.Email))
            {
                EmailError = "El format de l'email és incorrecte.";
                isValid = false;
            }

            PhoneNumberError = string.IsNullOrWhiteSpace(NewClient?.PhoneNumber) ? "El camp PhoneNumber és obligatori." : null;
            if (PhoneNumberError != null)
            {
                isValid = false;
            }
            else if (!IsValidPhoneNumber(NewClient.PhoneNumber))  // Nova validació per 9 dígits
            {
                PhoneNumberError = "El número de telèfon ha de tenir exactament 9 dígits.";
                isValid = false;
            }
            return isValid;
        }
        private bool IsValidEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
        private bool IsValidDniFormat(string dni)
        {
            // Expressió regular per validar el format: 8 dígits seguits d'una lletra
            string dniPattern = @"^\d{8}[A-Za-z]$";
            return Regex.IsMatch(dni, dniPattern);
        }
        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // Expressió regular per assegurar que el número té exactament 9 dígits
            string phonePattern = @"^\d{9}$";
            return Regex.IsMatch(phoneNumber, phonePattern);
        }


        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        private int GetNextClientId()
        {
            // Return the next available ID based on the count of existing clients
            return _option2ViewModel.Clients.Any() ?
                _option2ViewModel.Clients.Max(client => client.Id) + 1 : 1;
        }


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
                NewClient = new Client { Id = GetNextClientId() };
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
            NewClient = new Client()
            {
                Id = GetNextClientId() // Get a new unique ID for the new client
            };

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