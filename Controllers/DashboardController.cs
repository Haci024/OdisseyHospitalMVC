using HospitalMVC.DAL;
using HospitalMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.Controllers
{

    //[Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _db;
     
        public DashboardController(AppDbContext db)
        {
            _db = db;
         
        }
     
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM

            {
               
               
                OurServices = _db.OurServices.ToList(),
                Departments =_db.Department.Include(x=>x.Doctor).ToList(),
                BloodBanks = _db.BloodBanks.ToList(),
            };
            return View(homeVM);
        }


        public IActionResult Error()
        {
            return View("Error");
        }
    }
}
