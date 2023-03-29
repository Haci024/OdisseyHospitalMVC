using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.Models
{
    public class HospitalSlider
    {
      
        public int Id { get; set; }

        [Required(ErrorMessage ="Bura boş ola bilməz!")]
        public string Title { get; set; }
       
        [Required(ErrorMessage = "Başlıq məlumatı boş ola bilməz!")]
        public string Description { get; set; }
       
        public string Image { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        
       


    }
}
