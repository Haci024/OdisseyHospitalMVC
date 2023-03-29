using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.Models
{
    public class Department
    {
        public int Id { get; set; }

        public string Image { get; set; }
        [Required(ErrorMessage = "Departmanet  adını daxil etmədiniz")]
        public string DepartmentName { get; set; }
        [Required(ErrorMessage = "Departmanet haqqında məlumat daxil etmədiniz")]
        public string Description { get; set; }
        public List<Doctor> Doctor { get; set; }
        public bool IsDeactive { get; set; }
        [NotMapped]
        public  IFormFile Photo { get; set; }
       
        public List<Illnesses> Diseases { get; set; }
        public List<Rooms> Room { get; set; }
        public List<Personal> Personals { get; set; }
       




    }
}
