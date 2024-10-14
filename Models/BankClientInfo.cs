using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPF_MVVM_SPA_Template.Models
{
    public class BankClientInfo
    {
        public int Id {  get; set; }

        public string ClientName { get; set; }

        public string IBAN {  get; set; }

        public string SavedIncome { get; set; }

        public string Debt {  get; set; }

        public string Pin {  get; set; }

    }
}
