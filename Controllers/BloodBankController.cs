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
    public class BloodBankController : Controller
    {
        #region Database/and/Photo
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public BloodBankController(IWebHostEnvironment env, AppDbContext db)
        {
            _db = db;
            _env = env;

        }
        #endregion
        #region
        public IActionResult Index(int page = 1)
        {
            ViewBag.BloodCount = Math.Ceiling((decimal)_db.BloodBanks.Count() / 5);
            ViewBag.SelectedPage = page;
            List<BloodBank> bloodGroup = _db.BloodBanks.Skip((page - 1) * 5).Take(5).ToList();
            return View(bloodGroup);
        }
     
        #endregion
        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            BloodBank bloodBank = await _db.BloodBanks.FirstOrDefaultAsync(x => x.Id == id);
            if (bloodBank == null)
            {
                return View("Error");
            }
            if (bloodBank.IsDeactive)
            {
                bloodBank.IsDeactive = false;
            }
            else
            {
                bloodBank.IsDeactive = true;
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
        public async Task<IActionResult> Create(BloodBank newBlood)
        {

            if (!ModelState.IsValid)
            {

                return View();
            };
            bool IsExist = _db.BloodBanks.Any(x => x.BloodGroup == newBlood.BloodGroup);
            if (IsExist == true)
            {
                ModelState.AddModelError("BloodGroup", "Bu qan qrupu artıq mövcuddur!");
                return View();
            }
            if (newBlood.Photo == null)
            {
                ModelState.AddModelError("Photo", "Şəkil seçməyi unutdunuz!");
                return View();
            }
            if (!newBlood.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Şəkil faylı seçin!");
                return View();
            }
            if (newBlood.Photo.IsMore5MB())
            {
                ModelState.AddModelError("Photo", "Seçdiyiniz şəkil maksimum 5mb ölçüyə sahib ola bilər!");

                return View();
            }
            string path = Path.Combine(_env.WebRootPath, "assets/Image");
            newBlood.Image = await newBlood.Photo.SaveImageAsync(path);



            await _db.BloodBanks.AddAsync(newBlood);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        #region Update
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            BloodBank dbBlood = await _db.BloodBanks.FirstOrDefaultAsync(x => x.Id == id);
            if (dbBlood == null)
            {
                return View("Error");
            }


            return View(dbBlood);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, BloodBank changeBlood)
        {
            if (id == null)
            {
                return View("Error");
            }
            BloodBank dbBlood = await _db.BloodBanks.FirstOrDefaultAsync(x => x.Id == id);

            if (changeBlood == null)
            {
                return View("Error");
            }

            if (!ModelState.IsValid)
            {
                return View(dbBlood);
            }
            if (changeBlood.Photo != null)
            {

                if (!changeBlood.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Şəkil faylı seçin!");

                    return View();
                }

                if (changeBlood.Photo.IsMore5MB())
                {
                    ModelState.AddModelError("Photo", "Seçdiyiniz şəkil maksimum 5mb ölçüyə sahib ola bilər!");

                    return View();

                }
                string path = Path.Combine(_env.WebRootPath, "assets/Image");
                dbBlood.Image = await changeBlood.Photo.SaveImageAsync(path);
            }
            dbBlood.BloodGroup = changeBlood.BloodGroup;
            dbBlood.BloodQuantity = changeBlood.BloodQuantity;
            dbBlood.About = changeBlood.About;
          


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
            BloodBank bloodBank = _db.BloodBanks.FirstOrDefault(x => x.Id == id);

            if (bloodBank == null)
            {

                return View("Error");
            }
            return View(bloodBank);
        }
        #endregion
    }
}
