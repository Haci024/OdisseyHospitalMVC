using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.ViewModels
{
    public class ForgotPasswordVerify
    {
        [Required(ErrorMessage = "Bu xana boş ola bilməz")]
        public string Random { get; set; }
    }
}
