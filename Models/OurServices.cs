using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.Models
{
    public class OurServices
    {
        public int Id { get; set; }
        [Required(ErrorMessage="Zəhmət olmasa servisin adını qeyd edin!")]
        public string ServiceName { get; set; }
        [Required(ErrorMessage = "Zəhmət olmasa servis haqqında məlumat verin")]
        public string Description { get; set; }
        public string Image { get; set; }
       
       
        public bool Isdeactive  { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }


    }
}
