using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.Models
{
    public class BloodBank
    {
        public int Id { get; set; }
        [Required(ErrorMessage="Qan qrupunun adını daxil edin!")]
        public string BloodGroup { get; set; }
        [Required(ErrorMessage = "Qan qrupu haqqında məlumat daxil edin!")]
        public string About { get; set; }
       
        public string Image { get; set; }
       [NotMapped]
        public IFormFile Photo { get; set; }
        [Required(ErrorMessage = "Bankda olan qan miqdarını daxil edin!")]
        public float BloodQuantity { get; set; }
        public bool IsDeactive { get; set; }
    }
}
