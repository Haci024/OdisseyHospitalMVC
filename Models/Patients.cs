using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.Models
{
    public class Patients
    {
        public int Id { get; set; }
       
        [Required(ErrorMessage = "Pasientin adını daxil edin!")]
        public string Name { get; set; }
       
        [Required(ErrorMessage = "Pasientin soyadını daxil edin!")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Pasientin yaşını daxil etməyi   unutdunuz!")]
        public int Age { get; set; }
  
        public List<Reports> Reports { get; set; }
        [Required(ErrorMessage = "Pasientin qeydiyyat tarixini qeyd edin!")]
        public DateTime ApplyDate { get; set; }
     
        [Required(ErrorMessage = "Pasientin Gmail ünvanını  daxil edin!")]

        public string Email { get; set; }
        [Required(ErrorMessage = "Pasientin ev ünvanını  daxil edin!")]
        public string HomeAdress { get; set; }
        [Required(ErrorMessage = "Pasientin xəstəliyini   daxil edin!")]
        public string Disease { get; set; }

        public bool IsDeactive { get; set; }
       
        public string Image { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }

    }
}
