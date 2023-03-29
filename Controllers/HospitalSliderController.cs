using HospitalMVC.DAL;
using HospitalMVC.Helpers;
using HospitalMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.Controllers
{
    //[Authorize(Roles = "Admin")]

    public class HospitalSliderController : Controller
    {
        #region Database/and/Photo
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public HospitalSliderController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        #endregion
        #region Index
        public async Task<IActionResult> Index()
        {

            List<HospitalSlider> dbsliders = await _db.Sliders.ToListAsync();

            return View(dbsliders);

        }
        #endregion
        #region Update
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return View("Error");
            };
            HospitalSlider hospitalSlider = await _db.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            if (hospitalSlider == null)
            {
                return View("Error");
            }
            return View(hospitalSlider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(HospitalSlider changehospitalSlider, int? id)
        {
            if (id == null)
            {
                return View("Error");
            };
            HospitalSlider dbhospitalSlider = await _db.Sliders.FirstOrDefaultAsync(x => x.Id == id);
           
            if (dbhospitalSlider == null)
            {
                return View("Error");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (changehospitalSlider.Photo != null)
            {

                if (!changehospitalSlider.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please choose the ImageFile!");

                    return View();
                }

                if (changehospitalSlider.Photo.IsMore5MB())
                {
                    ModelState.AddModelError("Photo", "Image Max 5MB!");

                    return View();

                }
                string path = Path.Combine(_env.WebRootPath, "assets/Image");
                dbhospitalSlider.Image = await changehospitalSlider.Photo.SaveImageAsync(path);

            }

            dbhospitalSlider.Title = changehospitalSlider.Title;
            dbhospitalSlider.Description = changehospitalSlider.Description;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        #endregion
        #region Detail
        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            HospitalSlider hospitalSlider = _db.Sliders.FirstOrDefault(x => x.Id == id);
            if (hospitalSlider == null)
            {
                return View();
            }
            return View(hospitalSlider);
        }

        #endregion


    }
}


