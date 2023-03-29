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
    public class HospitalBlogsController : Controller
    {

        #region Index/Database
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public HospitalBlogsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            ViewBag.BlogCount = Math.Ceiling((decimal)_db.SliderImages.Count() / 5);
            ViewBag.SelectedPage = page;

            List<HospitalBlogs> hospitalBlogs = _db.HospitalBlogs.Skip((page - 1) * 5).Take(5).ToList();

            return View(hospitalBlogs);

        }
#endregion
        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            HospitalBlogs hospitalBlogs = await _db.HospitalBlogs.FirstOrDefaultAsync(x => x.Id == id);
            if (hospitalBlogs == null)
            {
                return View("Error");
            }
            if (hospitalBlogs.IsDeactive)
            {
                hospitalBlogs.IsDeactive = false;
            }
            else
            {
                hospitalBlogs.IsDeactive = true;
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
        public async Task<IActionResult> Create(HospitalBlogs hospitalBlogs)
        {
          
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (hospitalBlogs.Photo == null)
            {
                ModelState.AddModelError("Photo", "Please choose the Image!");
                return View();
            }
            if (!hospitalBlogs.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Please choose the ImageFile!");

                return View();
            }
            if (hospitalBlogs.Photo.IsMore5MB())
            {
                ModelState.AddModelError("Photo", "Image Max 5MB!");

                return View();
            }
            string path = Path.Combine(_env.WebRootPath, "assets/Image");
            hospitalBlogs.BlogImage = await hospitalBlogs.Photo.SaveImageAsync(path);
            await _db.HospitalBlogs.AddAsync(hospitalBlogs);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        #region Update
        public IActionResult Update(int? id)
        {
            if (id==null)
            {
                return View("Error");
            }
            HospitalBlogs hospitalBlogs = _db.HospitalBlogs.Include(x => x.HospitalBlogsDetail).FirstOrDefault(x => x.Id == id);
            if (hospitalBlogs==null)
            {
                return View("Error");
            }
            return View(hospitalBlogs);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,HospitalBlogs changehospitalBlog)
        {
            if (id==null)
            {
                return View("Error");
            }
            HospitalBlogs dbhospitalBlog =await _db.HospitalBlogs.Include(x => x.HospitalBlogsDetail).FirstOrDefaultAsync(x => x.Id == id);
            if (dbhospitalBlog == null)
            {
                return View("Error");
            }
            if (changehospitalBlog.Photo!=null)
            {
                if (!changehospitalBlog.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please choose the ImageFile!");

                    return View();
                }

                if (changehospitalBlog.Photo.IsMore5MB())
                {
                    ModelState.AddModelError("Photo", "Image Max 5MB!");

                    return View();

                }
                string path = Path.Combine(_env.WebRootPath, "assets/Image");
                dbhospitalBlog.BlogImage = await changehospitalBlog.Photo.SaveImageAsync(path);
            }
            dbhospitalBlog.CreateTime = changehospitalBlog.CreateTime;
            dbhospitalBlog.Title = changehospitalBlog.Title;
            dbhospitalBlog.Subtitle = changehospitalBlog.Subtitle;
            dbhospitalBlog.HospitalBlogsDetail.Description = changehospitalBlog.HospitalBlogsDetail.Description;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
        #region Detail
        public IActionResult Detail(int? id)
        {
            if (id==null)
            {
                return View("Error");
            }
            HospitalBlogs hospitalBlogs = _db.HospitalBlogs.Include(x=>x.HospitalBlogsDetail).FirstOrDefault(x=>x.Id==id);
            if (hospitalBlogs==null)
            {
                return View("Error");
            }
            
            return View(hospitalBlogs);
        }
        #endregion
    }
}
