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
        public string Dni {  get; set; }
        public string Name { get; set; }
        public string Surnames { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RegestrationDate { get; set; }
        public Client()
        {
            RegestrationDate = DateTime.Today; // Set default value to today's date
        }

    }
}
