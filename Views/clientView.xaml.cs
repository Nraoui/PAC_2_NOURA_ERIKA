using System.Windows;
using System.Windows.Controls;
using WPF_MVVM_SPA_Template.ViewModels;

namespace WPF_MVVM_SPA_Template.Views
{
    // Vista UserControl enlloc de Window ja que no necessitem una finestra (SPA)
    public partial class Option2View : UserControl
    {
        private MainViewModel _mainViewModel;
        public Option2View()
        {
            InitializeComponent();
            _mainViewModel = (MainViewModel)Application.Current.MainWindow.DataContext;



        }
        
    }
}
