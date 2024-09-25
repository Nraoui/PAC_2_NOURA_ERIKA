using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WPF_MVVM_SPA_Template.Models;

namespace WPF_MVVM_SPA_Template.ViewModels
{
    //Els ViewModels deriven de INotifyPropertyChanged per poder fer Binding de propietats
    class Option1ViewModel : INotifyPropertyChanged
    {
        // Referència al ViewModel principal
        private readonly MainViewModel _mainViewModel;

        // Col·lecció de Students (podrien carregar-se d'una base de dades)
        // ObservableCollection és una llista que notifica els canvis a la vista
        public ObservableCollection<Student> Students { get; set; } = new ObservableCollection<Student>();

        // Propietat per controlar l'estudiant seleccionat a la vista
        private Student? _selectedStudent;
        public Student? SelectedStudent
        {
            get { return _selectedStudent; }
            set { _selectedStudent = value; OnPropertyChanged(); }
        }

        // RelayCommands connectats via Binding als botons de la vista
        public RelayCommand AddStudentCommand { get; set; }
        public RelayCommand DelStudentCommand { get; set; }

        public Option1ViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            // Carreguem estudiants a memòria mode de prova
            Students.Add(new Student { Id = 1, Name = "David" });
            Students.Add(new Student { Id = 2, Name = "Jordi" });

            // Inicialitzem els diferents commands disponibles (accions)
            AddStudentCommand = new RelayCommand(x => AddStudent());
            DelStudentCommand = new RelayCommand(x => DelStudent());
        }

        //Mètodes per afegir i eliminar estudiants de la col·lecció
        private void AddStudent()
        {
            Students.Add(new Student { Id = Students.Count + 1, Name = "Nou" });
        }

        private void DelStudent()
        {
            if (SelectedStudent != null)
                Students.Remove(SelectedStudent);
        }

        // Això és essencial per fer funcionar el Binding de propietats entre Vistes i ViewModels
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
