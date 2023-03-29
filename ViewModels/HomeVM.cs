using HospitalMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.ViewModels
{
    public class HomeVM
    {
        public  List<Doctor>  Doctor { get; set; }
        public List<Patients> Patients { get; set; }
        public  List<OurServices> OurServices { get; set; }
        public List<Department> Departments { get; set; }
        public List<BloodBank> BloodBanks { get; set; }
    }
}
