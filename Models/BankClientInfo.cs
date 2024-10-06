using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MVVM_SPA_Template.Models
{
    class BankClientInfo
    {
        public int Id { get; set; }
        public string IBAN { get; set; }
        public int SavedIncome { get; set; }
        public int Debt { get; set; }
        public int Pin { get; set; }
        public string ClientName { get; set; }
    }
}
