using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.Models
{
    public class HospitalBlogsDetail
    {
       
        public int Id { get; set; }
        [Required(ErrorMessage ="Xəbər haqqında məlumat verməyi unutdunuz!")]
        public string Description { get; set; }
        public HospitalBlogs HospitalBlogs { get; set; }
       
        [ForeignKey("HospitalBogs")]
        public int HospitalBlogsId { get; set; }
    }
}
