using HospitalMVC.DAL;
using HospitalMVC.Helpers;
using HospitalMVC.Models;
using HospitalMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickMailer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.Controllers
{
   
    public class PatientsController : Controller
    {

        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public PatientsController(IWebHostEnvironment env, AppDbContext db)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index( int page = 1)
        {
            ViewBag.PatientCount = Math.Ceiling((decimal)_db.Patients.Count() / 10);
            ViewBag.SelectedPage = page;
            List<Patients> patients = await _db.Patients.Include(x => x.Reports).
                ThenInclude(x => x.Doctors).ThenInclude(x=>x.Department).
                Skip((page - 1) * 10).Take(10).ToListAsync();
            List<Department> departments = await _db.Department.ToListAsync();
            ViewBag.Department = departments;
            ViewBag.Doctor = departments.FirstOrDefault().Doctor;
            
            ViewBag.Doctor = await _db.Doctor.Include(x => x.Department).Include(x => x.Reports).ThenInclude(x => x.Patient).ToListAsync();



            return View(patients);
        }
        [HttpPost]
        public async Task<IActionResult> Index(string Name, string Surname)
        {

            List<Patients> patients = await _db.Patients.Include(x=>x.Reports).ThenInclude(x=>x.Doctors).Where(x => x.Name.StartsWith(Name) || x.Surname.StartsWith(Surname)).ToListAsync();
            List<Department> departments = await _db.Department.Include(x => x.Diseases).Include(x => x.Doctor).ToListAsync();
            ViewBag.Department = departments;
            ViewBag.Doctor = departments.FirstOrDefault().Doctor;
           

            return View(patients);
        }
    
        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Patients patients = await _db.Patients.Include(x=>x.Reports).ThenInclude(x=>x.Doctors).FirstOrDefaultAsync(x => x.Id == id);
            if (patients == null)
            {
                return View("Error");
            }
            if (patients.IsDeactive)
            {
                patients.IsDeactive = false;
            }
            else
            {
                patients.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        #region Create
        public async Task<IActionResult> Create()
        {
            List<Department> departments = await _db.Department.Include(x => x.Doctor).Include(x => x.Diseases).ToListAsync();
            ViewBag.Department = departments;
            ViewBag.Doctor = departments.FirstOrDefault().Doctor;
            ViewBag.Disease = departments.FirstOrDefault().Diseases;



            return View();
        }
        public async Task<IActionResult> LoadDoctorCategories(int? mainId)
        {
            if (mainId == null)
            {
                return View("Error");
            }
            List<Doctor> doctors = await _db.Doctor.Where(x => x.DepartmentId == mainId).ToListAsync();
            return PartialView("_LoadDoctorPartial", doctors);
        }
        public async Task<IActionResult> LoadDiseasesCategories(int? mainId)
        {
            if (mainId == null)
            {
                return View("Error");
            }
            List<Illnesses> diseases = await _db.Illnesses.Where(x => x.DepartmentId == mainId).ToListAsync();
            return PartialView("_LoadDiseasePartial", diseases);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Patients newPatient, int[] doctorId)
        {
            List<Department> departments = await _db.Department.Include(x => x.Doctor).
                ThenInclude(x => x.Reports).ThenInclude(x => x.Patient).ToListAsync();
            ViewBag.Department = departments;
            ViewBag.Doctor = departments.FirstOrDefault().Doctor;
            ViewBag.Disease = departments.FirstOrDefault().Diseases;



            if (!ModelState.IsValid)
            {
                return View();
            }
            List<Reports> reports = new List<Reports>();
            foreach (int item in doctorId)
            {
                Reports report = new Reports
                {
                    DoctorId = item,
                };
                reports.Add(report);
            }


            if (newPatient.Photo == null)
            {
                ModelState.AddModelError("Photo", "Please choose the Image!");
                return View();
            }
            if (!newPatient.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Please choose the ImageFile!");
                return View();
            }
            if (newPatient.Photo.IsMore5MB())
            {
                ModelState.AddModelError("Photo", "Image Max 5MB!");

                return View();
            }

            string path = Path.Combine(_env.WebRootPath, "assets/Image");
            newPatient.Image = await newPatient.Photo.SaveImageAsync(path);

            newPatient.Reports = reports;

            await _db.Patients.AddAsync(newPatient);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        #region Update
        public async Task<IActionResult> Update(int? id, int[] doctorId)
        {
            if (id == null)
            {
                return View("Error");
            }
            List<Department> departments = await _db.Department.Include(x => x.Doctor).ThenInclude(x => x.Reports).
                ThenInclude(x => x.Patient).ToListAsync();
            ViewBag.Department = departments;
            ViewBag.Doctor = departments.FirstOrDefault().Doctor;
            
            List<Reports> reports = new List<Reports>();


            Patients patients = await _db.Patients.Include(x => x.Reports).ThenInclude(x => x.Doctors).FirstOrDefaultAsync(x => x.Id == id);
            return View(patients);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Patients changePatients, int? id, int[] doctorId)
        {
            if (id == null)
            {
                return View("Error");
            }
            Patients dbpatients = await _db.Patients.Include(x => x.Reports).ThenInclude(x => x.Doctors).ThenInclude(x => x.Department).FirstOrDefaultAsync(x => x.Id == id);
            if (dbpatients == null)
            {
                return View("Error");
            }
            List<Department> departments = await _db.Department.Include(x => x.Doctor).
                ThenInclude(x => x.Reports).ThenInclude(x => x.Patient).ToListAsync();
            ViewBag.Department = departments;
            ViewBag.Doctor = dbpatients.Reports.FirstOrDefault().Doctors;
        

            List<Reports> reports = new List<Reports>();
            foreach (int item in doctorId)
            {
                Reports report = new Reports
                {
                    DoctorId = item,
                };
                reports.Add(report);
            }
            if (changePatients.Photo != null)
            {
                if (!changePatients.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please choose the ImageFile!");
                    return View();
                }
                if (changePatients.Photo.IsMore5MB())
                {
                    ModelState.AddModelError("Photo", "Image Max 5MB!");

                    return View();
                }
                string path = Path.Combine(_env.WebRootPath, "assets/Image");
                dbpatients.Image = await changePatients.Photo.SaveImageAsync(path);
            }
            dbpatients.Reports = reports;
            dbpatients.Name = changePatients.Name;
            dbpatients.Surname = changePatients.Surname;
            dbpatients.Age = changePatients.Age;
            dbpatients.Email = changePatients.Email;
            dbpatients.ApplyDate = changePatients.ApplyDate;
            dbpatients.Disease = changePatients.Disease;
            dbpatients.HomeAdress = changePatients.HomeAdress;


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
            Patients patients = await _db.Patients.Include(x => x.Reports).ThenInclude(x => x.Doctors).ThenInclude(x => x.Department).FirstOrDefaultAsync(x => x.Id == id);
            if (patients == null)
            {
                return View("Error");
            }
            List<Department> departments = await _db.Department.Include(x => x.Doctor).
                ThenInclude(x => x.Reports).ThenInclude(x => x.Patient).ToListAsync();
            ViewBag.Department = departments;
            ViewBag.Doctor = patients.Reports.FirstOrDefault().Doctors;
            ViewBag.Disease = departments.FirstOrDefault().Diseases;

            return View(patients);
        }
        #endregion
        public async Task<IActionResult> Doctors(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Patients patients = await _db.Patients.Include(x => x.Reports).ThenInclude(x => x.Doctors).FirstOrDefaultAsync(x => x.Id == id);
            ViewBag.Doctors = await _db.Doctor.Include(x => x.Reports).ThenInclude(x=>x.Patient).Include(x=>x.Department).ToListAsync();
            if (patients == null)
            {
                return View("Error");
            }
            return View(patients);
        }
    
        
        public async Task<IActionResult> SendMail(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            Patients patients = await _db.Patients.FirstOrDefaultAsync(x=>x.Id==id);
           
            MailMessage mailMessage = new MailMessage();
            string msg = " ";
            if (mailMessage.Subject==null)
            {
                
                return View();
            }

            try
            {
                mailMessage.To = patients.Email;

                await EmailUtil.SendEmailMessageAsync(patients.Name + " " + patients.Surname + " " + mailMessage.Subject, mailMessage.Body,patients.Email);
               
            }
            catch (Exception e)
            {
                msg = "Mesaj göndərilə bilmədi!";
            }
            ViewBag.Mgs = msg;



            return View(mailMessage);

        }
        [HttpPost]
        public async Task<IActionResult> SendMail(int? id, MailMessage mailMessage)
        {
            if (id==null)
            {
                return View("Error");
            }
            Patients patients = await _db.Patients.FirstOrDefaultAsync(x => x.Id == id);

            string msg = " ";

            if (mailMessage.Body == null)
            {

                ModelState.AddModelError("Body", " ");
                return View();
            }
            try
            {
                mailMessage.To = patients.Email;
                await EmailUtil.SendEmailMessageAsync(patients.Name+" "+patients.Surname+" "+mailMessage.Subject, mailMessage.Body, patients.Email);
                msg = "Mesaj göndərildi!";

            }
            catch (Exception e)
            {

                msg = "Mesaj göndərilə bilmədi!";
            }
            ViewBag.Mgs = msg;
            return View();

        }
    }
}



