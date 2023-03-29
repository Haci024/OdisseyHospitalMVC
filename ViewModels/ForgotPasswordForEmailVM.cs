using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.ViewModels
{
    public class ForgotPasswordForEmailVM
    {
        [Required(ErrorMessage = "Bu xana boş ola bilməz"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
