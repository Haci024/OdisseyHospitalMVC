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
    public class RoomsController : Controller
    {
        #region Database/and/Photo
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public RoomsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        #endregion
        #region Index
        public async Task<IActionResult> Index(int page = 1)
        {
            ViewBag.RoomCount = Math.Ceiling((decimal)_db.Rooms.Count() / 8);

            List<Rooms> room = await _db.Rooms.Include(x=>x.Department).Skip((page - 1) * 8).Take(8).ToListAsync();
            ViewBag.Department = await _db.Department.ToListAsync();
            return View(room);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string Roomtype)
        {

            List<Rooms> rooms =await _db.Rooms.Include(x => x.Department).Where(x => x.Roomtype.StartsWith(Roomtype)).ToListAsync();
            ViewBag.Department = _db.Department.Include(x => x.Doctor).ToList();
            return View(rooms);
        }
        #endregion
        #region Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Department = await _db.Department.ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rooms Newroom)
        {
            ViewBag.Department = await _db.Department.ToListAsync();
            bool IsExist = _db.Rooms.Any(x => x.RoomNum == Newroom.RoomNum);
            if (IsExist == true)
            {
                ModelState.AddModelError("RoomNum", "Bu otaq artıq mövcuddur!");
                return View();
            }


            await _db.Rooms.AddAsync(Newroom);
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
            Rooms dbroom = _db.Rooms.Include(x => x.Department).FirstOrDefault(x => x.Id == id);
            ViewBag.Department =  _db.Department.ToList();
            if (dbroom == null)
            {
                return View("Error");
            }

            return View(dbroom);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Rooms changeRoom, int departmentId)
        {
            if (id == null)
            {
                return View("Error");
            }
          
            Rooms dbRoom = await _db.Rooms.Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == id);
            ViewBag.Department = await _db.Department.ToListAsync();
            if (dbRoom == null)
            {
                return View("Error");
            }
            bool IsExist = _db.Rooms.Any(x => x.RoomNum == changeRoom.RoomNum && x.Id != changeRoom.Id);
            if (IsExist == true)
            {
                ModelState.AddModelError("RoomNum", "Bu otaq artıq mövcuddur!");

                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            dbRoom.DepartmentId = departmentId;
            dbRoom.RoomNum = changeRoom.RoomNum;
            dbRoom.Roomtype = changeRoom.Roomtype;
            dbRoom.RoomCapacity = changeRoom.RoomCapacity;
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

            Rooms Room = await _db.Rooms.Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == id);
            if (Room == null)
            {

                return View("Error");

            }
            if (Room.IsDeactive)
            {

                Room.IsDeactive = false;

            }
            else
            {

                Room.IsDeactive = true;

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
            Rooms Room = _db.Rooms.Include(x=>x.Department).FirstOrDefault(x => x.Id == id);
            ViewBag.Department =  _db.Department.ToList();
            if (Room == null)
            {
                return View("Error");
            }
            return View(Room);
        }
        #endregion

    }
}
