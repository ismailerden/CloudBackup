using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CloudBackup.WebPlatforms.AmazonS3;

namespace CloudBackup.WebApp.Controllers
{
    public class TreeViewClass
    {
        public string id { get; set; }
        public string text { get; set; }
        public bool children { get; set; }
    }
    public class DeviceController : BaseController
    {

        public DeviceController(IMemoryCache cache, IHostingEnvironment env) : base(cache)
        {
            _env = env;
        }
        private IHostingEnvironment _env;


        public IActionResult GetDeviceDatatable(JQueryDataTableParamModel param)
        {
            List<DeviceListModel> list = operations.Device.GetListModel(GetOrganizationId(), param.iDisplayStart, param.iDisplayLength);
            List<string[]> listReturn = new List<string[]>();

            foreach (var item in list)
            {
                listReturn.Add(new[] { item.Name, item.LastProcessTime.HasValue == true ? item.LastProcessTime.Value.ToString() : "", item.CreatedDate.HasValue == true ? item.CreatedDate.Value.ToString() : "", item.DeviceStatus.ToString(), WebUtilities.EncryptId(item.Id, GetUserName(), GetOrganizationId()) });
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
        public IActionResult GetDeviceJobDatatable(JQueryDataTableParamModel param)
        {
            List<DevicePlanListModel> list = operations.DevicePlan.GetListModel(GetOrganizationId(), param.iDisplayStart, param.iDisplayLength, WebUtilities.DecryptId(WebUtility.UrlDecode(Request.Query["deviceId"]), GetUserName(), GetOrganizationId()));
            List<string[]> listReturn = new List<string[]>();

            foreach (var item in list)
            {
                string retryPlan = "";

                CloudBackup.Database.Enum.RetryPlan pl = ((CloudBackup.Database.Enum.RetryPlan)Convert.ToInt32(item.RetryPlan));
                if (pl == CloudBackup.Database.Enum.RetryPlan.OneTime)
                {
                    retryPlan = "Bir kere";
                }
                if (pl == CloudBackup.Database.Enum.RetryPlan.Minute30)
                {
                    retryPlan = "30 dakika da bir";
                }
                else if (pl == CloudBackup.Database.Enum.RetryPlan.Hour1)
                {
                    retryPlan = "Her saat";
                }
                else if (pl == CloudBackup.Database.Enum.RetryPlan.Hour3)
                {
                    retryPlan = "3 saate bir";
                }
                else if (pl == CloudBackup.Database.Enum.RetryPlan.Hour6)
                {
                    retryPlan = "6 saate bir";
                }
                else if (pl == CloudBackup.Database.Enum.RetryPlan.Hour12)
                {
                    retryPlan = "12 saate bir";
                }
                else if (pl == CloudBackup.Database.Enum.RetryPlan.Day1)
                {
                    retryPlan = "Her Gün";
                }
                else if (pl == CloudBackup.Database.Enum.RetryPlan.Day2)
                {
                    retryPlan = "2 günde bir";
                }
                else if (pl == CloudBackup.Database.Enum.RetryPlan.Day3)
                {
                    retryPlan = "3 günde bir";
                }
                else if (pl == CloudBackup.Database.Enum.RetryPlan.Day4)
                {
                    retryPlan = "4 günde bir";
                }
                else if (pl == CloudBackup.Database.Enum.RetryPlan.Day5)
                {
                    retryPlan = "5 günde bir";
                }
                else if (pl == CloudBackup.Database.Enum.RetryPlan.Day6)
                {
                    retryPlan = "6 günde bir";
                }
                else if (pl == CloudBackup.Database.Enum.RetryPlan.Day7)
                {
                    retryPlan = "7 günde bir";
                }
                else if (pl == CloudBackup.Database.Enum.RetryPlan.Day8)
                {
                    retryPlan = "8 günde bir";
                }
                else if (pl == CloudBackup.Database.Enum.RetryPlan.Day9)
                {
                    retryPlan = "9 günde bir";
                }
                else if (pl == CloudBackup.Database.Enum.RetryPlan.Day10)
                {
                    retryPlan = "10 günde bir";
                }
                else if (pl == CloudBackup.Database.Enum.RetryPlan.Month1)
                {
                    retryPlan = "Ayda bir";
                }
                else if (pl == CloudBackup.Database.Enum.RetryPlan.Month2)
                {
                    retryPlan = "2 ayda bir";
                }
                else if (pl == CloudBackup.Database.Enum.RetryPlan.Month3)
                {
                    retryPlan = "3 ayda bir";
                }
                else if (pl == CloudBackup.Database.Enum.RetryPlan.Month4)
                {
                    retryPlan = "4 ayda bir";
                }
                else if (pl == CloudBackup.Database.Enum.RetryPlan.Year1)
                {
                    retryPlan = "Senede bir";
                }



                listReturn.Add(new[] { item.Name, item.Description, item.PlanName, retryPlan, item.LastProcessTime.HasValue == true ? item.LastProcessTime.Value.ToString() : "", WebUtilities.EncryptId(item.Id, GetUserName(), GetOrganizationId()) });
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
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult New(string id)
        {
            int realId = WebUtilities.DecryptId(id, GetUserName(), GetOrganizationId());
            if (realId > 0)
            {
                DeviceInsertModel model = operations.Device.GetDeviceById(GetOrganizationId(), realId);
                model.HashedDeviceId = WebUtilities.EncryptId(model.DeviceId, GetUserName(), GetOrganizationId());
                return View(model);
            }
            else
            {
                DeviceInsertModel model = new DeviceInsertModel();
                return View(model);
            }
        }
        public List<LogListModel> GetLogModelList(int skip, int take, int devicePlanId = 0)
        {
            return operations.Log.GetListModel(GetOrganizationId(), devicePlanId, skip, take);
        }
        public IActionResult GetLogDatatableWithDevicePlanId(JQueryDataTableParamModel param)
        {
            int devicePlanId = 0;
            if (!String.IsNullOrEmpty(Request.Query["devicePlanId"]))
                devicePlanId = WebUtilities.DecryptId(WebUtility.UrlDecode(Request.Query["devicePlanId"]), GetUserName(), GetOrganizationId());
            List<LogListModel> list = GetLogModelList(param.iDisplayStart, param.iDisplayLength, devicePlanId);
            List<string[]> listReturn = new List<string[]>();

            if (devicePlanId > 0)
                foreach (var item in list)
                {
                    listReturn.Add(new[] { item.ProcessDate.ToString(), item.LogText });
                }
            else
                foreach (var item in list)
                {
                    listReturn.Add(new[] { item.ProcessDate.ToString(), item.DeviceName, item.PlanName, item.LogText });
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
        public IActionResult ReloadApiInformation(string id)
        {
            operations.Device.ResetKeyAndAccesKey(GetOrganizationId(), WebUtilities.DecryptId(id, GetUserName(), GetOrganizationId()));
            return Redirect("/Device/Information?id=" + WebUtility.UrlEncode(id));
        }
        [HttpPost]
        public IActionResult New(DeviceInsertModel model)
        {

            model.CloseModal = false;
            if (String.IsNullOrEmpty(model.DeviceName) || String.IsNullOrEmpty(model.DeviceDescription))
            {
                TempData["Notification"] = WebUtilities.InsertNotification("Cihaz ismi ve açıklaması boş geçilemez.");
                return View(model);
            }
            if (String.IsNullOrEmpty(model.HashedDeviceId))
            {
                int returnValue = operations.Device.InsertJob(GetOrganizationId(), model.DeviceName, model.DeviceDescription);

                if (returnValue > 0)
                {
                    model.HashedDeviceId = WebUtilities.EncryptId(returnValue, GetUserName(), GetOrganizationId());
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
                int realId = WebUtilities.DecryptId(WebUtility.UrlDecode(model.HashedDeviceId), GetUserName(), GetOrganizationId());
                if (realId > 0)
                {
                    model.CloseModal = true;
                    model.DeviceId = realId;
                    operations.Device.UpdateDevice(model, GetOrganizationId());
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
        public int DeleteDevice(string id)
        {
            int count = operations.Device.DevicePlanCount(WebUtilities.DecryptId(WebUtility.UrlDecode(id), GetUserName(), GetOrganizationId()), GetOrganizationId());
            if (count > 0)
            {
                TempData["Notification"] = WebUtilities.InsertNotification("Cihaza bağlı planlar olduğu için silinemez.");
                return -1;
            }
            DeviceInsertModel model = operations.Device.GetDeviceById(GetOrganizationId(), WebUtilities.DecryptId(WebUtility.UrlDecode(id), GetUserName(), GetOrganizationId()));
            operations.Device.DeleteDevice(WebUtilities.DecryptId(WebUtility.UrlDecode(id), GetUserName(), GetOrganizationId()), GetOrganizationId());
            List<string> stringModel = new List<string>();
            if (!String.IsNullOrEmpty(model.BakcupJobId))
                stringModel.Add(model.BakcupJobId);
            if (!String.IsNullOrEmpty(model.CheckOnlineJobId))
                stringModel.Add(model.CheckOnlineJobId);
            if (!String.IsNullOrEmpty(model.FileListJobId))
                stringModel.Add(model.FileListJobId);
            HangfireProvider.DeleteHangFireJobs(stringModel);
            return 1;
        }
        public bool DeleteDevicePlan(string id)
        {
            var model = operations.DevicePlan.GetDevicePlanById(GetOrganizationId(), WebUtilities.DecryptId(WebUtility.UrlDecode(id), GetUserName(), GetOrganizationId()));
            HangfireProvider.DeleteHangFireJobs(new List<string> { model.BackgroundJobId });
            operations.DevicePlan.DeleteDevicePlan(WebUtilities.DecryptId(WebUtility.UrlDecode(id), GetUserName(), GetOrganizationId()), GetOrganizationId());
            return true;
        }
        public IActionResult Information(string id)
        {
            DeviceApiInformation inf = operations.Device.GetDeviceApiInformation(WebUtilities.DecryptId(id, GetUserName(), GetOrganizationId()), GetOrganizationId());
            inf.HashedDeviceId = WebUtility.UrlEncode(id);
            return View(inf);
        }
        public IActionResult JobList(string id)
        {

            string hashedId = WebUtility.UrlEncode(id);
            ViewBag.DeviceId = hashedId;
            var model = operations.Device.GetDeviceById(GetOrganizationId(), WebUtilities.DecryptId(id, GetUserName(), GetOrganizationId()));
            return View(model);
        }
        public void SetNewJobViewBag()
        {
            var list = operations.Plan.GetSelectList(GetOrganizationId());

            foreach (var item in list)
                item.Id = WebUtilities.EncryptId(item.RealId, GetUserName(), GetOrganizationId());

            ViewBag.Platforms = list;
        }
        public IActionResult NewJob(string id, string deviceId)
        {

            SetNewJobViewBag();

            int realId = WebUtilities.DecryptId(id, GetUserName(), GetOrganizationId());
            if (realId > 0)
            {
                DevicePlanInsertModel model = operations.DevicePlan.GetDevicePlanById(GetOrganizationId(), realId);
                model.HashedDevicePlanId = WebUtilities.EncryptId(model.Id, GetUserName(), GetOrganizationId());
                model.BackupStartDate = model.RealInsertBackupStartDate.Value.Day + "-" + model.RealInsertBackupStartDate.Value.Month + "-" + model.RealInsertBackupStartDate.Value.Year;
                model.BackupStartTime = model.RealInsertBackupStartDate.Value.Hour + ":" + model.RealInsertBackupStartDate.Value.Minute;
                model.HashedPlanId = WebUtilities.EncryptId(model.PlanId, GetUserName(), GetOrganizationId());
                model.DeviceHashedId = deviceId;
                return View(model);
            }
            else
            {
                DevicePlanInsertModel model = new DevicePlanInsertModel();
                model.DeviceHashedId = deviceId;
                return View(model);
            }

        }
        public IActionResult DirectoryListing(string deviceId, string cloudId)
        {
            ViewBag.CloudId = WebUtility.UrlEncode(cloudId);
            ViewBag.DeviceId = WebUtility.UrlEncode(deviceId);
            return View();
        }
        public IActionResult DeviceJobLog(string id)
        {
            string hashedId = WebUtility.UrlEncode(id);
            ViewBag.DevicePlanId = hashedId;
            var item = operations.DevicePlan.GetDevicePlanById(GetOrganizationId(), WebUtilities.DecryptId(id, GetUserName(), GetOrganizationId()));
            return View(item);
        }
        public IActionResult AllJobLog()
        {
            return View();
        }
        private string DailyCron(int day, int seek)
        {
            string dayCron = "";
            int lastDay = 0;
            for (int i = day; i <= 31; i += seek)
            {
                dayCron += i + ",";
                lastDay = i;
            }
            if (seek - (31 - lastDay) > 0)
                for (int i = seek - (31 - lastDay); i < day; i += seek)
                    dayCron += i + ",";
            dayCron = dayCron.Substring(0, dayCron.Length - 1);
            return dayCron;
        }
        private string MonthCron(int month, int seek)
        {
            string monthCron = "";
            int lastMonth = 0;
            for (int i = month; i <= 12; i += seek)
            {
                monthCron += i + ",";
                lastMonth = i;
            }
            if (seek - (12 - lastMonth) > 0)
                for (int i = seek - (12 - lastMonth); i < month; i += seek)
                    monthCron += i + ",";
            monthCron = monthCron.Substring(0, monthCron.Length - 1);
            return monthCron;
        }
        public void TriggerHangfireJob(DevicePlanInsertModel model)
        {
            model.RealInsertBackupStartDate = model.RealInsertBackupStartDate.Value.AddHours(-3);
            if (model.RetryPlan == Database.Enum.RetryPlan.OneTime)
                HangfireProvider.InsertJobOneTimeBackup(model.RealInsertBackupStartDate.Value, model.Id, model.DeviceId, GetOrganizationId());
            else
            {
                string retryPlan = "";
                if (model.RetryPlan == Database.Enum.RetryPlan.Minute30)
                    retryPlan = "" + model.RealInsertBackupStartDate.Value.Minute + "/30 * * * *";
                else if (model.RetryPlan == Database.Enum.RetryPlan.Hour1)
                    retryPlan = "" + model.RealInsertBackupStartDate.Value.Minute + " " + model.RealInsertBackupStartDate.Value.Hour + "/1 * * *";
                else if (model.RetryPlan == Database.Enum.RetryPlan.Hour3)
                    retryPlan = "" + model.RealInsertBackupStartDate.Value.Minute + " " + model.RealInsertBackupStartDate.Value.Hour + "/3 * * *";
                else if (model.RetryPlan == Database.Enum.RetryPlan.Hour6)
                    retryPlan = "" + model.RealInsertBackupStartDate.Value.Minute + " " + model.RealInsertBackupStartDate.Value.Hour + "/6 * * *";
                else if (model.RetryPlan == Database.Enum.RetryPlan.Hour12)
                    retryPlan = "" + model.RealInsertBackupStartDate.Value.Minute + " " + model.RealInsertBackupStartDate.Value.Hour + "/12 * * *";
                else if (model.RetryPlan == Database.Enum.RetryPlan.Day1)
                    retryPlan = "" + model.RealInsertBackupStartDate.Value.Minute + " " + model.RealInsertBackupStartDate.Value.Hour + " " + DailyCron(model.RealInsertBackupStartDate.Value.Day, 1) + " * *";
                else if (model.RetryPlan == Database.Enum.RetryPlan.Day2)
                    retryPlan = "" + model.RealInsertBackupStartDate.Value.Minute + " " + model.RealInsertBackupStartDate.Value.Hour + " " + DailyCron(model.RealInsertBackupStartDate.Value.Day, 2) + " * *";
                else if (model.RetryPlan == Database.Enum.RetryPlan.Day3)
                    retryPlan = "" + model.RealInsertBackupStartDate.Value.Minute + " " + model.RealInsertBackupStartDate.Value.Hour + " " + DailyCron(model.RealInsertBackupStartDate.Value.Day, 3) + " * *";
                else if (model.RetryPlan == Database.Enum.RetryPlan.Day4)
                    retryPlan = "" + model.RealInsertBackupStartDate.Value.Minute + " " + model.RealInsertBackupStartDate.Value.Hour + " " + DailyCron(model.RealInsertBackupStartDate.Value.Day, 4) + " * *";
                else if (model.RetryPlan == Database.Enum.RetryPlan.Day5)
                    retryPlan = "" + model.RealInsertBackupStartDate.Value.Minute + " " + model.RealInsertBackupStartDate.Value.Hour + " " + DailyCron(model.RealInsertBackupStartDate.Value.Day, 5) + " * *";
                else if (model.RetryPlan == Database.Enum.RetryPlan.Day6)
                    retryPlan = "" + model.RealInsertBackupStartDate.Value.Minute + " " + model.RealInsertBackupStartDate.Value.Hour + " " + DailyCron(model.RealInsertBackupStartDate.Value.Day, 6) + " * *";
                else if (model.RetryPlan == Database.Enum.RetryPlan.Day7)
                    retryPlan = "" + model.RealInsertBackupStartDate.Value.Minute + " " + model.RealInsertBackupStartDate.Value.Hour + " " + DailyCron(model.RealInsertBackupStartDate.Value.Day, 7) + " * *";
                else if (model.RetryPlan == Database.Enum.RetryPlan.Day8)
                    retryPlan = "" + model.RealInsertBackupStartDate.Value.Minute + " " + model.RealInsertBackupStartDate.Value.Hour + " " + DailyCron(model.RealInsertBackupStartDate.Value.Day, 8) + " * *";
                else if (model.RetryPlan == Database.Enum.RetryPlan.Day9)
                    retryPlan = "" + model.RealInsertBackupStartDate.Value.Minute + " " + model.RealInsertBackupStartDate.Value.Hour + " " + DailyCron(model.RealInsertBackupStartDate.Value.Day, 9) + " * *";
                else if (model.RetryPlan == Database.Enum.RetryPlan.Day10)
                    retryPlan = "" + model.RealInsertBackupStartDate.Value.Minute + " " + model.RealInsertBackupStartDate.Value.Hour + " " + DailyCron(model.RealInsertBackupStartDate.Value.Day, 10) + " * *";
                else if (model.RetryPlan == Database.Enum.RetryPlan.Month1)
                    retryPlan = "" + model.RealInsertBackupStartDate.Value.Minute + " " + model.RealInsertBackupStartDate.Value.Hour + " " + model.RealInsertBackupStartDate.Value.Day + " " + MonthCron(model.RealInsertBackupStartDate.Value.Month, 1) + " *";
                else if (model.RetryPlan == Database.Enum.RetryPlan.Month2)
                    retryPlan = "" + model.RealInsertBackupStartDate.Value.Minute + " " + model.RealInsertBackupStartDate.Value.Hour + " " + model.RealInsertBackupStartDate.Value.Day + " " + MonthCron(model.RealInsertBackupStartDate.Value.Month, 2) + " *";
                else if (model.RetryPlan == Database.Enum.RetryPlan.Month3)
                    retryPlan = "" + model.RealInsertBackupStartDate.Value.Minute + " " + model.RealInsertBackupStartDate.Value.Hour + " " + model.RealInsertBackupStartDate.Value.Day + " " + MonthCron(model.RealInsertBackupStartDate.Value.Month, 3) + " *";
                else if (model.RetryPlan == Database.Enum.RetryPlan.Month4)
                    retryPlan = "" + model.RealInsertBackupStartDate.Value.Minute + " " + model.RealInsertBackupStartDate.Value.Hour + " " + model.RealInsertBackupStartDate.Value.Day + " " + MonthCron(model.RealInsertBackupStartDate.Value.Month, 4) + " *";
                else if (model.RetryPlan == Database.Enum.RetryPlan.Year1)
                    retryPlan = "" + model.RealInsertBackupStartDate.Value.Minute + " " + model.RealInsertBackupStartDate.Value.Hour + " " + model.RealInsertBackupStartDate.Value.Day + " " + model.RealInsertBackupStartDate.Value.Month + " *";


                HangfireProvider.InsertJobTimePlanBackup(model.Id, retryPlan, model.DeviceId, GetOrganizationId());
            }
        }
        [HttpPost]
        public IActionResult NewJob(DevicePlanInsertModel model)
        {
            SetNewJobViewBag();
            model.CloseModal = false;
            if (String.IsNullOrEmpty(model.Name))
            {
                TempData["Notification"] = WebUtilities.InsertNotification("İsim boş geçilemez");
                return View(model);
            }
            if (String.IsNullOrEmpty(model.Description))
            {
                TempData["Notification"] = WebUtilities.InsertNotification("Açıklama boş geçilemez");
                return View(model);
            }
            if (String.IsNullOrEmpty(model.LocalSource))
            {
                TempData["Notification"] = WebUtilities.InsertNotification("Kaynak bilgisayar boş geçilemez");
                return View(model);
            }
            if (String.IsNullOrEmpty(model.RemoteSource))
            {
                TempData["Notification"] = WebUtilities.InsertNotification("Hedef bilgisayar boş geçilemez");
                return View(model);
            }
            if (String.IsNullOrEmpty(model.BackupStartDate))
            {
                TempData["Notification"] = WebUtilities.InsertNotification("Başlangıç tarihi boş geçilemez");
                return View(model);
            }
            if (String.IsNullOrEmpty(model.BackupStartTime))
            {
                TempData["Notification"] = WebUtilities.InsertNotification("Başlangıç saati boş geçilemez");
                return View(model);
            }
            try
            {
                string[] dateArray = model.BackupStartDate.Split('-');
                string[] timeArray = model.BackupStartTime.Split(':');
                model.RealInsertBackupStartDate = new DateTime(Convert.ToInt32(dateArray[2]), Convert.ToInt32(dateArray[1]), Convert.ToInt32(dateArray[0]), Convert.ToInt32(timeArray[0]), Convert.ToInt32(timeArray[1]), 0);
            }
            catch
            {
                TempData["Notification"] = WebUtilities.InsertNotification("Lütfen tarih ve saat bilgisini kontrol ediniz.");
                return View(model);
            }
            model.PlanId = WebUtilities.DecryptId(WebUtility.UrlDecode(model.HashedPlanId), GetUserName(), GetOrganizationId());
            model.DeviceId = WebUtilities.DecryptId(model.DeviceHashedId, GetUserName(), GetOrganizationId());
            model.OrganizationId = GetOrganizationId();
            if (String.IsNullOrEmpty(model.HashedDevicePlanId))
            {
                int returnValue = operations.DevicePlan.InsertDevicePlan(model);

                if (returnValue > 0)
                {
                    model.Id = returnValue;
                    TriggerHangfireJob(model);
                    model.HashedDevicePlanId = WebUtilities.EncryptId(returnValue, GetUserName(), GetOrganizationId());
                    TempData["Notification"] = WebUtilities.InsertNotification("Cihaz planı başarılı bir şekilde eklenmiştir.");
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
                int realId = WebUtilities.DecryptId(WebUtility.UrlDecode(model.HashedDevicePlanId), GetUserName(), GetOrganizationId());
                if (realId > 0)
                {

                    model.CloseModal = true;
                    model.Id = realId;
                    operations.DevicePlan.UpdateDevicePlan(model, GetOrganizationId());

                    List<string> list = new List<string>();
                    list.Add(operations.DevicePlan.GetDevicePlanById(GetOrganizationId(), realId).BackgroundJobId);
                    HangfireProvider.DeleteHangFireJobs(list);
                    TriggerHangfireJob(model);
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
        public JsonResult GetParentDirectoryListing(string directoryId, string deviceId, string cloudId)
        {


            var sList = new List<object>();
            if (String.IsNullOrEmpty(cloudId))
            {
                DpOperations db = new DpOperations();
                int realDeviceId = WebUtilities.DecryptId(WebUtility.HtmlDecode(WebUtility.UrlDecode(deviceId)), GetUserName(), GetOrganizationId());

                string directoryName = db.Device.GetDirectoryById(Convert.ToInt32(directoryId), GetOrganizationId());
                HangfireProvider.SendFileListCommand(realDeviceId, GetOrganizationId(), directoryId);

                directoryId = directoryId.Replace(@"\\", @"\");

                for (int i = 0; i <= 20; i++)
                {
                    double second = db.Device.IsFileListIncoming(realDeviceId, GetOrganizationId(), directoryName);
                    if (second > 2 && second < 60 && second != 0)
                    {
                        var list = db.Device.GetFileList(realDeviceId, GetOrganizationId(), directoryName);
                        foreach (var item in list)
                        {
                            sList.Add(new { id = item.Id, text = item.Directory, parent = directoryId, children = true });
                        }
                        break;
                    }
                    System.Threading.Thread.Sleep(3000);
                }
            }
            else
            {
                int realplanId = WebUtilities.DecryptId(WebUtility.UrlDecode(cloudId), GetUserName(), GetOrganizationId());
                var planData = operations.Plan.GetPlanById(GetOrganizationId(), realplanId);
                List<FileListModel> list = new List<FileListModel>();
                if (planData.Type == Database.Enum.PlanType.GoogleDrive)
                {
                    list = GetGoogleDriveList(realplanId, directoryId, planData);
                }
                else if (planData.Type == Database.Enum.PlanType.AmazonS3)
                {
                    list = GetAmazonS3List(realplanId, directoryId, planData);
                }
                foreach (var item in list)
                {
                    sList.Add(new { id = item.CloudId, text = item.Directory, parent = directoryId, children = true });
                }
            }
            return Json(sList);
        }
        public string ReturnDeviceDiretoryById(int id)
        {
            return operations.Device.GetDirectoryById(id, GetOrganizationId());
        }
        public List<FileListModel> GetGoogleDriveList(int planId, string directory, PlatformInsertModel planData)
        {


            PlatformGoogle gg = new PlatformGoogle(_env.ContentRootPath);



            dynamic platformDetail = JObject.Parse(planData.JsonData);
            string token = "";
            if (WebUtilities.ConvertDatabaseDateTime(platformDetail.tokenExpire.ToString()) > DateTime.Now)
                token = platformDetail.accessToken;
            else
            {
                var trp = gg.AuthCodeToAccessToken("", "", platformDetail.refreshToken.ToString());

                dynamic dData = null;
                if (!String.IsNullOrEmpty(planData.JsonData))
                    dData = JObject.Parse(planData.JsonData);

                List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
                list.Add(new KeyValuePair<string, string>("accountId", planId.ToString()));
                list.Add(new KeyValuePair<string, string>("accessToken", trp.AccessToken));
                if (!String.IsNullOrEmpty(trp.RefreshToken))
                    list.Add(new KeyValuePair<string, string>("refreshToken", trp.RefreshToken));
                else if (dData != null)
                    list.Add(new KeyValuePair<string, string>("refreshToken", dData.refreshToken.ToString()));
                list.Add(new KeyValuePair<string, string>("tokenExpire", DateTime.Now.AddSeconds(trp.ExpiresInSeconds).ToString()));
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

                token = trp.AccessToken;
            }
            GoogleDriveAuthInformation inf = new GoogleDriveAuthInformation() { access_token = token };
            return gg.GetDirectoryList(directory, inf.access_token);
        }
        public List<FileListModel> GetAmazonS3List(int planId, string directory, PlatformInsertModel planData)
        {
            dynamic platformDetail = JObject.Parse(planData.JsonData);
            AmazonS3 s3 = new AmazonS3();
            return s3.GetDirectoryList(directory,platformDetail.apiAccessKey.ToString() + "-" + platformDetail.apiSecretKey.ToString() + "-" + platformDetail.region.ToString());
        }
        public JsonResult GetRootDirectoryListing(string deviceId, string cloudId)
        {
            var sList = new List<object>();
            if (String.IsNullOrEmpty(cloudId))
            {
                int realDeviceId = WebUtilities.DecryptId(WebUtility.HtmlDecode(WebUtility.UrlDecode(deviceId)), GetUserName(), GetOrganizationId());

                HangfireProvider.SendFileListCommand(realDeviceId, GetOrganizationId(), null);


                DpOperations db = new DpOperations();
                for (int i = 0; i <= 20; i++)
                {
                    double second = db.Device.IsFileListIncoming(realDeviceId, GetOrganizationId(), null);
                    if (second > 2 && second < 60 && second != 0)
                    {
                        var list = db.Device.GetFileList(realDeviceId, GetOrganizationId(), null);
                        foreach (var item in list)
                        {
                            sList.Add(new { id = item.Id, text = item.Directory, parent = "#", children = true });
                        }
                        break;
                    }
                    System.Threading.Thread.Sleep(3000);
                }
            }
            else
            {


                int realplanId = WebUtilities.DecryptId(WebUtility.UrlDecode(cloudId), GetUserName(), GetOrganizationId());
                var planData = operations.Plan.GetPlanById(GetOrganizationId(), realplanId);
                List<FileListModel> list = new List<FileListModel>();
                if (planData.Type == Database.Enum.PlanType.GoogleDrive)
                {
                    list = GetGoogleDriveList(realplanId, "root", planData);
                }
                else if (planData.Type == Database.Enum.PlanType.AmazonS3)
                {
                    list = GetAmazonS3List(realplanId, "", planData);
                }
                foreach (var item in list)
                {
                    sList.Add(new { id = item.CloudId, text = item.Directory, parent = "#", children = true });
                }
            }
            return Json(sList);
        }
        public IActionResult Download()
        {
            return View();
        }
    }
}