using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Istifadəçi adınızı daxil edin!"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage ="Istifadəçi şifrənizi daxil edin!"), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
