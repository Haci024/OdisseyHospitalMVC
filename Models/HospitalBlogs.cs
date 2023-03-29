using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.Models
{
    public class HospitalBlogs
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Xəbər başlığını seçməyi unutdunuz!")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Xəbərin qısa başlıqını qeyd edin!")]
        public string Subtitle { get; set; }
        public string BlogImage { get; set; }
        public HospitalBlogsDetail HospitalBlogsDetail { get; set; }
        public DateTime CreateTime { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
       
        public bool IsDeactive { get; set; }

    }
}
