using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WPF_MVVM_SPA_Template.Models;
using WPF_MVVM_SPA_Template.Views;

namespace WPF_MVVM_SPA_Template.ViewModels
{
    //Els ViewModels deriven de INotifyPropertyChanged per poder fer Binding de propietats
    class Option2ViewModel : INotifyPropertyChanged
    {
        // Referència al ViewModel principal
        private readonly MainViewModel _mainViewModel;
        // Col·lecció de Courses (podrien carregar-se d'una base de dades)
        // ObservableCollection és una llista que notifica els canvis a la vista
        public ObservableCollection<Client> Clients { get; set; } = new ObservableCollection<Client>();
        

        // Propietat per controlar el curs seleccionat a la vista
        private Client? _selectedClient;
        public Client? SelectedClient
        {
            get { return _selectedClient; }
            set { _selectedClient = value; OnPropertyChanged(); }
        }

        // RelayCommands connectats via Binding als botons de la vista
        public RelayCommand AddClientCommand { get; set; }
        public RelayCommand DelClientCommand { get; set; }
        public RelayCommand EditClientCommand {  get; set; }





        public Option2ViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            // Carreguem cursos a memòria mode de prova
            Clients.Add(new Client { Id = 1,Dni="Y9456786L" ,Name = "Jordi",Surnames = "Soler ", Email = "jordi.soler@gmail.com", PhoneNumber = "612354687",RegestrationDate =new DateTime(2024,3,12) });
            Clients.Add(new Client { Id = 2, Dni = "X4578969W",Name = "Laia", Surnames = "Pujol ", Email = "laia.pujol@gmail.com", PhoneNumber = "613456895", RegestrationDate = new DateTime(2024, 4, 23) });
            Clients.Add(new Client { Id = 3, Dni = "L4589621O", Name = "Marc", Surnames = "Torres ", Email = "marc.torres@gmail.com", PhoneNumber = "613245697", RegestrationDate = new DateTime(2024, 5, 2) });
            Clients.Add(new Client { Id = 4, Dni = "N4589623M", Name = "Anna", Surnames = "Garcia ", Email = "anna.garcia@gmail.com", PhoneNumber = "612456321", RegestrationDate = new DateTime(2024, 6, 30) });
            Clients.Add(new Client { Id = 5, Dni = "M1235987P",Name = "Oriol", Surnames = "Marti ", Email = "oriol.marti@gmail.com", PhoneNumber = "613249562", RegestrationDate = new DateTime(2024, 7, 14) });
            Clients.Add(new Client { Id = 6, Dni = "R4598623U",Name = "Núria", Surnames = "López ", Email = "nuria.lopez@gmail.com", PhoneNumber = "613456217", RegestrationDate = new DateTime(2024, 8, 26) });

            // Inicialitzem els diferents commands disponibles (accions)
            AddClientCommand = new RelayCommand(x => AddClient());
            DelClientCommand = new RelayCommand(x => DelClient());
            EditClientCommand = new RelayCommand(x => EditClient());

            




        }
        

        //Mètodes per afegir i eliminar cursos de la col·lecció
        private void AddClient()
        {
            var clientFormViewModel = new ClientFormViewModel(_mainViewModel, this);
            _mainViewModel.CurrentView = new ClientFormView { DataContext = clientFormViewModel };

        }

        private void DelClient()
        {
            if (SelectedClient != null)
                Clients.Remove(SelectedClient);
        }
        private void EditClient()
        {
            if (SelectedClient != null)
            {
                
                var clientFormViewModel = new ClientFormViewModel(_mainViewModel, this);
                clientFormViewModel.NewClient = new Client
                {
                    Id = SelectedClient.Id,
                    Dni = SelectedClient.Dni,
                    Name = SelectedClient.Name,
                    Surnames = SelectedClient.Surnames,
                    Email = SelectedClient.Email,
                    PhoneNumber = SelectedClient.PhoneNumber,
                    RegestrationDate = SelectedClient.RegestrationDate
                };

                    _mainViewModel.CurrentView = new ClientFormView { DataContext = clientFormViewModel };

                }
            }

        
        

        // Això és essencial per fer funcionar el Binding de propietats entre Vistes i ViewModels
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
