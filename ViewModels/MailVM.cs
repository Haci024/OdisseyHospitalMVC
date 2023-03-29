using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.ViewModels
{
    public class MailVM
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
