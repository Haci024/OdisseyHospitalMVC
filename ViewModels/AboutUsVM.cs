using HospitalMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.ViewModels
{
    public class AboutUsVM
    {
        public Bios Bios { get; set; }
        public List<AboutUs> AboutUs { get; set; }
    }
}
