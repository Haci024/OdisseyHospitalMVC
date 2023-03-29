using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.Models
{
    public class AppUser:IdentityUser
    {
        public string Name { get; set; }
      
        public string Surname { get; set; }
       
        public bool IsDeactive { get; set; }
    }
}
