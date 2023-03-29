using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.ViewModels
{
    public class ResetPasswordVM
    {
      
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        [DataType(DataType.Password),Compare("Password",ErrorMessage ="Şifrə ilə şifrənin təkrarı ilə eyni olmalıdır")]
        public string CheckPassword { get; set; }
    }
}
