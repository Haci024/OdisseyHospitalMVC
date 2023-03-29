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
    public class EquipmentsController : Controller
    {
        #region Database/and/Photo
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public EquipmentsController(IWebHostEnvironment env, AppDbContext db)
        {
            _db = db;
            _env = env;

        }
        #endregion
        #region Index
        public IActionResult Index(int page = 1)
        {
            ViewBag.EquipmentCount = ViewBag.PatientCount = Math.Ceiling((decimal)_db.Equipments.Count() / 5);
            ViewBag.SelectedPage = page;
            List<Equipments> equipments = _db.Equipments.Skip((page - 1) * 5).Take(5).ToList();
            return View(equipments);
        }
        [HttpPost]
        public IActionResult Index(string Name)
        {

            List<Equipments> equipments = _db.Equipments.Where(x => x.Name.StartsWith(Name)).ToList();

            return View(equipments);
        }
        #endregion
        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Equipments equipments = await _db.Equipments.FirstOrDefaultAsync(x => x.Id == id);
            if (equipments == null)
            {
                return View("Error");
            }
            if (equipments.IsDeactive)
            {
                equipments.IsDeactive = false;
            }
            else
            {
                equipments.IsDeactive = true;
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
        public async Task<IActionResult> Create( Equipments newEquipments)
        {
          
            if (!ModelState.IsValid)
            {

                return View();
            };
            bool IsExist = _db.Equipments.Any(x => x.Name == newEquipments.Name);
            if (IsExist == true)
            {
                ModelState.AddModelError("Name", "Bu alət artıq mövcuddur!");
                return View();
            }
            if (newEquipments.Photo == null)
            {
                ModelState.AddModelError("Photo", "Şəkil seçməyi unutdunuz!");
                return View();
            }
            if (!newEquipments.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Şəkil faylı seçin!");
                return View();
            }
            if (newEquipments.Photo.IsMore5MB())
            {
                ModelState.AddModelError("Photo", "Seçdiyiniz şəkil maksimum 5mb ölçüyə sahib ola bilər!");

                return View();
            }
            string path = Path.Combine(_env.WebRootPath, "assets/Image");
            newEquipments.Images = await newEquipments.Photo.SaveImageAsync(path);


          
            await _db.Equipments.AddAsync(newEquipments);
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
            Equipments dbequipments = await _db.Equipments.FirstOrDefaultAsync(x => x.Id == id);
            if (dbequipments == null)
            {
                return View("Error");
            }
        

            return View(dbequipments);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Equipments changeEquipments)
        {
            if (id == null)
            {
                return View("Error");
            }
            Equipments dbEquipments = await _db.Equipments.FirstOrDefaultAsync(x => x.Id == id);
           
            if (dbEquipments == null)
            {
                return View("Error");
            }
          
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (changeEquipments.Photo != null)
            {

                if (!changeEquipments.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Şəkil faylı seçin!");

                    return View();
                }

                if (changeEquipments.Photo.IsMore5MB())
                {
                    ModelState.AddModelError("Photo", "Seçdiyiniz şəkil maksimum 5mb ölçüyə sahib ola bilər!");

                    return View();

                }
                string path = Path.Combine(_env.WebRootPath, "assets/Image");
                dbEquipments.Images = await changeEquipments.Photo.SaveImageAsync(path);
            }
            dbEquipments.Name = changeEquipments.Name;
            dbEquipments.Price = changeEquipments.Price;
            dbEquipments.Description = changeEquipments.Description;
            dbEquipments.EquipmentCount = changeEquipments.EquipmentCount;


            await _db.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        #endregion
        #region Detail
        public  IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Equipments equipments = _db.Equipments.FirstOrDefault(x => x.Id == id);
          
            if (equipments == null)
            {

                return View("Error");
            }
            return View(equipments);
        }
        #endregion

    }
}
