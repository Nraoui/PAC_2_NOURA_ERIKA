using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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

