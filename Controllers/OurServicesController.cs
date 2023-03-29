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

    public class OurServicesController : Controller
    {
        #region Database/and/Photo
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public OurServicesController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        #endregion
        #region Index
        public IActionResult Index(int page=1)
        {
            ViewBag.ServiceCount = Math.Ceiling((decimal)_db.OurServices.Count() / 5);
            ViewBag.SelectedPage = page;
            List<OurServices> ourServices = _db.OurServices.Skip((page-1)*5).Take(5).ToList();

            return View(ourServices);
        }
        [HttpPost]
        public IActionResult Index(string ServiceName)
        {    
            List<OurServices> ourServices = _db.OurServices.Where(x=>x.ServiceName.StartsWith(ServiceName)).ToList();

            return View(ourServices);
        }
        #endregion
        #region Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OurServices ourServices)
        {
            bool IsExist = _db.OurServices.Any(x => x.ServiceName== ourServices.ServiceName);
            if (IsExist == true)
            {
                ModelState.AddModelError("ServiceName", "This Department already IsExist!");
                return View();
            }
            if (ourServices.Photo == null)
            {
                ModelState.AddModelError("Photo", "Please choose the Image!");
                return View();
            }
            if (!ourServices.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Please choose the ImageFile!");
                return View();
            }
            if (ourServices.Photo.IsMore5MB())
            {
                ModelState.AddModelError("Photo", "Image Max 5MB!");

                return View();
            }
            string path = Path.Combine(_env.WebRootPath, "assets/Image");
            ourServices.Image = await ourServices.Photo.SaveImageAsync(path);
            await _db.OurServices.AddAsync(ourServices);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        #region Update
        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            OurServices ourServices = _db.OurServices.FirstOrDefault(x => x.Id == id);
            if (ourServices == null)
            {
                return View("Error");

            }
            return View(ourServices);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, OurServices changeOurServices)
        {
            if (id == null)
            {
                return View("Error");
            }
            OurServices dbOurServices = await _db.OurServices.FirstOrDefaultAsync(x => x.Id == id);
            if (dbOurServices == null)
            {
                return View("Error");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (changeOurServices.Photo != null)
            {


                if (!changeOurServices.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please choose the ImageFile!");

                    return View();
                }

                if (changeOurServices.Photo.IsMore5MB())
                {
                    ModelState.AddModelError("Photo", "Image Max 5MB!");

                    return View();

                }
                string path = Path.Combine(_env.WebRootPath, "assets/Image");
                dbOurServices.Image = await changeOurServices.Photo.SaveImageAsync(path);



            }
            dbOurServices.ServiceName = changeOurServices.ServiceName;
            dbOurServices.Description = changeOurServices.Description;
          
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            OurServices ourServices = await _db.OurServices.FirstOrDefaultAsync(x => x.Id == id);
            if (ourServices == null)
            {
                return View("Error");
            }
            if (ourServices.Isdeactive)
            {
                ourServices.Isdeactive = false;
            }
            else
            {
                ourServices.Isdeactive = true;
            }
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
            OurServices ourservices = _db.OurServices.FirstOrDefault(x => x.Id == id);
            if (ourservices == null)
            {
                return View("Error");
            }
            return View(ourservices);
        }

        #endregion


    }
}

