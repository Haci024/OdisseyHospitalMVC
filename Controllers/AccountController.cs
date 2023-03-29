using HospitalMVC.DAL;
using HospitalMVC.Helpers;
using HospitalMVC.Models;
using HospitalMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

////namespace Employers.Controllers
////{

////    public class AccountController : Controller
////    {
////        private readonly AppDbContext _db;
////        private readonly UserManager<AppUser> _userManager;
////        private readonly SignInManager<AppUser> _signInManager;
////        private readonly RoleManager<IdentityRole> _roleManager;
////        public AccountController(UserManager<AppUser> userManager,
////            RoleManager<IdentityRole> roleManager,
////            SignInManager<AppUser> signInManager,
////            AppDbContext db)
////        {
////            _roleManager = roleManager;
////            _userManager = userManager;
////            _signInManager = signInManager;
////            _db = db;
////        }
////        public async Task<IActionResult> Logout()
////        {
////            if (!User.Identity.IsAuthenticated)
////            {
////                return NotFound();
////            }
////            await _signInManager.SignOutAsync();
////            return RedirectToAction("Index", "Dashboard");
////        }
////        public async Task<IActionResult> Login()
////        {
////            if (User.Identity.IsAuthenticated)
////            {
////                return NotFound();
////            }
////            ViewBag.IsExistAdmin = false;
////            HasAdmin hasAdmin = await _db.HasAdmins.FirstOrDefaultAsync();
////            if (hasAdmin.Hasadmin)
////            {
////                ViewBag.IsExistAdmin = true;
////            }
////            return View();
////        }
////        [HttpPost]
////        [ValidateAntiForgeryToken]
////        public async Task<IActionResult> Login(LoginVM loginVm)
////        {
////            ViewBag.IsExistAdmin = false;
////            HasAdmin hasAdmin = await _db.HasAdmins.FirstOrDefaultAsync();
////            if (hasAdmin.Hasadmin)
////            {
////                ViewBag.IsExistAdmin = true;
////            }
////            AppUser appUser = await _userManager.FindByNameAsync(loginVm.Username);
////            if (appUser == null)
////            {
////                ModelState.AddModelError("Username", "İstifadəçi adı yalnışdır");
////                return View();
////            }
////            if (appUser.IsDeactive)
////            {
////                ModelState.AddModelError("", "İstifadəçi blok olunub");
////                return View();
////            }
////            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(appUser, loginVm.Password, loginVm.RememberMe, true);
////            if (signInResult.IsLockedOut)
////            {
////                ModelState.AddModelError("", "Şifrəni 5 dəfə səhv yazdığınız üçün müvəqqəti olaraq bloklandı");
////                return View();
////            }
////            if (!signInResult.Succeeded)
////            {
////                ModelState.AddModelError("", "Şifrə düzgün deyil");
////                return View();
////            }
////            return RedirectToAction("Index", "Dashboard");
////        }
////        public async Task<IActionResult> CreateAdmin()
////        {
////            if (User.Identity.IsAuthenticated)
////            {
////                return NotFound();
////            }
////            await CreateRoles();
////            AppUser appUser = await _userManager.FindByNameAsync("Admin");
////            if (appUser != null)
////            {
////                return NotFound();
////            }

////            AppUser newUser = new AppUser
////            {
////                FullName = "Admin",
////                UserName = "Admin",
////                Email = "Admin@admin.com"

////            };
////            await _userManager.CreateAsync(newUser, "Admin1234");
////            await _userManager.AddToRoleAsync(newUser, "SuperAdmin");
////            HasAdmin hasAdmin = await _db.HasAdmins.FirstOrDefaultAsync();
////            hasAdmin.Hasadmin = true;
////            await _db.SaveChangesAsync();
////            return RedirectToAction("Login");
////        }
////        public async Task CreateRoles()
////        {
////            if (!await _roleManager.RoleExistsAsync("SuperAdmin"))
////            {
////                await _roleManager.CreateAsync(new IdentityRole { Name = "SuperAdmin" });
////            }
////            if (!await _roleManager.RoleExistsAsync("Admin"))
////            {
////                await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
////            }
////        }
////        #region MyRegion
////        public IActionResult ForgotPasswordForEmail()
////        {
////            return View();
////        }
////        [HttpPost]
////        [ValidateAntiForgeryToken]
////        public async Task<IActionResult> ForgotPasswordForEmailAsync(ForgotPasswordForEmailVM forEmailVM)
////        {
////            AppUser appUser = await _userManager.FindByEmailAsync(forEmailVM.Email);
////            if (appUser == null)
////            {
////                ModelState.AddModelError("Email", "Belə bir email yoxdur");
////                return View();
////            }
////            string random = Helper.GetRandomString();
////            appUser.Random = random;
////            await _userManager.UpdateAsync(appUser);
////            var message = "<div><p>Sizin birdəfəlik təsdiq şifrəniz </p>" + $"<p style='color:red;'>{random}</p></div>";
////            await Helper.SendEmailMessageAsync("Şifrə yeniləmə", message, forEmailVM.Email);
////            return RedirectToAction("ForgotPasswordVerify", new { email = forEmailVM.Email });
////        }
////        public async Task<IActionResult> ForgotPasswordVerifyAsync(string email)
////        {
////            AppUser appUser = await _userManager.FindByEmailAsync(email);
////            if (appUser == null)
////            {
////                return NotFound();
////            }
////            return View();
////        }
////        [HttpPost]
////        [ValidateAntiForgeryToken]
////        public async Task<IActionResult> ForgotPasswordVerifyAsync(string email, ForgotPasswordVerify forgotPasswordVerify)
////        {
////            AppUser appUser = await _userManager.FindByEmailAsync(email);
////            if (appUser == null)
////            {
////                return NotFound();
////            }

