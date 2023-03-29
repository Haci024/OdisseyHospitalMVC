using HospitalMVC.DAL;
using HospitalMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.Controllers
{
    //[Authorize(Roles = "Admin")]

    public class IllnessesController : Controller
    {

        #region Database/and/Photo
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public IllnessesController(IWebHostEnvironment env, AppDbContext db)
        {
            _db = db;
            _env = env;

        }
        #endregion
        #region Index
        public IActionResult Index(int page = 1)
        {
            ViewBag.IllnessCount = Math.Ceiling((decimal)_db.SliderImages.Count() / 5);
            ViewBag.SelectedPage = page;
            List<Illnesses> illnesses = _db.Illnesses.Include(x => x.Department).Skip((page - 1) * 5).Take(30).ToList();

            return View(illnesses);
        }
        [HttpPost]
        public IActionResult Index(string Ilness)
        {

            List<Illnesses> illnesses = _db.Illnesses.Include(x => x.Department).Where(x => x.Ilness.StartsWith(Ilness)).ToList();

            return View(illnesses);
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
        public async Task<IActionResult> Create(int departmentId, Illnesses newIllness)
        {
            ViewBag.Department = await _db.Department.Where(x => x.IsDeactive == false).ToListAsync();
            if (!ModelState.IsValid)
            {

                return View();
            };

            newIllness.DepartmentId = departmentId;
            await _db.Illnesses.AddAsync(newIllness);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        #region  Update
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Illnesses illnesses = await _db.Illnesses.FirstOrDefaultAsync(x => x.Id == id);
            if (illnesses == null)
            {
                return View("Error");
            }
            ViewBag.Department = await _db.Department.ToListAsync();

            return View(illnesses);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Illnesses changeIllness, int departmentId)
        {
            if (id == null)
            {
                return View("Error");
            }
            Illnesses dbIllness = await _db.Illnesses.FirstOrDefaultAsync(x => x.Id == id);
            if (dbIllness == null)
            {
                return View("Error");
            }
            ViewBag.Department = await _db.Department.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }


            dbIllness.DepartmentId = departmentId;
            dbIllness.HealthyMethod = changeIllness.HealthyMethod;
            dbIllness.Ilness = changeIllness.Ilness;


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
            Illnesses illnesses = _db.Illnesses.Include(x=>x.Department).FirstOrDefault(x => x.Id == id);
            ViewBag.Department = await _db.Department.ToListAsync();
            if (illnesses == null)
            {

                return View("Error");
            }
            return View(illnesses);
        }
        #endregion

    }
}
  