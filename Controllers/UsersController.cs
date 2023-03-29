
using HospitalMVC.DAL;
using HospitalMVC.Helpers;
using HospitalMVC.Models;
using HospitalMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalMVC.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;//Rollari idare edirik
        private readonly UserManager<AppUser> _userManager;//istifadecileri idare edirik!
        private readonly SignInManager<AppUser> _signManager;//Daxil olanlari idare edirik!
        private readonly AppDbContext _db;
        public UsersController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext db, SignInManager<AppUser> signManager)
        {
            _userManager = userManager;
            _db = db;
            _roleManager = roleManager;
            _signManager = signManager;
        }
        public async Task<IActionResult> Index()
        {
            List<AppUser> users = await _userManager.Users.ToListAsync();
            List<UserVM> userVMs = new List<UserVM>();
            foreach (AppUser user in users)
            {
                UserVM userVM = new UserVM
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    UserName = user.UserName,
                    Email = user.Email,
                    IsDeactive = user.IsDeactive,
                    Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
                };
                userVMs.Add(userVM);
            }
            return View(userVMs);
        }
        [HttpPost]
        public async Task<IActionResult> Index(string Name, string Surname, string Username)
        {

            List<AppUser> users = await _userManager.Users.Where(x => x.Name.StartsWith(Name) || x.Surname.StartsWith(Surname) ||
            x.UserName.StartsWith(Username)).ToListAsync();
            List<UserVM> userVMs = new List<UserVM>();
            foreach (AppUser user in users)
            {
                UserVM userVM = new UserVM
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    UserName = user.UserName,
                    Email = user.Email,
                    IsDeactive = user.IsDeactive,
                    Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
                };
                userVMs.Add(userVM);
            }
            return View(userVMs);
        }
        public async Task<IActionResult> Activity(string? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return View("Error");
            }
            if (user.IsDeactive)
            {
                user.IsDeactive = false;
            }
            else
            {
                user.IsDeactive = true;
            }
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> ChangeRole(string id)
        {
            {
                if (id == null)
                {
                    return View("Error");
                }
                AppUser user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return View("Error");
                }

                List<IdentityRole> roles = await _roleManager.Roles.ToListAsync();
                ViewBag.Roles = roles;
                //roles.Add(Helpers.Roles.Admin.ToString());
                //roles.Add(Helpers.Roles.Doctor.ToString());

                ChangeRoleVM changeRoleVM = new ChangeRoleVM
                {
                    OldRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault(),

                };
                return View(changeRoleVM);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(string id, ChangeRoleVM roleVM, string newRole)
        {

            if (id == null)
            {
                return View("Error");
            }
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return View("Error");
            }


            List<IdentityRole> roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = roles;
            //roles.Add(Helpers.Roles.Admin.ToString());
            //roles.Add(Helpers.Roles.Doctor.ToString());

            ChangeRoleVM changeRoleVM = new ChangeRoleVM
            {
                OldRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault(),

            };

            if (newRole != changeRoleVM.OldRole)
            {

                IdentityResult addIdentiyResult = await _userManager.AddToRoleAsync(user, newRole);
                if (!addIdentiyResult.Succeeded)
                {
                    foreach (IdentityError error in addIdentiyResult.Errors)
                    {
                        ModelState.AddModelError(" ", error.Description);
                    }
                }
                IdentityResult removeIdentiyResult = await _userManager.AddToRoleAsync(user, newRole);
                if (!removeIdentiyResult.Succeeded)
                {
                    foreach (IdentityError error in removeIdentiyResult.Errors)
                    {
                        ModelState.AddModelError(" ", error.Description);
                    }
                }
                return View(changeRoleVM);
            }
            await _userManager.UpdateAsync(user);

            return RedirectToAction("Index");

        }
  
        public async Task<IActionResult> CreateRole()
        {
            List<IdentityRole> roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = roles;
            if (!ModelState.IsValid)
            {
                return View();
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser newUser = new AppUser()
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                Email = registerVM.Email,
                UserName = registerVM.Username,

            };



            if (!ModelState.IsValid)
            {
                return View();
            }

            IdentityResult identityResult = await _userManager.CreateAsync(newUser, registerVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError(" ", error.Description);
                }
                return View();
            }
            //List<IdentityRole> roles = await _roleManager.Roles.ToListAsync();
            //ViewBag.Roles = roles.FirstOrDefault();

          
            
          
            if (!(await _roleManager.RoleExistsAsync(Roles.Doctor.ToString())))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = Roles.Doctor.ToString() });
            }
            await _userManager.AddToRoleAsync(newUser, Roles.Doctor.ToString());//Birinci Member yerine Admin olacaq
            await _signManager.SignInAsync(newUser, true);
          
            return RedirectToAction("Users","UserProfile", "@User.Identity.Name");

        }
        //public async Task CreateRoles()
        //{
          

        //}


        public async Task<IActionResult> UserProfile(string?Id)
        {

            if (User.Identity.Name==null)
            {
                return View("Error");
            }
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (appUser==null)
            {
                return View("Error");
            }


            return View(appUser);
        }

    [HttpGet]
        public async Task<IActionResult> UserUpdate()
        {
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            UserUpdateVM userUpdateVM = new UserUpdateVM();
            appUser.Name = userUpdateVM.Name;
            appUser.Surname = userUpdateVM.Surname;
            appUser.UserName = userUpdateVM.UserName;
            appUser.Email = userUpdateVM.Email;

            if (appUser == null)
            {
                return View("Error");
            }
        

            return View(userUpdateVM);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserUpdate( UserUpdateVM userUpdateViewModel)
        {
           
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);
           
            appUser.PasswordHash = _userManager.PasswordHasher.HashPassword(appUser, userUpdateViewModel.Password);
            IdentityResult result = await _userManager.UpdateAsync(appUser);
            if (appUser == null)
            {
                return View("Error");
            }
            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(" ", error.Description);
                }
            }      
            return RedirectToAction("Index", "Users");
        }


    }
}

