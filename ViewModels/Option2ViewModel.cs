using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WPF_MVVM_SPA_Template.Models;

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
        

        public Option2ViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            // Carreguem cursos a memòria mode de prova
            Clients.Add(new Client { Id = 1, Nom = "Jordi",Cognoms = "Soler ", Email = "jordi.soler@gmail.com", Telefon = "612354687",DataAlta =new DateTime(2024,3,12) });
            Clients.Add(new Client { Id = 2, Nom = "Laia", Cognoms = "Pujol ", Email = "laia.pujol@gmail.com", Telefon = "613456895", DataAlta = new DateTime(2024, 4, 23) });
            Clients.Add(new Client { Id = 3, Nom = "Marc", Cognoms = "Torres ", Email = "marc.torres@gmail.com", Telefon = "613245697", DataAlta = new DateTime(2024, 5, 2) });
            Clients.Add(new Client { Id = 4, Nom = "Anna", Cognoms = "Garcia ", Email = "anna.garcia@gmail.com", Telefon = "612456321", DataAlta = new DateTime(2024, 6, 30) });
            Clients.Add(new Client { Id = 5, Nom = "Oriol", Cognoms = "Marti ", Email = "oriol.marti@gmail.com", Telefon = "613249562", DataAlta = new DateTime(2024, 7, 14) });
            Clients.Add(new Client { Id = 6, Nom = "Núria", Cognoms = "López ", Email = "nuria.lopez@gmail.com", Telefon = "613456217", DataAlta = new DateTime(2024, 8, 26) });

            // Inicialitzem els diferents commands disponibles (accions)
            AddClientCommand = new RelayCommand(x => AddClient());
            DelClientCommand = new RelayCommand(x => DelClient());
            

        }

        //Mètodes per afegir i eliminar cursos de la col·lecció
        private void AddClient()
        {
            Clients.Add(new Client { Id = Clients.Count + 1 });
        }

        private void DelClient()
        {
            if (SelectedClient != null)
                Clients.Remove(SelectedClient);
        }
        

        // Això és essencial per fer funcionar el Binding de propietats entre Vistes i ViewModels
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
