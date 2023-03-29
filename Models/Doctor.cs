using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.Models
{
    public class Doctor
    {
        public int Id { get; set;}
        [Required(ErrorMessage = "Həkimin adını qeyd etməyi unutdunuz!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Həkimin soyadını qeyd etməyi unutdunuz!")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Həkim haqqında məlumat verməyi unutdunuz!")]  
        public string Description { get; set; }
        [Required(ErrorMessage = "Həkim yaşını daxil etməyi   unutdunuz!")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Həkimin ev adresini  daxil etməyi unutdunuz!")]
        public string HomeAdress { get; set; }
        [Required(ErrorMessage = "Həkimin cinisini qeyd etməyi  unutdunuz!")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Həkimin işini qeyd etməyi unutdunuz!")]
        public string Work { get; set; }

        public Department Department { get; set; }
        [Required(ErrorMessage = "Həkimin təhsili barəsində  məlumat verməyi unutdunuz!")]
        public string Education { get; set; }
       
        public string DoctorImage { get; set; }
       [NotMapped]
        public IFormFile DoctorPhoto { get; set; }
        [Required(ErrorMessage = "Həkimin qan qrupunu daxil etməyi unutdunuz!")]
        public int BloodGroup { get; set; }
        [Required(ErrorMessage = "Həkimin telefon nömrəsini daxil etməyi unutdunuz!")]
        public double WorkNumber { get; set; }
        [Required(ErrorMessage = "Həkimin elektron poçtunu daxil etməyi unutdunuz!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Həkimin maaşını daxil etməyi unutdunuz!")]
        public int  Salary { get; set; }

        public List<Reports> Reports { get; set; }

        public bool IsDeactive { get; set; }
       
        public int DepartmentId { get; set; }
        
        


    }
}
