using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CloudBackup.Database.Entity;
using CloudBackup.Database.Operations;
using CloudBackup.Database.ViewModel;
using CloudBackup.WebApp.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CloudBackup.WebApp.Controllers
{
    public class ProfileController : BaseController
    {
        public ProfileController(IMemoryCache cache) : base(cache)
        {

        }
        [HttpPost]
        public IActionResult ChangeProfileInformation(ProfileViewModel model)
        {
            model.OrganizationId = GetOrganizationId();
            model.UserId = WebUtilities.DecryptId(WebUtility.UrlDecode(model.HashedUserId), GetUserName(), GetOrganizationId());
            operations.User.UpdateProfile(model);
            TempData["Notification"] = WebUtilities.InsertNotification("Profiliniz başarılı bir şekilde değiştirilmiştir.");
            return Redirect("/Profile/Index");
        }
        public IActionResult Index()
        {
            var model = operations.User.GetProfile(GetUserId(), GetOrganizationId());
            model.HashedUserId = WebUtilities.EncryptId(model.UserId, GetUserName(), GetOrganizationId());
            return View(model);
        }
        public IActionResult ResetPassword()
        {

            if(String.IsNullOrEmpty( Request.Form["txtOldPassword"]))
            {
                TempData["Notification"] = WebUtilities.InsertNotification("Mevcut şifrenizi girmediniz.");
                return Redirect("/Profile/Index");
            }
            if (String.IsNullOrEmpty(Request.Form["txtNewPassword"]))
            {
                TempData["Notification"] = WebUtilities.InsertNotification("Yeni şifre belirlemediniz.");
                return Redirect("/Profile/Index");
            }
            if (Request.Form["txtNewPassword"] != Request.Form["txtNewPasswordRety"])
            {
                TempData["Notification"] = WebUtilities.InsertNotification("Yeni şifre ile şifre tekrar uyuşmuyor.");
                return Redirect("/Profile/Index");
            }
            if(Request.Form["txtNewPassword"].ToString().Length<7)
            {
                TempData["Notification"] = WebUtilities.InsertNotification("Şifre en az 8 karakterden oluşmalı.");
                return Redirect("/Profile/Index");
            }
            if (Request.Form["txtNewPassword"].ToString().IndexOf(' ')>0)
            {
                TempData["Notification"] = WebUtilities.InsertNotification("Şifrenizde boşluk karakteri bulunmamalı.");
                return Redirect("/Profile/Index");
            }

            var model = operations.User.GetUserByUserName(GetUserName(), GetOrganizationId());

            IPasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            PasswordVerificationResult checkPassword = passwordHasher.VerifyHashedPassword(model, model.Password, Request.Form["txtOldPassword"]);
            if (checkPassword == PasswordVerificationResult.Success)
            {
                IPasswordHasher<User> passwordHasherNew = new PasswordHasher<User>();
                string checkPasswordNew = passwordHasher.HashPassword(model, Request.Form["txtNewPassword"].ToString());
                operations.User.ChangePassword(GetUserId(), checkPasswordNew);
                TempData["Notification"] = WebUtilities.InsertNotification("Şifreniz başarılı bir şekilde değiştirilmiştir.");
            }
            else
            {
                TempData["Notification"] = WebUtilities.InsertNotification("Mevcut şifrenizi yanlış girdiniz.");
            }
           
            return Redirect("/Profile/Index");
        }
    }
}