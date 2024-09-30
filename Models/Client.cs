using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MVVM_SPA_Template.Models
{
    class Client
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Cognoms { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public DateTime DataAlta { get; set; }

    }
}
