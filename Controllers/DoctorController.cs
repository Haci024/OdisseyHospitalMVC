using HospitalMVC.DAL;
using HospitalMVC.Helpers;
using HospitalMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.Controllers
{
    [Authorize(Roles = "Admin,Doctor")]
    public class DoctorController : Controller
    {
        #region Database/and/Photo
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public DoctorController(IWebHostEnvironment env, AppDbContext db)
        {
            _db = db;
            _env = env;

        }
        #endregion
        #region Index
        public IActionResult Index(int page = 1)
        {
          
            ViewBag.DoctorNumCount = Math.Ceiling((decimal)_db.Doctor.Count() / 10);
            ViewBag.SelectedPage = page;
            List<Doctor> doctors = _db.Doctor.Include(x => x.Department).Skip((page - 1) * 10).OrderBy(x=>x.Id).Take(10).ToList();
            ViewBag.Department = _db.Department.Include(x => x.Doctor).ToList();
            return View(doctors);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string Name,string Surname,string Work)
        {

            List<Doctor> doctors = _db.Doctor.Include(x => x.Department).Where(x => x.Name.StartsWith(Name) || x.Surname.StartsWith(Surname)|| x.Work.StartsWith(Work)).ToList();
            ViewBag.Department = _db.Department.Include(x => x.Doctor).ToList();
            return View(doctors);
        }
        public async Task<IActionResult> LoadSelectDepartmentPartial(int? mainId)
        {
            if (mainId == null)
            {
                return View("Error");
            }
            List<Doctor> doctors = await _db.Doctor.Include(x => x.Department).Where(x =>x.DepartmentId == mainId).ToListAsync();
            return PartialView("_LoadSelectDepartmentPartial", doctors);
        }
        #endregion
        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Doctor doctor = await _db.Doctor.FirstOrDefaultAsync(x => x.Id == id);
            if (doctor == null)
            {
                return View("Error");
            }
            if (doctor.IsDeactive)
            {
                doctor.IsDeactive = false;
            }
            else
            {
                doctor.IsDeactive = true;
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
        public async Task<IActionResult> Create(int departmentId, Doctor newDoctor)
        {
            ViewBag.Department = await _db.Department.Where(x => x.IsDeactive == false).ToListAsync();
            if (!ModelState.IsValid)
            {

                return View();
            };
            if (newDoctor.DoctorPhoto == null)
            {
                ModelState.AddModelError("DoctorPhoto", "Şəkil seçin!");
                return View();
            }
            if (!newDoctor.DoctorPhoto.IsImage())
            {
                ModelState.AddModelError("DoctorPhoto", "Şəkil  faylı seçin!");

                return View();
            }
            if (newDoctor.DoctorPhoto.IsMore5MB())
            {
                ModelState.AddModelError("DoctorPhoto", "Şəkil maksimum 5mb ölçüyə sahib ola bilər!");

                return View();
            }
            string path = Path.Combine(_env.WebRootPath, "assets/Image");
            newDoctor.DoctorImage = await newDoctor.DoctorPhoto.SaveImageAsync(path);


            newDoctor.DepartmentId = departmentId;
            await _db.Doctor.AddAsync(newDoctor);
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
            Doctor doctor = await _db.Doctor.FirstOrDefaultAsync(x => x.Id == id);
            if (doctor == null)
            {
                return View("Error");
            }
            ViewBag.Department = await _db.Department.ToListAsync();

            return View(doctor);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Doctor changedoctor, int departmentId)
        {
            if (id == null)
            {
                return View("Error");
            }
            Doctor dbdoctor = await _db.Doctor.FirstOrDefaultAsync(x => x.Id == id);
            if (dbdoctor == null)
            {
                return View("Error");
            }
            ViewBag.Department = await _db.Department.ToListAsync();
            if (!ModelState.IsValid)
            {
                return View(dbdoctor);
            }
            if (changedoctor.DoctorPhoto != null)
            {

                if (!changedoctor.DoctorPhoto.IsImage())
                {
                    ModelState.AddModelError("DoctorPhoto", "Şəkil faylı seçin!");

                    return View();
                }

                if (changedoctor.DoctorPhoto.IsMore5MB())
                {
                    ModelState.AddModelError("DoctorPhoto", "Şəkil maksimum 5mb ölçüyə sahib ola bilər!");

                    return View();

                }
                string path = Path.Combine(_env.WebRootPath, "assets/Image");
                dbdoctor.DoctorImage = await changedoctor.DoctorPhoto.SaveImageAsync(path);
            }
            dbdoctor.Name = changedoctor.Name;
            dbdoctor.Surname = changedoctor.Surname;
            dbdoctor.WorkNumber = changedoctor.WorkNumber;
            dbdoctor.Email = changedoctor.Email;
            dbdoctor.Education = changedoctor.Education;
            dbdoctor.Type = changedoctor.Type;
            dbdoctor.HomeAdress = changedoctor.HomeAdress;
            dbdoctor.Work = changedoctor.Work;
            dbdoctor.Salary = changedoctor.Salary;
            dbdoctor.Description = changedoctor.Description;
            dbdoctor.BloodGroup = changedoctor.BloodGroup;
            dbdoctor.DepartmentId = departmentId;
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
            Doctor doctor = _db.Doctor.Include(x=>x.Department).Include(x=>x.Reports).ThenInclude(x=>x.Patient).FirstOrDefault(x => x.Id == id);
            ViewBag.Department = await _db.Department.ToListAsync();
            if (doctor == null)
            {

                return View("Error");
            }
            return View(doctor);
        }
        #endregion
        #region Patients
        public async Task<IActionResult> Patients(int? id)
        {
            if (id==null)
            {
                return View("Error");
            }
            Doctor doctor = await _db.Doctor.Include(x=>x.Department).FirstOrDefaultAsync(x=>x.Id==id);
            ViewBag.Patients =await _db.Patients.Include(x => x.Reports).ThenInclude(x => x.Doctors).ToListAsync();
            if (doctor==null)
            {
                return View("Error");
            }
            return View(doctor);
        }
        public async Task<IActionResult> SendMail(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            Doctor doctor = await _db.Doctor.FirstOrDefaultAsync(x => x.Id == id);
            string msg = " ";
            MailMessage mailMessage = new MailMessage();


            try
            {
                mailMessage.To = doctor.Email;
                
                    await EmailUtil.SendEmailMessageAsync( doctor.Name + " " + doctor.Surname + " " + mailMessage.Subject, mailMessage.Body, doctor.Email);

            }
            catch (Exception e)
            {

                msg = "Mail göndərilə bilmədi!";
            }
            ViewBag.Mgs = msg;



            return View(mailMessage);

        }
        [HttpPost]
        public async Task<IActionResult> SendMail(int? id, MailMessage mailMessage)
        {
            if (id == null)
            {
                return View("Error");
            }
            Doctor doctor = await _db.Doctor.FirstOrDefaultAsync(x => x.Id == id);

            string msg = " ";
           
            if (mailMessage.Subject == null)
            {

                ModelState.AddModelError("Subject", " ");
                return View();
            }
            try
            {
                mailMessage.To = doctor.Email;
                if (doctor.Type == "Kişi")
                {
                    await EmailUtil.SendEmailMessageAsync("Cənab Dk." + doctor.Name + " " + doctor.Surname + " " + mailMessage.Subject, mailMessage.Body, doctor.Email);
                }
                else
                {
                    await EmailUtil.SendEmailMessageAsync("Xanım Dk." + doctor.Name + " " + doctor.Surname + " " + mailMessage.Subject, mailMessage.Body, doctor.Email);
                }
                msg = "Mesaj göndərildi!";

            }
            catch (Exception e)
            {

                msg = "Mail göndərilə bilmədi!";
            }
            ViewBag.Mgs = msg;
            return View();

        }
        #endregion
    }

}




