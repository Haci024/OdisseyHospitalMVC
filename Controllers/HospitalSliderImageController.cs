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

    public class HospitalSliderImageController : Controller
    {
        #region Database/and/Photo
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public HospitalSliderImageController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        #endregion
        #region Index
        public  IActionResult Index(int page = 1)
        {
            ViewBag.SliderImageCount = Math.Ceiling((decimal)_db.SliderImages.Count() / 3);
            ViewBag.SelectedPage = page;
            List<HospitalSliderImage> hospitalsliderimages = _db.SliderImages.Skip((page - 1) * 3).Take(3).ToList();
            return View(hospitalsliderimages);
        }
        #endregion
        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            HospitalSliderImage sliderImage = await _db.SliderImages.FirstOrDefaultAsync(x => x.Id == id);
            if (sliderImage == null)
            {
                return View("Error");
            }
            if (sliderImage.IsDeactive)
            {
                sliderImage.IsDeactive = false;
            }
            else
            {
                sliderImage.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        #endregion
        #region Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HospitalSliderImage sliderImage)
        {
            if (sliderImage.Photo == null)
            {
                ModelState.AddModelError("Photo", "Şəkil seçməyi unutdunuz!");
                return View();
            }
            if (!sliderImage.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Şəkil faylı seçin!");
                return View();
            }
            if (sliderImage.Photo.IsMore5MB())
            {
                ModelState.AddModelError("Photo", "Seçdiyiniz şəkil maksimum 5mb ölçüyə sahib ola bilər!");

                return View();
            }
            string path = Path.Combine(_env.WebRootPath, "assets/Image");
            sliderImage.Image = await sliderImage.Photo.SaveImageAsync(path);
            await _db.SliderImages.AddAsync(sliderImage);
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
            HospitalSliderImage hospitalSliderImage = _db.SliderImages.FirstOrDefault(x => x.Id == id);
            if (hospitalSliderImage == null)
            {
                return View("Error");
            }
            return View(hospitalSliderImage);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(HospitalSliderImage changehospitalSliderImage, int? id)
        {
            if (id == null)
            {
                return View("Error");
            };
            HospitalSliderImage dbhospitalSliderImage = await _db.SliderImages.FirstOrDefaultAsync(x => x.Id == id);

            if (dbhospitalSliderImage == null)
            {
                return View("Error");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (changehospitalSliderImage.Photo != null)
            {

                if (!changehospitalSliderImage.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Şəkil faylı seçin!");

                    return View();
                }

                if (changehospitalSliderImage.Photo.IsMore5MB())
                {
                    ModelState.AddModelError("Photo", "Seçdiyiniz şəkil maksimum 5mb ölçüyə sahib ola bilər!");

                    return View();

                }
                string path = Path.Combine(_env.WebRootPath, "assets/Image");
                dbhospitalSliderImage.Image = await changehospitalSliderImage.Photo.SaveImageAsync(path);

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
            HospitalSliderImage hospitalSliderImage = _db.SliderImages.FirstOrDefault(x => x.Id == id);
            if (hospitalSliderImage == null)
            {
                return View("Error");
            }

            return View(hospitalSliderImage);


        }
        #endregion
    }
}

