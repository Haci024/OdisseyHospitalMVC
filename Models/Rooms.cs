using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.Models
{
    public class Rooms
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Otaq nömrəsi seçmədiniz")]
        public int RoomNum { get; set; }
        [Required(ErrorMessage = "Otaq tipini seçmədiniz")]
        public string Roomtype { get; set; }
        [Required(ErrorMessage = "Zəhmət olmasa otağın tutumunu qeyd edin!")]
        public int RoomCapacity { get; set; }

        public bool IsDeactive { get; set; }
        public Department Department { get; set; }
        public int DepartmentId { get; set; }

    }
}
