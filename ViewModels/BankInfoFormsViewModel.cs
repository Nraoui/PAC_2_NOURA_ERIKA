using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF_MVVM_SPA_Template.Models;
using WPF_MVVM_SPA_Template.Views;

namespace WPF_MVVM_SPA_Template.ViewModels
{
    class BankInfoFormsViewModel : INotifyPropertyChanged
    {
        private readonly MainViewModel _mainViewModel;
        private readonly Option2ViewModel _option2ViewModel;
        private readonly Option3ViewModel _option3ViewModel;

        private BankClientInfo? _newInfo;
        public BankClientInfo? NewInfo
        {
            get { return _newInfo; }
            set { _newInfo = value; OnPropertyChanged(); }
        }

        public RelayCommand SaveBCInfo { get; set; }
        public RelayCommand CancelBCInfo { get; set; }

        public BankInfoFormsViewModel(MainViewModel mainViewModel, Option3ViewModel option3ViewModel, BankClientInfo? infoToEdit = null)
        {
            _mainViewModel = mainViewModel;
            _option2ViewModel = mainViewModel.Option2VM;
            _option3ViewModel = mainViewModel.Option3VM;
            var currentId = _option3ViewModel.BankClientInfo.Count;

            if (infoToEdit != null)
            {
                NewInfo = new BankClientInfo
                {
                    Id = infoToEdit.Id,
                    IBAN = infoToEdit.IBAN,
                    ClientName = infoToEdit.ClientName,
                    SavedIncome = infoToEdit.SavedIncome,
                    Debt = infoToEdit.Debt,
                    Pin = infoToEdit.Pin
                };
            }
            else
            {
                currentId += 1;
                NewInfo = new BankClientInfo { Id = currentId, ClientName = _option2ViewModel.GetNomById(currentId) };
            }


            SaveBCInfo = new RelayCommand(x => Save());
            CancelBCInfo = new RelayCommand(x => Cancel());
        }

        private void Save()
        {
            if (NewInfo != null)
            {
                if (_option3ViewModel.BankClientInfo.Any(c => c.Id == NewInfo.Id))
                {
                    //MessageBox.Show(_option3ViewModel.SelectedInfo.Id.ToString());

                    var existingInfo = _option3ViewModel.BankClientInfo.First(c => c.Id == NewInfo.Id);
                    existingInfo.SavedIncome = NewInfo.SavedIncome;
                    existingInfo.Debt = NewInfo.Debt;

                }
                else
                {
                    _option3ViewModel.BankClientInfo.Add(NewInfo);
                }

            }

            _mainViewModel.SelectedView = "Option3";
            ClearForm();
        }

        private void ClearForm()
        {
            NewInfo = new BankClientInfo();
            NewInfo.Id = _option3ViewModel.BankClientInfo.Count + 1;
        }

        private void Cancel()
        {
            ClearForm();
            _mainViewModel.SelectedView = "Option3";
        }



        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}