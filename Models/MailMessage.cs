using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HospitalMVC.Models
{
    public class MailMessage
    {
        [Required(ErrorMessage = "Mesajın kimə  olduğunu qeyd etmədiniz!")]
        public  string To{ get; set; }
        [Required(ErrorMessage ="Mesaj başlığını daxil edin!")]
        public string Subject{ get; set; }
        [Required(ErrorMessage = "Mesaj mətnini daxil edin!")]
        public string Body { get; set; }
    }
}
