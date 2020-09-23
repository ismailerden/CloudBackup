using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CloudBackup.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using CloudBackup.Database.Operations;
using Microsoft.Extensions.Configuration;
using CloudBackup.Database.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using CloudBackup.WebApp.Core;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using CloudBackup.Database.ViewModel;
using Hangfire;
using CloudBackup.WebPlatforms.AmazonS3;
using CloudBackup.Database.ViewModel.DataTableModel;
using System.Net;
using CloudBackup.Database.ViewModel.CreateModel;

namespace CloudBackup.WebApp.Controllers
{
    //[Authorize]
    public class HomeController : BaseController
    {
        int _failedAttemptCount = 5;
        public HomeController(IMemoryCache cache) : base(cache)
        {

        }
        [AllowAnonymous]
        public IActionResult Index()
        {

            AddressBinding binding = GetOrganizationBinding();

            string url = Request.Host.Host;
            int port = Convert.ToInt32(Request.Host.Port);
            string cacheKey = url + ":" + port;

            //throw new Exception(cacheKey);

            if (binding == null)
                return Redirect("http://google.com");


            if (User.Identity.IsAuthenticated)
                return Redirect("/Home/Dashboard");

            return View();
        }
        public IActionResult OrganizationList()
        {
            if (GetUserId() != 0)
                return Redirect("/Home/Dashboard");
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> Login(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName) || String.IsNullOrEmpty(password))
            {
                TempData["Notification"] = WebUtilities.InsertNotification("Kullanıcı adı veya şifre boş geçilemez");
                return Redirect("/");
            }

