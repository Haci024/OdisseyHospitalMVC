using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.ViewModels
{
    public class UserUpdateVM
    {
        public string Id { get; set; }

        [Required(ErrorMessage = " adınızı daxil edin!")]
        public string Name { get; set; }
        [Required(ErrorMessage = " Soyad daxil edin!")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Istifadəçi adınızı daxil edin!")]
        public string UserName { get; set; }
        [Required(ErrorMessage = " Mail hesabınızı daxil edin!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Şifrəni yeniləmədiniz!")]
        public string Password { get; set; }

    }
}
