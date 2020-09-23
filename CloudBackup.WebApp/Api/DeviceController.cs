using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CloudBackup.Database.Entity;
using CloudBackup.Database.Operations;
using CloudBackup.Database.ViewModel.CreateModel;
using CloudBackup.Database.ViewModel.DeviceApiModel;
using CloudBackup.WebApp.Core;
using CloudBackup.WebPlatforms.GoogleDrive;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace CloudBackup.WebApp.Api
{
    [Route("api/device")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        protected IHostingEnvironment _env { get; set; }
        public DeviceController(IHostingEnvironment env)
        {
            _env = env;
        }

        [HttpPost]
        [Route("check")]
        public HttpResponseMessage CheckOnlineDevice([FromBody] CheckModel data)
        {
            DpOperations db = new DpOperations();
            var device = db.Device.GetApiAccessAndSecretKeyDevice(data.apiAccessKey, data.apiSecretKey);

            if (device != null)
            {
                if (String.IsNullOrEmpty(device.CpuId) || String.IsNullOrEmpty(device.MacAddress) || String.IsNullOrEmpty(device.DiskSeriNo))
                {
                    data.deviceId = device.Id;
                    db.Device.UpdateDeviceInformation(data);
                    DevicePlanLog lg = new DevicePlanLog()
                    {
                        CreatedDate = DateTime.Now,
                        Description = "Cihaz ekleme başarılı bir şekilde gerçekleşmiştir. Cihaz aktif",
                        DeviceId = device.Id,
                        OrganizationId = device.OrganizationId
                    };
                    db.Log.InsertLog(lg);

                    HangfireProvider.ScheduleRecurringCheckOnline(DateTime.Now, device.Id, "0 */3 * * *", device.OrganizationId);
                    //HangfireProvider.ScheduleRecurringFileList(DateTime.Now, device.Id, "0 */12 * * *", device.OrganizationId);
                    //HangfireProvider.InsertJobOneTimeFileList(DateTime.Now, device.Id, device.OrganizationId);
                    HangfireProvider.InsertJobOneTimeCheckOnline(DateTime.Now, device.Id, device.OrganizationId);

                }
                else
                {
                    if (data.updateData)
                    {
                        data.deviceId = device.Id;
                        db.Device.UpdateDeviceInformation(data);
                        DevicePlanLog lg = new DevicePlanLog()
                        {
                            CreatedDate = DateTime.Now,
                            Description = "Cihaz ekleme başarılı bir şekilde gerçekleşmiştir. Cihaz aktif",
                            DeviceId = device.Id,
                            OrganizationId = device.OrganizationId
                        };
                        db.Log.InsertLog(lg);
                    }
                    else
                    {
                        DevicePlanLog lg = new DevicePlanLog()
                        {
                            CreatedDate = DateTime.Now,
                            Description = "Cihaz kontrolü başarılı bir şekilde gerçekleşmiştir. Cihaz aktif",
                            DeviceId = device.Id,
                            OrganizationId = device.OrganizationId
                        };
                        db.Log.InsertLog(lg);
                    }
                }
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            }
            else
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }


        }

        [Route("filelist")]
        public IActionResult GetDeviceFileList([FromBody] DeviceFileListModel data)
        {
            DpOperations db = new DpOperations();
            var device = db.Device.GetApiAccessAndSecretKeyDevice(data.apiAccessKey, data.apiSecretKey);

            if (device != null)
            {

                List<DeviceFileList> listFile = new List<DeviceFileList>();

                foreach (var item in data.directoryListing)
                {
                    string[] directoryArray = Regex.Split(item, @"\\");
                    DeviceFileList m = new DeviceFileList();
                    m.CreatedDate = DateTime.Now;
                    m.DeviceId = device.Id;
                    m.OrganizationId = device.OrganizationId;
                    if (!String.IsNullOrEmpty(data.subDirectory))
                        m.SubDirectory = data.subDirectory;

                    m.Directory = item;
                    listFile.Add(m);
                }
                db.Device.InsertFileList(listFile, data.subDirectory);
                return Ok();

            }
            else
                return Unauthorized();

        }
        [Route("deviceplanid/{deviceplanid}/finish")]
        [HttpPost]
        public IActionResult FinishDevicePlan([FromBody] BackupLogModel checkModel, int deviceplanid)
        {
            DpOperations db = new DpOperations();
            var device = db.Device.GetApiAccessAndSecretKeyDevice(checkModel.apiAccessKey, checkModel.apiSecretKey);

            if (device != null)
            {
                decimal totalSize = 0;
                decimal.TryParse(checkModel.TotalSize.ToString(),out totalSize);
                db.Log.InsertLog(new DevicePlanLog
                {
                    CreatedDate = DateTime.Now,
                    Description = String.Format("{0} tane dosya eklendi. {6} tane dosya güncellendi. {1} tane klasör eklendi. {2} tane dosya silindi. {3} tane işlem yapıldı. {4} tane başarısız işlem. Toplamda {5} MB yedeklendi.", checkModel.CreateFileCount, checkModel.CreateDirectoryCount, checkModel.DeletedCount, checkModel.ProccessCount, checkModel.FailedCount, totalSize.ToString("##.##"), checkModel.UpdatedCount),
                    DeviceId = device.Id,
                    OrganizationId = device.OrganizationId,
                    DevicePlanId = checkModel.DevicePlanId
                });

                db.BackupLog.InsertBackupLog(new BackupLog
                {
                    CreateDirectoryCount = checkModel.CreateDirectoryCount,
                    CreateFileCount = checkModel.CreateFileCount,
                    DeletedCount = checkModel.DeletedCount,
                    DevicePlanId = checkModel.DevicePlanId,
                    FailedCount = checkModel.FailedCount,
                    OrganizationId = device.OrganizationId,
                    ProccessCount = checkModel.ProccessCount,
                    TotalSize = Convert.ToDouble(totalSize),
                    UpdatedCount = checkModel.UpdatedCount
                });
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }
        [Route("deviceplan/{devicePlanId}/resettoken")]
        [HttpPost]
        public IActionResult ResetToken([FromBody] CheckModel checkModel, int devicePlanId)
        {
            DpOperations db = new DpOperations();
            var device = db.Device.GetApiAccessAndSecretKeyDevice(checkModel.apiAccessKey, checkModel.apiSecretKey);

            if (device != null)
            {

                var job = db.DevicePlan.GetDevicePlanById(device.OrganizationId, devicePlanId);


                var planData = db.Plan.GetPlanById(device.OrganizationId, job.PlanId);

                if (planData.Type == Database.Enum.PlanType.GoogleDrive)
                {
                    TokenResponse trp = ResetGoogleDriveToken(planData, job.PlanId, db, device.OrganizationId);
                    return Ok(trp);
                }
                else
                    return NotFound();

            }
            else
                return Unauthorized();
        }
        [Route("decrpyt")]
        [HttpPost]
        public IActionResult DecryptMessage([FromBody] CheckModel data, string message)
        {
            dynamic config = WebUtilities.GetConfig(_env.ContentRootPath);



            DpOperations db = new DpOperations();
            var device = db.Device.GetApiAccessAndSecretKeyDevice(data.apiAccessKey, data.apiSecretKey);

            if (device != null)
            {

                var response = new HttpResponseMessage(HttpStatusCode.OK);
                string returnValue = WebUtilities.DecryptDeviceInformation(message);
                if (returnValue.StartsWith("F|"))
                {
                    string[] arrayString = returnValue.Split('|');
                    if (!String.IsNullOrEmpty(arrayString[arrayString.Length - 1]))
                    {
                        string returnContent = "";
                        arrayString[arrayString.Length - 1] = db.Device.GetDirectoryById(Convert.ToInt32(returnValue.Split('|')[returnValue.Split('|').Length - 1]), device.OrganizationId);
                        for (int i = 0; i < arrayString.Length; i++)
                        {
                            returnContent += arrayString[i] + "|";
                        }
                        return Ok(returnContent.Substring(0, returnContent.Length - 1));
                    }
                    else
                        return Ok(WebUtilities.DecryptDeviceInformation(message));
                }
                else if (returnValue.StartsWith("B|"))
                {
                    var model = db.DevicePlan.GetDevicePlanById(device.OrganizationId, Convert.ToInt32(returnValue.Split('|')[1]));
                    var plan = db.Plan.GetPlanById(device.OrganizationId, model.PlanId);

                    if (plan.Type == Database.Enum.PlanType.GoogleDrive)
                    {
                        return Ok(ReturnGoogleJobModel(plan, model, db, config.GoogleApiSettings.ApiKey.ToString()));
                    }
                    else if (plan.Type == Database.Enum.PlanType.AmazonS3)
                    {
                        BackupJobModelAmazonS3 s3 = new BackupJobModelAmazonS3();
                        dynamic platformDetail = JObject.Parse(plan.JsonData);
                        s3.apiAccessKey = "'" + platformDetail.apiAccessKey + "'";
                        s3.apiSecretKey ="'" + platformDetail.apiSecretKey + "'";
                        s3.region ="'" + platformDetail.region + "'";
                        s3.DevicePlanId = "'" + model.Id.ToString() + "'";
                        s3.LocalDirectory = "'" + model.LocalSource + "'";
                        s3.PlanType = Database.Enum.PlanType.AmazonS3;
                        s3.RemoteDirectory ="'" + model.RemoteSource + "'";
                        return Ok(s3);
                    }
                    else
                        return NotFound();
                }
                else
                    return Ok(WebUtilities.DecryptDeviceInformation(message));


            }
            else
                return Unauthorized();

        }
        [Route("log")]
        [HttpPost]
        public IActionResult InsertLog([FromBody] LogDeviceModel data)
        {
            DpOperations db = new DpOperations();
            var device = db.Device.GetApiAccessAndSecretKeyDevice(data.apiAccessKey, data.apiSecretKey);

            if (device != null && data.errorList != null)
            {
                foreach (var item in data.errorList)
                {
                    db.Log.InsertLog(new DevicePlanLog
                    {
                        CreatedDate = DateTime.Now,
                        Description = item,
                        DeviceId = device.Id,
                        DevicePlanId = data.devicePlanId,
                        OrganizationId = device.OrganizationId
                    });
                }
            }
            return Ok();
        }

        private BackupJobModelGoogle ReturnGoogleJobModel(PlatformInsertModel plan, DevicePlanInsertModel model, DpOperations db, string googleApiCode)
        {
            dynamic platformDetail = JObject.Parse(plan.JsonData);
            DateTime? expireToken = null;

            if (!String.IsNullOrEmpty(platformDetail.tokenExpire.ToString()))
            {
                expireToken = Convert.ToDateTime(platformDetail.tokenExpire.ToString());
            }

            BackupJobModelGoogle returnModel = new BackupJobModelGoogle();
            if (expireToken.HasValue && expireToken.Value > DateTime.Now)

            {
                returnModel.PlanType = plan.Type;
                returnModel.LocalDirectory = "'" + model.LocalSource + "'";
                returnModel.RemoteDirectory = "'" + model.RemoteSource + "'";
                returnModel.DevicePlanId = "'" + model.Id.ToString() + "'";
                returnModel.GoogleAccessToken = "'" + platformDetail.accessToken + "'";
                returnModel.GoogleApiCode = "'" + googleApiCode + "'";
                returnModel.GoogleTokenExpired = "'" + platformDetail.tokenExpire.ToString() + "'";
            }
            else
            {
                //PlatformGoogle gg = new PlatformGoogle();
                TokenResponse tr = ResetGoogleDriveToken(plan, plan.Id, db, plan.OrganizationId);

                returnModel.PlanType = plan.Type;
                returnModel.LocalDirectory = "'" + model.LocalSource + "'";
                returnModel.RemoteDirectory = "'" + model.RemoteSource + "'";
                returnModel.DevicePlanId = "'" + model.Id.ToString() + "'";
                returnModel.GoogleAccessToken = "'" + platformDetail.accessToken + "'";
                returnModel.GoogleApiCode = "'" + googleApiCode + "'";
                returnModel.GoogleTokenExpired = "'" + platformDetail.tokenExpire.ToString() + "'";
            }
            return returnModel;

        }

        private TokenResponse ResetGoogleDriveToken(PlatformInsertModel planData, int planId, DpOperations db, int organizationId)
        {
            PlatformGoogle pg = new PlatformGoogle(_env.ContentRootPath);
            dynamic dData = null;
            if (!String.IsNullOrEmpty(planData.JsonData))
                dData = JObject.Parse(planData.JsonData);

            TokenResponse trp = pg.AuthCodeToAccessToken("", "", dData.refreshToken.ToString());

            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("accountId", planId.ToString()));
            list.Add(new KeyValuePair<string, string>("accessToken", trp.AccessToken));
            if (!String.IsNullOrEmpty(trp.RefreshToken))
                list.Add(new KeyValuePair<string, string>("refreshToken", trp.RefreshToken));
            else if (dData != null)
                list.Add(new KeyValuePair<string, string>("refreshToken", dData.refreshToken.ToString()));
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
            db.Plan.UpdatePlan(planData, organizationId);

            return trp;
        }
    }
}