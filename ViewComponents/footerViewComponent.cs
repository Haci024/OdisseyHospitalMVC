using HospitalMVC.DAL;
using HospitalMVC.Models;
using HospitalMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly AppDbContext _db;

        public FooterViewComponent(AppDbContext db)
        {
            _db = db;

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            Bios bio = await _db.Bios.FirstOrDefaultAsync();

         
            return View(bio);
        }
    }
}
