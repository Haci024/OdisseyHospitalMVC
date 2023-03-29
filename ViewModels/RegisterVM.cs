using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Adınızı daxil edin!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Soyadınızı  daxil edin!")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Istifadəçi adınızı daxil edin!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Istifadəçi mailinizi daxil edin!"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifrənizi daxil edin!"), DataType(DataType.Password)]
        public string Password { get; set; }


        [Required(ErrorMessage = "Şifrə təkrarını  daxil edin!"), DataType(DataType.Password), Compare(nameof(Password))]//Sifrenin tekrarlanmasi.Tekrar daxil edilen sifre evvelki ile eyni olmalidir.
        public string CheckPassword { get; set; }
    }
}
