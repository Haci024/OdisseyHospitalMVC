using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.Models
{
    public class AboutUs
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Başlıq əlavə etməyi  unutdunuz!")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Mətn daxil etməyi unutdunuz!")]
        public string Description { get; set; }

        public string Image { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