            User user = operations.User.GetUserByUserName(userName, GetOrganizationId());
            if (user == null)
            {
                TempData["Notification"] = WebUtilities.InsertNotification("Kullanıcı adı veya şifre hatalı");
                return Redirect("/");
            }
            else
            {
                if (user.FailedLoginCount > _failedAttemptCount)
                {
                    TempData["Notification"] = WebUtilities.InsertNotification("Hesabınız kilitlenmiştir lütfen sistem yöneticinize başvurunuz.");
                }
                else
                {
                    IPasswordHasher<User> passwordHasher = new PasswordHasher<User>();
                    PasswordVerificationResult checkPassword = passwordHasher.VerifyHashedPassword(user, user.Password, password);
                    if (checkPassword == PasswordVerificationResult.Success)
                    {
                        var claims = new List<Claim>
{
    new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName ) ,
    new Claim(ClaimTypes.NameIdentifier, user.UserName),
    new Claim("OrganizationId", user.OrganizationId.ToString()),
    new Claim("UserId", user.Id.ToString()),
    new Claim(ClaimTypes.Role, "Administrator"),
};

                        var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            //AllowRefresh = <bool>,
                            // Refreshing the authentication session should be allowed.

                            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                            // The time at which the authentication ticket expires. A 
                            // value set here overrides the ExpireTimeSpan option of 
                            // CookieAuthenticationOptions set with AddCookie.

                            //IsPersistent = true,
                            // Whether the authentication session is persisted across 
                            // multiple requests. Required when setting the 
                            // ExpireTimeSpan option of CookieAuthenticationOptions 
                            // set with AddCookie. Also required when setting 
                            // ExpiresUtc.

                            //IssuedUtc = <DateTimeOffset>,
                            // The time at which the authentication ticket was issued.

                            RedirectUri = "/Home/Dashboard"
                            // The full path or absolute URI to be used as an http 
                            // redirect response value.
                        };

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);
                        operations.User.ResetFailedCount(user.Id);
                    }
                    else
                    {
                        operations.User.UpdateFailedLoginCount(user.Id);
                        if (_failedAttemptCount - user.FailedLoginCount - 1 > 0)
                            TempData["Notification"] = WebUtilities.InsertNotification("Kullanıcı adı veya şifre hatalı. Kalan giriş denemeniz:" + (_failedAttemptCount - user.FailedLoginCount - 1));
                        else
                            TempData["Notification"] = WebUtilities.InsertNotification("Hesabınız kilitlenmiştir lütfen sistem yöneticinize başvurunuz.");
                    }
                }
            }
            return Redirect("/");
        }
        public IActionResult Dashboard()
        {
            AmazonS3 ss = new AmazonS3();
            //ss.GetDirectoryList("", "");
            DashboardViewModel model = operations.Organization.GetDashboard(GetOrganizationId());
            return View(model);
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/Home/Index");
        }
        public IActionResult GetOrganizationDatatable(JQueryDataTableParamModel param)
        {
            if (GetUserId() != 0)
                return null;
            List<OrganizationListModel> list = operations.Organization.GetOrganizationList(param.iDisplayStart, param.iDisplayLength, 0);
            List<string[]> listReturn = new List<string[]>();

            foreach (var item in list)
            {
                listReturn.Add(new[] { item.Name, item.PersonFullName, item.ContactEmail, item.AddressBindings, item.ActiveStatus.ToString(), WebUtilities.EncryptId(item.Id, GetUserName(), GetOrganizationId()) });
            }
            int count = 0;
            if (list != null && list.Count > 0)
                count = list.FirstOrDefault().Count;

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = count,
                iTotalDisplayRecords = count,
                aaData = listReturn
            });

        }
        public int OrganizationChangeStatus(string id)
        {
            if (GetUserId() != 0)
                return -1;
            operations.Organization.ChangeOrganizationActiveStatus(WebUtilities.DecryptId(WebUtility.UrlDecode(id), GetUserName(), GetOrganizationId()));
            return 1;
        }
        public IActionResult OrganizationNew(string id)
        {
            if (GetUserId() != 0)
                return Redirect("/Home/Dashboard");
            int realId = WebUtilities.DecryptId(id, GetUserName(), GetOrganizationId());
            if (realId > 0)
            {
                var organizationModel = operations.Organization.GetOrganizationList(0, 99, realId).FirstOrDefault();
                OrganizationInsertModel model = new OrganizationInsertModel();
                model.AddressBindings = organizationModel.AddressBindings;
                model.ContactEmail = organizationModel.ContactEmail;
                model.Id = organizationModel.Id;
                model.HashedId = WebUtilities.EncryptId(model.Id, GetUserName(), GetOrganizationId());
                model.Name = organizationModel.Name;
                model.PersonFullName = organizationModel.PersonFullName;
                return View(model);
            }
            else
            {
                OrganizationInsertModel model = new OrganizationInsertModel();
                return View(model);
            }
        }
        [HttpPost]
        public IActionResult OrganizationNew(OrganizationInsertModel model)
        {
            if (GetUserId() != 0)
                return Redirect("/Home/Dashboard");
            model.CloseModal = false;
            if (String.IsNullOrEmpty(model.Name) || String.IsNullOrEmpty(model.PersonFullName) || String.IsNullOrEmpty(model.ContactEmail) || String.IsNullOrEmpty(model.AddressBindings))
            {
                TempData["Notification"] = WebUtilities.InsertNotification("İsim , ad , mail adresi , adresler boş geçilemez.");
                return View(model);
            }

            if (String.IsNullOrEmpty(model.HashedId))
            {
                int returnValue = operations.Organization.CreateOrganization(model);

                List<AddressBindingInsertModel> insertModel = new List<AddressBindingInsertModel>();
                string[] arrAddressBinding = model.AddressBindings.Split(',');
                for (int i = 0; i < arrAddressBinding.Length; i++)
                {
                    insertModel.Add(new AddressBindingInsertModel { Address = arrAddressBinding[i].Trim().Split(':')[0], Port = Convert.ToInt32(arrAddressBinding[i].Trim().Split(':')[1]), OrganizationId = returnValue });
                }

                operations.Organization.InsertAddressBindings(returnValue, insertModel);

                if (returnValue > 0)
                {
                    model.HashedId = WebUtilities.EncryptId(returnValue, GetUserName(), GetOrganizationId());
                    TempData["Notification"] = WebUtilities.InsertNotification("Organizasyon başarılı bir şekilde eklenmiştir.");
                    model.CloseModal = true;
                    return View(model);
                }
                else
                {
                    TempData["Notification"] = WebUtilities.InsertNotification("Ekleme sırasında bir hata oluştu lütfen sistem yöneticisine başvurunuz.");
                    model.CloseModal = false;
                    return View(model);
                }
            }
            else
            {
                int realId = WebUtilities.DecryptId(WebUtility.UrlDecode(model.HashedId), GetUserName(), GetOrganizationId());
                if (realId > 0)
                {
                    List<AddressBindingInsertModel> insertModel = new List<AddressBindingInsertModel>();
                    string[] arrAddressBinding = model.AddressBindings.Split(',');
                    for (int i = 0; i < arrAddressBinding.Length; i++)
                    {
                        insertModel.Add(new AddressBindingInsertModel { Address = arrAddressBinding[i].Trim().Split(':')[0], Port = Convert.ToInt32(arrAddressBinding[i].Trim().Split(':')[1]), OrganizationId = realId });
                    }
                    operations.Organization.DeleteAddressBinding(realId);
                    operations.Organization.InsertAddressBindings(realId, insertModel);
                    operations.Organization.UpdateOrganization(model);
                    return View(model);
                }
                else
                {
                    TempData["Notification"] = WebUtilities.InsertNotification("Hatalı işlem lütfen pencereyi kapatıp tekrar deneyiniz.");
                    model.CloseModal = false;
                    return View(model);
                }
            }
            // return View();
        }
        public IActionResult UsersList(string organizationId)
        {
            if (GetUserId() != 0)
                return Redirect("/Home/Dashboard");
            ViewBag.HashedOrganizationId = organizationId;
            ViewBag.OrganizationName = operations.Organization.GetOrganizationList(0, 9, WebUtilities.DecryptId(organizationId, GetUserName(), GetOrganizationId())).FirstOrDefault().Name;
            return View();
        }
        public IActionResult GetUsersDatatable(JQueryDataTableParamModel param, string organizationId)
        {
            organizationId = WebUtility.HtmlDecode(organizationId);
            if (GetUserId() != 0)
                return null;
            List<UserListModel> list = operations.User.GetUserList(param.iDisplayStart, param.iDisplayLength, WebUtilities.DecryptId(organizationId, GetUserName(), GetOrganizationId()));
            List<string[]> listReturn = new List<string[]>();

            foreach (var item in list)
            {
                listReturn.Add(new[] { item.UserName, item.FullName, item.Email, item.ActiveStatus.ToString(), WebUtilities.EncryptId(item.Id, GetUserName(), GetOrganizationId()) });
            }
            int count = 0;
            if (list != null && list.Count > 0)
                count = list.FirstOrDefault().Count;

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = count,
                iTotalDisplayRecords = count,
                aaData = listReturn
            });

        }
        public int UserChangeStatus(string id)
        {
            if (GetUserId() != 0)
                return -1;
            operations.User.ChangeActiveStatusUser(WebUtilities.DecryptId(WebUtility.UrlDecode(id), GetUserName(), GetOrganizationId()));
            return 1;
        }
        public IActionResult UsersNew(string id, string organizationId)
        {
            if (GetUserId() != 0)
                return Redirect("/Home/Dashboard");
            
            int realId = WebUtilities.DecryptId(id, GetUserName(), GetOrganizationId());
            ViewBag.hashedOrganizationId = organizationId;
            if (realId > 0)
            {
                var userModel = operations.User.GetUserById(realId);
                userModel.HashedId = WebUtilities.EncryptId(userModel.Id,GetUserName(),GetOrganizationId());
                return View(userModel);
            }
            else
            {
                UsersInsertModel model = new UsersInsertModel();
                return View(model);
            }
        }
        [HttpPost]
        public IActionResult UsersNew(UsersInsertModel model,string hashedOrganizationId)
        {
            if (GetUserId() != 0)
                return Redirect("/Home/Dashboard");
            model.CloseModal = false;
            if (String.IsNullOrEmpty(model.UserName) || String.IsNullOrEmpty(model.FirstName) || String.IsNullOrEmpty(model.LastName) || String.IsNullOrEmpty(model.Email))
            {
                TempData["Notification"] = WebUtilities.InsertNotification("İsim , ad , mail adresi , kullanıcı adı boş geçilemez.");
                return View(model);
            }

            if (String.IsNullOrEmpty(model.HashedId))
            {
                int returnValue = operations.Organization.CreateOrganization(model);

                List<AddressBindingInsertModel> insertModel = new List<AddressBindingInsertModel>();
                string[] arrAddressBinding = model.AddressBindings.Split(',');
                for (int i = 0; i < arrAddressBinding.Length; i++)
                {
                    insertModel.Add(new AddressBindingInsertModel { Address = arrAddressBinding[i].Trim().Split(':')[0], Port = Convert.ToInt32(arrAddressBinding[i].Trim().Split(':')[1]), OrganizationId = returnValue });
                }

                operations.Organization.InsertAddressBindings(returnValue, insertModel);

                if (returnValue > 0)
                {
                    model.HashedId = WebUtilities.EncryptId(returnValue, GetUserName(), GetOrganizationId());
                    TempData["Notification"] = WebUtilities.InsertNotification("Organizasyon başarılı bir şekilde eklenmiştir.");
                    model.CloseModal = true;
                    return View(model);
                }
                else
                {
                    TempData["Notification"] = WebUtilities.InsertNotification("Ekleme sırasında bir hata oluştu lütfen sistem yöneticisine başvurunuz.");
                    model.CloseModal = false;
                    return View(model);
                }
            }
            else
            {
                int realId = WebUtilities.DecryptId(WebUtility.UrlDecode(model.HashedId), GetUserName(), GetOrganizationId());
                if (realId > 0)
                {
                    List<AddressBindingInsertModel> insertModel = new List<AddressBindingInsertModel>();
                    string[] arrAddressBinding = model.AddressBindings.Split(',');
                    for (int i = 0; i < arrAddressBinding.Length; i++)
                    {
                        insertModel.Add(new AddressBindingInsertModel { Address = arrAddressBinding[i].Trim().Split(':')[0], Port = Convert.ToInt32(arrAddressBinding[i].Trim().Split(':')[1]), OrganizationId = realId });
                    }
                    operations.Organization.DeleteAddressBinding(realId);
                    operations.Organization.InsertAddressBindings(realId, insertModel);
                    operations.Organization.UpdateOrganization(model);
                    return View(model);
                }
                else
                {
                    TempData["Notification"] = WebUtilities.InsertNotification("Hatalı işlem lütfen pencereyi kapatıp tekrar deneyiniz.");
                    model.CloseModal = false;
                    return View(model);
                }
            }
            // return View();
        }
    }

}
