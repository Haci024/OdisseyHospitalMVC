using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.Models
{
    public class Reports
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public Patients Patient { get; set; }
        public Doctor Doctors { get; set; }

    }
}
