using HospitalMVC.DAL;
using HospitalMVC.Models;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickMailer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalMVC.Helpers;
using MailMessage = HospitalMVC.Models.MailMessage;

namespace HospitalMVC.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class EmailController : Controller
    {
        private readonly AppDbContext _db;
        public EmailController(AppDbContext db)
        {
            _db = db;
        }
        #region Mail
        public IActionResult Send()
        {

           
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(MailMessage mailMessage)
        {
            string msg = " ";
            if (!ModelState.IsValid)
            {

                return View();
            }
            try
            {
               await  EmailUtil.SendEmailMessageAsync(mailMessage.Subject,mailMessage.Body, mailMessage.To);
              
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
