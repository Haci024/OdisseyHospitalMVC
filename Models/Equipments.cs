using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.Models
{
  
    public class Equipments
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Alətin adını daxil etməyi unutmayın!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Alətin qiymətini daxil etməyi unutmayın!")]
        public float Price { get; set; }
        [Required(ErrorMessage = "Alət haqqında məlumatı  daxil etməyi unutmayın!")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Alətin sayını daxil etməyi unutmayın!")]
        public int EquipmentCount { get; set; }
        public string Images { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
       
        public bool IsDeactive { get; set; }
    }
}
