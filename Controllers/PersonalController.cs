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

    public class PersonalController : Controller
    {
        #region Database/and/Photo
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public PersonalController(IWebHostEnvironment env, AppDbContext db)
        {
            _db = db;
            _env = env;

        }
        #endregion
        #region Index
        public IActionResult Index(int page = 1)
        {
            ViewBag.PersonalCount = Math.Ceiling((decimal)_db.Personal.Count() / 5);
            ViewBag.SelectedPage = page;
            List<Personal> personal = _db.Personal.Include(x => x.Department).Skip((page - 1) * 5).Take(5).ToList();

            return View(personal);
        }
        [HttpPost]
        public IActionResult Index(string Name,string Surname,string Position)
        {

            List<Personal> personal = _db.Personal.Include(x => x.Department).Where(x=>x.Name.StartsWith(Name)|| x.Surname.StartsWith(Surname)|| x.Position.StartsWith(Position)).ToList();

            return View(personal);
        }
        #endregion
        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Personal personal = await _db.Personal.FirstOrDefaultAsync(x => x.Id == id);
            if (personal == null)
            {
                return View("Error");
            }
            if (personal.IsDeactive)
            {
                personal.IsDeactive = false;
            }
            else
            {
                personal.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion
        #region Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Department = await _db.Department.Where(x => x.IsDeactive == false).ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int departmentId, Personal newPersonal)
        {
            ViewBag.Department = await _db.Department.Where(x => x.IsDeactive == false).ToListAsync();
            if (!ModelState.IsValid)
            {

                return View();
            };
            if (newPersonal.Photo == null)
            {
                ModelState.AddModelError("Photo", "Şəkil seçin");
                return View();
            }
            if (!newPersonal.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Xaiş edirik ki şəkil faylı seçin!");

                return View();
            }
            if (newPersonal.Photo.IsMore5MB())
            {
                ModelState.AddModelError("Photo", "Seçdiyiniz şəkil maksimum 5mb ölçüyə sahib ola bilər!");

                return View();
            }
            string path = Path.Combine(_env.WebRootPath, "assets/Image");
            newPersonal.Image = await newPersonal.Photo.SaveImageAsync(path);


            newPersonal.DepartmentId = departmentId;
            await _db.Personal.AddAsync(newPersonal);
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
            Personal personal = await _db.Personal.FirstOrDefaultAsync(x => x.Id == id);
            if (personal == null)
            {
                return View("Error");
            }
            ViewBag.Department = await _db.Department.ToListAsync();

            return View(personal);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Personal changePersonal, int departmentId)
        {
            if (id == null)
            {
                return View("Error");
            }
            Personal dbPersonal = await _db.Personal.FirstOrDefaultAsync(x => x.Id == id);
            bool IsExist = _db.Doctor.Any(x => x.Name == dbPersonal.Name && x.Id != id);
            if (dbPersonal == null)
            {
                return View("Error");
            }
            ViewBag.Department = await _db.Department.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View(dbPersonal);
            }
            if (changePersonal.Photo != null)
            {

                if (!changePersonal.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Xaiş edirik ki şəkil faylı seçin!");

                    return View();
                }

                if (changePersonal.Photo.IsMore5MB())
                {
                    ModelState.AddModelError("Photo", "Seçdiyiniz şəkil maksimum 5mb ölçüyə sahib ola bilər!");

                    return View();

                }
                string path = Path.Combine(_env.WebRootPath, "assets/Image");
                dbPersonal.Image = await changePersonal.Photo.SaveImageAsync(path);
            }
            dbPersonal.Name = changePersonal.Name;
            dbPersonal.Age = changePersonal.Age;
            dbPersonal.Surname = changePersonal.Surname;
            dbPersonal.Salary = changePersonal.Salary;
            dbPersonal.Position = changePersonal.Position;

           
            dbPersonal.DepartmentId = departmentId;

            await _db.SaveChangesAsync();

            return RedirectToAction("Index");

        }
        #endregion
        #region Detail
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Personal personal = _db.Personal.FirstOrDefault(x => x.Id == id);
            ViewBag.Department = await _db.Department.ToListAsync();
            if (personal == null)
            {

                return View("Error");
            }
            return View(personal);
        }
        #endregion
    }
}
