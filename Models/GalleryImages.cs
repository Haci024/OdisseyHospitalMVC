using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.Models
{
    public class GalleryImages
    {
        public int Id { get; set; }
       
        public Gallery Gallery { get; set; }
       
        public int GalleryId { get; set; }
        [Required(ErrorMessage = "Şəkil daxil etməyi unutmayın!")]
        public string  Image { get; set; }
    }
}
