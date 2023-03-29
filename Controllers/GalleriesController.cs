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
    //[Authorize(Roles = "Admin")]
    public class GalleriesController : Controller
    {

        #region Database/and/Photo
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public GalleriesController(IWebHostEnvironment env, AppDbContext db)
        {
            _db = db;
            _env = env;
        }
        #endregion
        #region Index
        public IActionResult Index(int page = 1)
        {
            ViewBag.GalleriesCount = Math.Ceiling((decimal)_db.Galleries.Count() / 5);
            ViewBag.SelectedPage = page;

            List<Gallery> galleries = _db.Galleries.Include(x => x.GalleryImages).Skip((page - 1) * 5).Take(5).ToList();
            return View(galleries);
        }
        #endregion
        #region Create
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Gallery gallery)
        {
            bool IsExist = _db.Galleries.Any(x => x.Title == gallery.Title);
            if (IsExist == true)
            {
                ModelState.AddModelError("Title", "Bu adda qalareya artıq mövcuddur!");
                return View();
            }

            if (!ModelState.IsValid)
            {
                return View();
            }
            if (gallery.Photos== null)
            {
                ModelState.AddModelError("Photos", "Şəkil seçməyi unutdunuz!");

                return View();
            }

            List<GalleryImages> galleryImages = new List<GalleryImages>();

            foreach (IFormFile photo in gallery.Photos)
            {
                if (photo == null)
                {
                    ModelState.AddModelError("Photos", "Şəkil seçməyi unutdunuz!");
                    return View();
                }
                if (!photo.IsImage())
                {
                    ModelState.AddModelError("Photos", "Şəkil faylı seçin!");
                    return View();
                }
                if (photo.IsMore5MB())
                {
                    ModelState.AddModelError("Photos", "Seçdiyiniz şəkil maksimum 5mb ölçüyə sahib ola bilər!");

                    return View();
                }
                string path = Path.Combine(_env.WebRootPath, "assets/Image");
                GalleryImages galleryImage = new GalleryImages();
                galleryImage.Image = await photo.SaveImageAsync(path);
                galleryImages.Add(galleryImage);

            }
            gallery.GalleryImages = galleryImages;
            await _db.Galleries.AddAsync(gallery);
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
            Gallery gallery = await _db.Galleries.Include(x => x.GalleryImages).FirstOrDefaultAsync(x=>x.Id==id);
            if (gallery == null)
            {
                return View("Error");
            }
            return View(gallery);
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Gallery changeGallery)
        {
            if (id == null)
            {
                return View("Error");
            }
            Gallery dbGallery = await _db.Galleries.Include(x => x.GalleryImages).FirstOrDefaultAsync(x=>x.Id==id);
            if (dbGallery == null)
            {
                return View("Error");
            }
          
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (changeGallery.Photos==null)
            {
                ModelState.AddModelError("Photos", "Galereyada ən az 1 şəkil olmalıdır!");
                return View();
            }
            if (changeGallery.Photos!=null)
            {
                List<GalleryImages> galleryImages = new List<GalleryImages>();
                foreach (IFormFile photo in changeGallery.Photos)
                {
                    GalleryImages galleryImage = new GalleryImages();

                    if (!photo.IsImage())
                    {
                        ModelState.AddModelError("Photos", "Şəkil faylı seçin!");
                        return View();
                    }
                    if (photo.IsMore5MB())
                    {
                        ModelState.AddModelError("Photos", "Seçdiyiniz şəkil maksimum 5mb ölçüyə sahib ola bilər!");

                        return View();
                    }
                    string path = Path.Combine(_env.WebRootPath, "assets/Image");
                    galleryImage.Image = await photo.SaveImageAsync(path);
                    galleryImages.Add(galleryImage);
                    
                }
                dbGallery.GalleryImages.AddRange(galleryImages);
               
            }
          
            


            dbGallery.Description = changeGallery.Description;
            dbGallery.Title = changeGallery.Title;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            GalleryImages galleryImage = await _db.GalleryImages.FirstOrDefaultAsync(x => x.Id == id);
         

            _db.GalleryImages.Remove(galleryImage);
            await _db.SaveChangesAsync();
            return Content("Ok");
        }
        #endregion
        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Gallery gallery = await _db.Galleries.FirstOrDefaultAsync(x => x.Id == id);
            if (gallery == null)
            {
                return View("Error");
            }
            if (gallery.IsDeactive)
            {
                gallery.IsDeactive = false;
            }
            else
            {
                gallery.IsDeactive = true;
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
            Gallery gallery = _db.Galleries.Include(x => x.GalleryImages).FirstOrDefault(x=>x.Id==id);
            if (gallery == null)
            {
                return View("Error");
            }
            return View(gallery);
        }
        #endregion

    }
}
