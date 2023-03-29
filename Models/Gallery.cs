using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.Models
{
    public class Gallery
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Qalareya başlığını daxil  etməyi unutdunuz!")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Qalareya haqqında məlumat verməyi unutdunuz!")]
        public string Description { get; set; }
       
       
       [NotMapped]
        public IFormFile[] Photos { get; set; }
        
        public bool IsDeactive { get; set; }
      
        public List<GalleryImages> GalleryImages { get; set; }
        
    }
}
