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
    public class AboutUsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public AboutUsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public IActionResult Index()
        {

            List<AboutUs> aboutUs = _db.AboutUs.ToList();

            return View(aboutUs);
        }

        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            AboutUs dbaboutUs = _db.AboutUs.FirstOrDefault(x => x.Id == id);
            if (dbaboutUs == null)
            {
                return View("Error");

            }
            return View(dbaboutUs);
        }
        #region Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, AboutUs changeAboutUs)
        {
            if (id == null)
            {
                return View("Error");
            }
            AboutUs dbAboutUs = await _db.AboutUs.FirstOrDefaultAsync(x => x.Id == id);
            if (dbAboutUs == null)
            {
                return View("Error");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (changeAboutUs.Photo != null)
            {


                if (!changeAboutUs.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please choose the ImageFile!");

                    return View();
                }

                if (changeAboutUs.Photo.IsMore5MB())
                {
                    ModelState.AddModelError("Photo", "Image Max 5MB!");

                    return View();

                }
                string path = Path.Combine(_env.WebRootPath, "assets/Image");
                dbAboutUs.Image = await changeAboutUs.Photo.SaveImageAsync(path);



            }
            dbAboutUs.Description = changeAboutUs.Description;
            dbAboutUs.Title = changeAboutUs.Title;
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
            AboutUs aboutUs = _db.AboutUs.FirstOrDefault(x => x.Id == id);
            if (aboutUs == null)
            {
                return View("Error");
            }
            return View(aboutUs);
        }
        #endregion


    }

    internal class AutohorizeAttribute : Attribute
    {
    }
}
