using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WPF_MVVM_SPA_Template.Views;

namespace WPF_MVVM_SPA_Template.ViewModels
{
    //Els ViewModels deriven de INotifyPropertyChanged per poder fer Binding de propietats
    class MainViewModel : INotifyPropertyChanged
    {

        // ViewModels de les diferents opcions
        public Option1ViewModel Option1VM { get; set; }
        public Option2ViewModel Option2VM { get; set; }
        
        public ClientFormViewModel ClientFormVM { get; set; }
        public Option3ViewModel Option3VM { get; set; }
        public BankInfoFormsViewModel BankInfoFormsVM { get; set; }




        // Propietat que conté la vista actual (és un objecte)
        private object? _currentView;
        public object? CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        // Propietat per controlar la vista seleccionada al ListBox (És un string)
        private string? _selectedView;
        public string? SelectedView
        {
            get { return _selectedView; }
            set
            {
                _selectedView = value;
                OnPropertyChanged();
                ChangeView();
            }
        }

        public RelayCommand ChangeThemeCommand { get; set; }

        public MainViewModel()
        {
            // Inicialitzem els diferents ViewModels
            Option1VM = new Option1ViewModel(this);
            Option2VM = new Option2ViewModel(this);
            Option3VM = new Option3ViewModel(this);
            // Mostra la vista principal inicialment
            SelectedView = "Option1";
            ChangeView();

            ChangeThemeCommand = new RelayCommand(x => ChangeTheme());
        }

        // Canvi de Vista
        private void ChangeView()
        {
            switch (SelectedView)
            {
                case "Option1": CurrentView = new Option1View { DataContext = Option1VM }; break;
                case "Option2": CurrentView = new Option2View { DataContext = Option2VM }; break;
                case "Option3": CurrentView = new Option3View { DataContext = Option3VM }; break;
            }
        }


        private void ChangeTheme()
        {
            var currentTheme = Application.Current.Resources.MergedDictionaries[0].Source.ToString();
            string? altTheme;
            //MessageBox.Show($"Current Theme: {currentTheme}");
            if (currentTheme.Contains("DarkTheme.xaml"))
            {
                altTheme = "ModernTheme.xaml";
            }
            else
            {
                altTheme = "DarkTheme.xaml";
            }
            //MessageBox.Show($"Switching to Theme: {altTheme}");
            ResourceDictionary resourceDictionary = new ResourceDictionary
            {
                Source = new Uri($"pack://application:,,,/Views/Themes/{altTheme}", UriKind.Absolute)
            };
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);

            
        }

        // Això és essencial per fer funcionar el Binding de propietats entre Vistes i ViewModels
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
