using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.Models
{
    public class HospitalSliderImage
    {
        public int Id { get; set; }
       
        public string Image { get; set; }
       
        public bool IsDeactive { get; set; }
       [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
