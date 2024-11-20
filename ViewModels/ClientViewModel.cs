using FastReport;
using FastReport.Data;
using FastReport.Export.PdfSimple;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
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
        public RelayCommand EditClientCommand { get; set; }
        public RelayCommand ExportToJsonCommand { get; set; }
        public RelayCommand LoadFromJsonCommand { get; set; }
        public RelayCommand ViewPerformanceCommand { get; set; }
        public RelayCommand ExportToPdfCommand { get; set; }





        public Option2ViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            // Carreguem cursos a memòria mode de prova

            Clients.Add(new Client { Id = 1, Dni = "Y9456786L", Name = "Jordi", Surnames = "Soler ", Email = "jordi.soler@gmail.com", PhoneNumber = "612354687", RegestrationDate = new DateTime(2024, 3, 12) });
            Clients.Add(new Client { Id = 2, Dni = "X4578969W", Name = "Laia", Surnames = "Pujol ", Email = "laia.pujol@gmail.com", PhoneNumber = "613456895", RegestrationDate = new DateTime(2024, 4, 23) });
            Clients.Add(new Client { Id = 3, Dni = "L4589621O", Name = "Marc", Surnames = "Torres ", Email = "marc.torres@gmail.com", PhoneNumber = "613245697", RegestrationDate = new DateTime(2024, 5, 2) });
            Clients.Add(new Client { Id = 4, Dni = "N4589623M", Name = "Anna", Surnames = "Garcia ", Email = "anna.garcia@gmail.com", PhoneNumber = "612456321", RegestrationDate = new DateTime(2024, 6, 30) });
            Clients.Add(new Client { Id = 5, Dni = "M1235987P", Name = "Oriol", Surnames = "Marti ", Email = "oriol.marti@gmail.com", PhoneNumber = "613249562", RegestrationDate = new DateTime(2024, 7, 14) });
            Clients.Add(new Client { Id = 6, Dni = "R4598623U", Name = "Núria", Surnames = "López ", Email = "nuria.lopez@gmail.com", PhoneNumber = "613456217", RegestrationDate = new DateTime(2024, 8, 26) });
            LoadFromJson();

            






            // Inicialitzem els diferents commands disponibles (accions)
            AddClientCommand = new RelayCommand(x => AddClient());
            DelClientCommand = new RelayCommand(x => DelClient());
            EditClientCommand = new RelayCommand(x => EditClient());
            ExportToJsonCommand = new RelayCommand(x => SaveToJson());
            LoadFromJsonCommand = new RelayCommand(x => LoadFromJson());
            ViewPerformanceCommand = new RelayCommand(x => ViewPerformance());
            ExportToPdfCommand = new RelayCommand(x => ExportToPdf());


        }

        private void ExportToPdf()
        {
            try
            {
                // Load the FastReport template
                Report report = new Report();
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Data\ClientsReport.frx");
                filePath = Path.GetFullPath(filePath);
                report.Load(filePath);

                // Register the client data as an in-memory data source
                report.RegisterData(Clients, "Data");

                // Enable the data source in the report
                DataSourceBase dataSource = report.GetDataSource("Data");
                if (dataSource != null)
                {
                    dataSource.Enabled = true;
                }
                else
                {
                    throw new Exception("Failed to find data source 'Clients' in the report.");
                }
                // Vinculem el DataSource a la banda de dades
                DataBand? dataBand = report.FindObject("Data1") as DataBand;
                if (dataBand != null)
                {
                    dataBand.DataSource = dataSource;
                }


                // Prepare the report
                report.Prepare();

                // Export the report to a PDF
                string pdfFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Data\ClientsReport.pdf");
                pdfFilePath = Path.GetFullPath(pdfFilePath);

                using (MemoryStream ms = new MemoryStream())
                {
                    PDFSimpleExport pdfExport = new PDFSimpleExport();
                    report.Export(pdfExport, ms);
                    File.WriteAllBytes(pdfFilePath, ms.ToArray());
                }

                // Dispose of the report to free resources
                report.Dispose();

                // Notify the user of success
                MessageBox.Show($"Report successfully exported to PDF: {pdfFilePath}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Open the PDF using the system's default viewer
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = pdfFilePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to export to PDF: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
        }


        private void ViewPerformance()
        {
            if (SelectedClient != null)
            {
                var rendimentViewModel = new RendimentViewModel(SelectedClient, _mainViewModel);
                _mainViewModel.CurrentView = new RendimentView { DataContext = rendimentViewModel };
            }
        }

        private void AddClient()
        {
            var clientFormViewModel = new ClientFormViewModel(_mainViewModel, this);
            _mainViewModel.CurrentView = new ClientFormView { DataContext = clientFormViewModel };

        }

        private void DelClient()
        {
            if (SelectedClient != null)
            {
                _mainViewModel.Option3VM.RemoveBankInfoByClientName(SelectedClient.Name);
                Clients.Remove(SelectedClient);
            }
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

        private void SaveToJson()
        {
            try
            {
                // Serialize the data with indentation for readability
                var options = new JsonSerializerOptions { WriteIndented = true };
                var jsonData = JsonSerializer.Serialize(Clients, options);

                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Data\ClientsData.json");
                filePath = Path.GetFullPath(filePath);
                
                // Write JSON data to the file
                File.WriteAllText(filePath, jsonData);

                MessageBox.Show("Data saved to JSON successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and notify the user
                MessageBox.Show($"Failed to save data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadFromJson()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Data\ClientsData.json");
            filePath = Path.GetFullPath(filePath);

            if (File.Exists(filePath))
            {
                var jsonData = File.ReadAllText(filePath);
                var ClientDataFromJson = JsonSerializer.Deserialize<ObservableCollection<Client>>(jsonData);

                // Replace existing collection with the loaded one
                Clients.Clear();
                foreach (var item in ClientDataFromJson)
                {
                    Clients.Add(item);
                }

            }
            else
            {
                MessageBox.Show("No JSON file for Clients!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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