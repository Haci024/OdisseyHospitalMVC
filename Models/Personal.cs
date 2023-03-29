using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.Models
{
    public class Personal
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "İşçinin adını qeyd edin!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "İşçinin soyadını qeyd edin!")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "İşçinin vəzifəsini qeyd edin!")]
        public string Position { get; set; }
        [Required(ErrorMessage = "İşçinin yaşını qeyd edin!")]
        public int Age { get; set; }
        [Required(ErrorMessage = "İşçinin maaşını qeyd edin!")]
        public int Salary { get; set; }

        public Department Department { get; set; }
        
        public int DepartmentId { get; set; }
        
        public string  Image { get; set; }
       [NotMapped]
        public IFormFile Photo { get; set; }

        public bool IsDeactive { get; set; }
    }
}
