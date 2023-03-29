using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.Models
{
    public class Illnesses
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Xəstəliy haqqında məlumat verməyi unutdunuz!")]
        public string Ilness { get; set; }
        [Required(ErrorMessage = "Xəstəliyin müalicə metodunu daxil etməyi unutdunuz")]
        public string HealthyMethod { get; set; }
        public Department Department { get; set; }
      
        public int DepartmentId { get; set; }
       


    }
}