////            if (forgotPasswordVerify.Random.Length != 6)
////            {
////                ModelState.AddModelError("Random", "Birdəfəlik şifrə 6 xarakterdən olmalıdır");
////                return View();
////            }
////            if (forgotPasswordVerify.Random != appUser.Random)
////            {
////                ModelState.AddModelError("Random", "Birdəfəlik şifrə yanlışdır");
////                return View();
////            }

////            return RedirectToAction("ForgotPassword", new { email });
////        }

////        public async Task<IActionResult> ForgotPassword(string email)
////        {
////            if (email == null)
////            {
////                return NotFound();
////            }
////            AppUser user = await _userManager.FindByEmailAsync(email);
////            if (user == null)
////            {
////                return NotFound();
////            }
////            return View();
////        }
////        [HttpPost]
////        [ValidateAntiForgeryToken]
////        public async Task<IActionResult> ForgotPassword(string email, ResetPasswordVM resetPassword)
////        {
////            if (email == null)
////            {
////                return NotFound();
////            }
////            AppUser user = await _userManager.FindByEmailAsync(email);
////            if (user == null)
////            {
////                return NotFound();
////            }
////            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
////            IdentityResult identityResult = await _userManager.ResetPasswordAsync(user, token, resetPassword.Password);
////            if (!identityResult.Succeeded)
////            {
////                ModelState.AddModelError("", "Şifrə min 6 simboldan ibarət olmalıdır");
////                ModelState.AddModelError("", "Şifrədə ən az 1 ədəd kiçik hərf olmalıdır");
////                ModelState.AddModelError("", "Şifrədə ən az 1 ədəd böyük hərf olmalıdır");
////                ModelState.AddModelError("", "Şifrədə ən az 1 ədəd rəqəm hərf olmalıdır");
////                return View();
////            }
////            return RedirectToAction("Login");
////        }
////        #endregion
////    }
////}

namespace HospitalMVC.Controllers
{

    public class AccountController : Controller
    {


        private readonly RoleManager<IdentityRole> _roleManager;//Rollari idare edirik
        private readonly UserManager<AppUser> _userManager;//istifadecileri idare edirik!
        private readonly SignInManager<AppUser> _signManager;//Daxil olanlari idare edirik!
        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, SignInManager<AppUser> signManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signManager = signManager;
        }
        public IActionResult Login()
        {

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser appUser = await _userManager.FindByEmailAsync(loginVM.Email);
            if (appUser == null)
            {
                ModelState.AddModelError("", "Email və ya Şifrə yalnışdır!");
                return View();
            }
            if (appUser.IsDeactive)
            {
                ModelState.AddModelError("", "Sizin hesabınız artıq blok edilib!");
                return View();
            }
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signManager.PasswordSignInAsync(appUser, loginVM.Password, true, true);
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "Sizin girişiniz 1 dəqiqə müddətinə blok edilib");

                return View();
            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Email və ya Şifrə yalnışdır!");
                return View();
            }
            return RedirectToAction("Index", "Dashboard");
        }
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View("Error");
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
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
            IdentityResult identityResult = await _userManager.CreateAsync(newUser, registerVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError(" ", error.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(newUser, Roles.Doctor.ToString());//Birinci Member yerine Admin olacaq
            await _signManager.SignInAsync(newUser, true);
            return RedirectToAction("Index", "Users");

        }
        public async Task<IActionResult> Logout()
        {

            await _signManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }


        public async Task CreateRoles()
        {
            if (!(await _roleManager.RoleExistsAsync(Roles.Doctor.ToString())))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = Roles.Admin.ToString() });
            }
            if (!(await _roleManager.RoleExistsAsync(Roles.Doctor.ToString())))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = Roles.Doctor.ToString() });
            }

        }
    }
}

