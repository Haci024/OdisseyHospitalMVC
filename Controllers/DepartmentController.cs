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
    [Authorize(Roles ="Admin")]

    public class DepartmentController : Controller
    {
        #region Database/and/Photo
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public DepartmentController(IWebHostEnvironment env, AppDbContext db)
        {
            _db = db;
            _env = env;
        }
        #endregion
        #region Index
        public IActionResult Index(int page = 1)
        {
            ViewBag.DepartmentCount = Math.Ceiling((decimal)_db.Department.Count() / 5);
            ViewBag.SelectedPage = page;

            List<Department> departments = _db.Department.Skip((page - 1) * 5).Take(5).ToList();
            return View(departments);
        }
        [HttpPost]
        public IActionResult Index(string DepartmentName)
        {

            List<Department> departments = _db.Department.Where(x => x.DepartmentName.StartsWith(DepartmentName)).ToList();

            return View(departments);
        }
        #endregion
        #region Create
        public IActionResult Create()
        {
          
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            bool IsExist = _db.Department.Any(x => x.DepartmentName == department.DepartmentName);
            if (IsExist == true)
            {
                ModelState.AddModelError("DepartmentName", "Bu departament artıq mövcuddur!");
                return View();
            }
            if (department.Photo == null)
            {
                ModelState.AddModelError("Photo", "Şəkil seçməyi unutdunuz!");
                return View();
            }
            if (!department.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Şəkil faylı seçin!");
                return View();
            }
            if (department.Photo.IsMore5MB())
            {
                ModelState.AddModelError("Photo", "Seçdiyiniz şəkil maksimum 5mb ölçüyə sahib ola bilər!");

                return View();
            }
            string path = Path.Combine(_env.WebRootPath, "assets/Image");
            department.Image = await department.Photo.SaveImageAsync(path);
            await _db.Department.AddAsync(department);
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
            Department department = _db.Department.FirstOrDefault(x => x.Id == id);
            if (department == null)
            {
                return View("Error");
            }
            return View(department);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Department changedDepartment)
        {
            if (id == null)
            {
                return View("Error");
            }
            Department dbDepartment = await _db.Department.FirstOrDefaultAsync(x => x.Id == id);
            if (dbDepartment == null)
            {
                return View("Error");
            }
            bool IsExist = _db.Department.Any(x => x.DepartmentName == changedDepartment.DepartmentName && x.Id != id);

            if (IsExist == true)
            {
                ModelState.AddModelError("DepartmentName", "Bu departament artıq mövcuddur!");

                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (changedDepartment.Photo != null)
            {

                if (!changedDepartment.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Şəkil faylı seçməyiniz tələb olunur!");

                    return View();
                }

                if (changedDepartment.Photo.IsMore5MB())
                {
                    ModelState.AddModelError("Photo", "Seçdiyiniz şəkil maksimum 5mb ölçüyə sahib ola bilər!");

                    return View();

                }
                string path = Path.Combine(_env.WebRootPath, "assets/Image");
                dbDepartment.Image = await changedDepartment.Photo.SaveImageAsync(path);

            }
            dbDepartment.DepartmentName = changedDepartment.DepartmentName;
            dbDepartment.Description = changedDepartment.Description;
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
            Department department = await _db.Department.FirstOrDefaultAsync(x => x.Id == id);
            if (department == null)
            {
                return View("Error");
            }
            if (department.IsDeactive)
            {
                department.IsDeactive = false;
            }
            else
            {
                department.IsDeactive = true;
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
            Department department = _db.Department.FirstOrDefault(x=>x.Id==id);
            if (department == null)
            {
                return View("Error");
            }
            return View(department);
        }
        #endregion
        #region Doctors
      
        public async Task<IActionResult> Doctor(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

           
            Department department = await _db.Department.Include(x => x.Doctor).FirstOrDefaultAsync(x => x.Id == id);
           
            if (department == null)
            {
                return View("Error");
            }

            return View(department);
        }
        #endregion
        #region Personals
        public async Task<IActionResult> Personal(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            Department department = await _db.Department.Include(x => x.Personals).FirstOrDefaultAsync(x => x.Id == id);

            if (department == null)
            {
                return View("Error");
            }

            return View(department);
        }
        #endregion

    }
}
