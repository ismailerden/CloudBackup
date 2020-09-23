using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CloudBackup.Database.Operations;
using CloudBackup.Database.ViewModel;
using CloudBackup.Database.ViewModel.CreateModel;
using CloudBackup.Database.ViewModel.DataTableModel;
using CloudBackup.WebPlatforms;
using CloudBackup.WebPlatforms.GoogleDrive;
using CloudBackup.WebApp.Core;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CloudBackup.WebApp.Controllers
{
    public class PlatformController : BaseController
    {
        private IHostingEnvironment _env;


        public PlatformController(IMemoryCache cache, IHostingEnvironment env) : base(cache)
        {
            _env = env;

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult New(string id)
        {
            int realId = WebUtilities.DecryptId(id, GetUserName(), GetOrganizationId());
            if (realId > 0)
            {
                PlatformInsertModel model = operations.Plan.GetPlanById(GetOrganizationId(), realId);
                model.HashedPlatformId = WebUtilities.EncryptId(model.Id, GetUserName(), GetOrganizationId());
                return View(model);
                //return View();
            }
            else
            {
                PlatformInsertModel model = new PlatformInsertModel();
                return View(model);
            }
        }
        [HttpPost]
        public IActionResult New(PlatformInsertModel model)
        {
            model.CloseModal = false;
            if (String.IsNullOrEmpty(model.Name))
            {
                TempData["Notification"] = WebUtilities.InsertNotification("Platform ismi boş geçilemez.");
                return View(model);
            }
            if (model.Type == Database.Enum.PlanType.Seçiniz)
            {
                TempData["Notification"] = WebUtilities.InsertNotification("Platform tipini seçiniz.");
                return View(model);
            }
            string jsonData = "";
            foreach (var item in Request.Form)
            {
                if (item.Key.StartsWith("Form:") == true)
                {
                    string header = item.Key.Replace("Form:", "");
                    string data = item.Value;
                    jsonData += "\"" + header + "\": \"" + data + "\",";
                }
            }
            if ((!String.IsNullOrEmpty(model.HashedPlatformId) && model.Type==Database.Enum.PlanType.GoogleDrive) || model.Type != Database.Enum.PlanType.GoogleDrive)
            {
                jsonData = jsonData.Substring(0, jsonData.Length - 1);
                jsonData = "{ " + jsonData + " }";

                model.JsonData = jsonData;
            }
            model.OrganizationId = GetOrganizationId();

            if (String.IsNullOrEmpty(model.HashedPlatformId))
            {
                model.OrganizationId = GetOrganizationId();




                int returnValue = operations.Plan.InsertPlatform(model);

                if (returnValue > 0)
                {
                    model.HashedPlatformId = WebUtilities.EncryptId(returnValue, GetUserName(), GetOrganizationId());
                    TempData["Notification"] = WebUtilities.InsertNotification("Cihaz başarılı bir şekilde eklenmiştir.");
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
                int realId = WebUtilities.DecryptId(WebUtility.UrlDecode(model.HashedPlatformId), GetUserName(), GetOrganizationId());
                if (realId > 0)
                {
                    model.CloseModal = true;
                    model.Id = realId;
                    operations.Plan.UpdatePlan(model, GetOrganizationId());
                    return View(model);
                }
                else
                {
                    TempData["Notification"] = WebUtilities.InsertNotification("Hatalı işlem lütfen pencereyi kapatıp tekrar deneyiniz.");
                    model.CloseModal = false;
                    return View(model);
                }
            }
        }


        public IActionResult AuthGoogleDrive()
        {

            return View();
        }
        [AllowAnonymous]
        public TokenResponse exchangeCode(string code, string redirectUri)
        {

            PlatformGoogle pg = new PlatformGoogle(_env.ContentRootPath);
            return pg.AuthCodeToAccessToken(code, redirectUri, "");

        }
        [AllowAnonymous]
        public ActionResult ReturnGoogleAuth(string code, string state)
        {
            TokenResponse trp = exchangeCode(code, "https://" + Request.Host.Value + "/Platform/ReturnGoogleAuth");

            int realPlanId = WebUtilities.DecryptId(state, GetUserName(), GetOrganizationId());

            var planData = operations.Plan.GetPlanById(GetOrganizationId(), realPlanId);

            dynamic dData = null;
            if (!String.IsNullOrEmpty(planData.JsonData))
                dData = JObject.Parse(planData.JsonData);

            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("accountId", realPlanId.ToString()));
            list.Add(new KeyValuePair<string, string>("accessToken", trp.AccessToken));
            if (!String.IsNullOrEmpty(trp.RefreshToken))
                list.Add(new KeyValuePair<string, string>("refreshToken", trp.RefreshToken));
            else if (dData != null && dData.refreshToken != null)
                list.Add(new KeyValuePair<string, string>("refreshToken", dData.refreshToken));
            list.Add(new KeyValuePair<string, string>("tokenExpire", DateTime.Now.AddSeconds(trp.ExpiresInSeconds.Value).ToString()));
            string jsonData = "";
            foreach (var item in list)
            {

                string header = item.Key;
                string data = item.Value;
                jsonData += "\"" + header + "\": \"" + data + "\",";

            }
            jsonData = jsonData.Substring(0, jsonData.Length - 1);
            jsonData = "{ " + jsonData + " }";

            planData.JsonData = jsonData;
            operations.Plan.UpdatePlan(planData, GetOrganizationId());

            TempData["Notification"] = WebUtilities.InsertNotification(planData.Name + " Planı google eşleştirmesi tamamlanmıştır. ");
            return Redirect("/Home/Dashboard");
        }
        [AllowAnonymous]

        public ActionResult GetGoogleDriveAuthAsync(string devicePlanId)
        {

            PlatformGoogle pg = new PlatformGoogle(_env.ContentRootPath);
            pg.AuthRequestUrl(Request.Host.Value);

            return Redirect(pg.AuthRequestUrl(Request.Host.Value) + "&state=" + WebUtility.UrlEncode(devicePlanId));

        }



        public IActionResult _GoogleDrive(string id)
        {
            var drive = operations.Plan.GetPlanById(GetOrganizationId(), WebUtilities.DecryptId(id, GetUserName(), GetOrganizationId()));
            drive.HashedPlatformId = WebUtility.UrlEncode(id);
            if (drive != null)
            {
                return View(drive);
            }
            else
            {
                PlatformInsertModel model = new PlatformInsertModel();
                return View(model);
            }
        }
        public IActionResult _Amazons3(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var drive = operations.Plan.GetPlanById(GetOrganizationId(), WebUtilities.DecryptId(id, GetUserName(), GetOrganizationId()));
                drive.HashedPlatformId = WebUtility.UrlEncode(id);
                if (drive != null)
                {
                    return View(drive);
                }
                else
                {
                    PlatformInsertModel model = new PlatformInsertModel();
                    return View(model);
                }
            }
            else
            {
                PlatformInsertModel model = new PlatformInsertModel();
                return View(model);
            }
        }
        public int DeletePlatform(string id)
        {
            int count = operations.Device.PlanDeviceCount(WebUtilities.DecryptId(WebUtility.UrlDecode(id), GetUserName(), GetOrganizationId()), GetOrganizationId());
            if (count > 0)
            {
                TempData["Notification"] = WebUtilities.InsertNotification("Plana bağlı cihazlar olduğu için silinemez.");
                return -1;
            }

            operations.Plan.DeletePlan(WebUtilities.DecryptId(WebUtility.UrlDecode(id), GetUserName(), GetOrganizationId()), GetOrganizationId());
            return 1;
        }
        public IActionResult GetPlatformDatatable(JQueryDataTableParamModel param)
        {
            List<PlanListModel> list = operations.Plan.GetListModel(GetOrganizationId(), param.iDisplayStart, param.iDisplayLength);
            List<string[]> listReturn = new List<string[]>();

            foreach (var item in list)
            {
                listReturn.Add(new[] { item.Name, item.Type.ToString(), WebUtilities.EncryptId(item.Id, GetUserName(), GetOrganizationId()) });
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
    }
}