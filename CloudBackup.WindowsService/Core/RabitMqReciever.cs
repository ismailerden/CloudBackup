using CloudBackup.Database.ViewModel.DeviceApiModel;
using CloudBackup.WindowsService.Model;
using CloudBackup.WinPlatform.Platform;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client.Events;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
//using RabbitMQ.Client.Events;

namespace CloudBackup.WindowsService
{
    public class RabitMqReciever
    {
        private int delayTime = 0;
        RestClient client;
        public RabitMqReciever()
        {
            //  client = new RestClient("https://api.ebackup.cloud");
            client = new RestClient("https://localhost:44328");
        }
        public void ReadQue(string apiAccessKey, string ApiSecretKey)
        {

            if (!String.IsNullOrEmpty(apiAccessKey) && !String.IsNullOrEmpty(ApiSecretKey))
            {
                WriteError("Que Readed :" + Common.SecurityKey());
                Consumer consumer;
                consumer = new Consumer("178.128.207.112", Common.SecurityKey());

                //listen for message events
                consumer.onMessageReceived += Consumer_onMessageReceived;

                //start consuming
                consumer.StartConsuming();
            }
        }

        public void WriteError(string error)
        {
            StreamWriter Dosya = System.IO.File.AppendText(AppDomain.CurrentDomain.BaseDirectory + "log.txt");
            Dosya.WriteLine(error + "  Tarih: " + DateTime.Now.ToString());
            Dosya.Close();
        }
        private void Consumer_onMessageReceived(byte[] message)
        {

            string returnText = System.Text.Encoding.UTF8.GetString(message);


            //CheckCpuSpeed();

            string[] deviceInfo = Common.SecurityKey().Split('|');
            CheckModel model = new CheckModel();
            var result = Common.ApiAndSecretKey();
            model.apiAccessKey = result.Key;
            model.apiSecretKey = result.Value;
            model.cpuId = deviceInfo[0];
            model.diskVolume = deviceInfo[1];

            model.macAddress = deviceInfo[2];






            IRestRequest request = new RestRequest("api/device/decrpyt?message={message}", Method.POST);
            request.RequestFormat = RestSharp.DataFormat.Json;
            request.AddParameter("message", returnText, ParameterType.UrlSegment);
            request.AddBody(model);
            TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();
            RestRequestAsyncHandle handle = client.ExecuteAsync(request, r => { taskCompletion.SetResult(r); });
            IRestResponse response = taskCompletion.Task.Result;
            string messageBody = response.Content;
            messageBody = messageBody.Replace("\"", "");
            WriteError(messageBody);

            if (messageBody.StartsWith("C|"))
            {
                Thread thFile = new Thread(SendCheckOnline);
                thFile.Start();
            }
            else if (messageBody.StartsWith("F|"))
            {
                Thread thFile = new Thread(() => SendFileList(messageBody.Split('|')[2]));
                thFile.Start();
            }
            else if (!messageBody.StartsWith("C|") && !messageBody.StartsWith("F|"))
            {
                Thread thBackup = new Thread(() => SendFile(messageBody));
                thBackup.Start();
            }


        }
        private TokenResponse ResetGoogleToken(int devicePlanId)
        {


            string[] deviceInfo = Common.SecurityKey().Split('|');
            CheckModel model = new CheckModel();
            var result = Common.ApiAndSecretKey();
            model.apiAccessKey = result.Key;
            model.apiSecretKey = result.Value;
            model.cpuId = deviceInfo[0];
            model.diskVolume = deviceInfo[1];
            model.macAddress = deviceInfo[2];


            IRestRequest request = new RestRequest("api/device/deviceplan/{devicePlanId}/resettoken", Method.POST);
            request.RequestFormat = RestSharp.DataFormat.Json;
            request.AddParameter("devicePlanId", devicePlanId.ToString(), ParameterType.UrlSegment);
            request.AddBody(model);
            TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();
            RestRequestAsyncHandle handle = client.ExecuteAsync(request, r => { taskCompletion.SetResult(r); });
            IRestResponse response = taskCompletion.Task.Result;
            return JsonConvert.DeserializeObject<TokenResponse>(response.Content);
        }
        public void SendFile(string model)
        {
            BackupLog bc = new BackupLog();
            string localDirectory = "";
            string remoteDirectory = "";
            string devicePlanId = "";
            dynamic platformDetail = JObject.Parse(model);
            localDirectory = platformDetail.localDirectory;
            remoteDirectory = platformDetail.remoteDirectory;
            devicePlanId = platformDetail.devicePlanId;
            bc.DevicePlanId = Convert.ToInt32(devicePlanId);
            bc.CreateDirectoryCount = 0;
            bc.CreateFileCount = 0;
            bc.FailedCount = 0;
            bc.ProccessCount = 0;
            bc.TotalSize = 0;
            bc.DeletedCount = 0;
            bc.UpdatedCount = 0;
            DbOperations db = new DbOperations();

            IPlatform platf = null;
            if (platformDetail.planType.ToString() == "1")
            {


                DriveService s = new Google.Apis.Drive.v3.DriveService();
                var services = new DriveService(new BaseClientService.Initializer()
                {
                    ApiKey = platformDetail.googleApiCode,  // from https://console.developers.google.com (Public API access)
                    ApplicationName = "ECloud Backup",

                });

                platf = new PlatformGoogle(s, platformDetail.googleAccessToken.ToString(), platformDetail.googleApiCode.ToString(), Common.ConvertDatabaseDateTime(platformDetail.googleTokenExpired.ToString()));
            }
            else if (platformDetail.planType.ToString() == "2")
            {
                platf = new AmazonS3(platformDetail.apiAccessKey.ToString(), platformDetail.apiSecretKey.ToString(), platformDetail.region.ToString());
            }

            DateTime dtStartBackup = DateTime.Now;

            SendBackupDirectory(localDirectory, remoteDirectory, db, Convert.ToInt32(devicePlanId), platf, bc);

            var list = db.GetErrorFileList(Convert.ToInt32(devicePlanId), dtStartBackup);
            List<string> errorList = new List<string>();
            foreach (var k in list)
            {
                errorList.Add(k.FullName + " - " + k.ErrorMessage);
            }

            LogDeviceModel mdl = new LogDeviceModel
            {
                apiAccessKey = bc.apiAccessKey,
                apiSecretKey = bc.apiSecretKey,
                cpuId = bc.cpuId,
                deviceId = bc.deviceId,
                devicePlanId = Convert.ToInt32(devicePlanId),
                diskVolume = bc.diskVolume,
                errorList = errorList,
                macAddress = bc.macAddress
            };


            IRestRequest request = new RestRequest("api/device/deviceplanid/{deviceplanid}/finish", Method.POST);
            request.RequestFormat = RestSharp.DataFormat.Json;
            request.AddParameter("deviceplanid", devicePlanId, ParameterType.UrlSegment);
            request.AddBody(bc);
            TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();
            RestRequestAsyncHandle handle = client.ExecuteAsync(request, r => { taskCompletion.SetResult(r); });
            IRestResponse response = taskCompletion.Task.Result;



            IRestRequest request1 = new RestRequest("api/device/log", Method.POST);
            request1.RequestFormat = RestSharp.DataFormat.Json;
            request1.AddBody(mdl);
            TaskCompletionSource<IRestResponse> taskCompletion1 = new TaskCompletionSource<IRestResponse>();
            RestRequestAsyncHandle handle1 = client.ExecuteAsync(request1, r => { taskCompletion1.SetResult(r); });
            IRestResponse response1 = taskCompletion1.Task.Result;

        }
        private double GetFileSize(FileInfo f)
        {
            string[] sizes = { "B", "KB", "MB" };
            double len = f.Length;
            for (int i = 0; i <= 1; i++)
            {
                len = len / 1024;
            }

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            return len;
        }
        public void SendBackupDirectory(string directory, string remoteDirectory, DbOperations db, int devicePlanId, IPlatform platform, BackupLog bc)
        {
            string[] deviceInfo = Common.SecurityKey().Split('|');
            BackupJobModel model = new BackupJobModel();
            var result = Common.ApiAndSecretKey();
            model.apiAccessKey = result.Key;
            model.apiSecretKey = result.Value;
            model.cpuId = deviceInfo[0];
            model.diskVolume = deviceInfo[1];
            model.macAddress = deviceInfo[2];

            bc.apiAccessKey = model.apiAccessKey;
            bc.apiSecretKey = model.apiSecretKey;
            bc.cpuId = model.cpuId;
            bc.diskVolume = model.diskVolume;
            bc.macAddress = model.macAddress;

            List<CloudFile> cfList = db.GetFileList(devicePlanId, directory);
            DirectoryInfo d = new DirectoryInfo(directory);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles(); //Getting Text files
            string str = "";

            foreach (FileInfo file in Files)
            {
                CloudFile currentFile = cfList.Where(f => f.FullName == file.FullName && f.ResultStatus == Enum.ResultStatus.Success).FirstOrDefault();
                if (currentFile != null)
                {
                    if (file.LastWriteTime > currentFile.ProccessDate.Value && !String.IsNullOrEmpty(currentFile.CloudId))
                    {
                        if (platform.CheckAccessTokenExpire())
                        {
                            TokenResponse resp = ResetGoogleToken(devicePlanId);
                            platform.UpdateAccessToken(resp.AccessToken, DateTime.Now.AddSeconds(resp.ExpiresInSeconds.Value));
                        }
                        string fileId = platform.UpdateFile(file.Name, file.FullName, file.GetType().ToString(), remoteDirectory, currentFile.CloudId);// UpdateFile(file.Name, file.FullName, file.GetType().ToString(), accessToken, s, remoteDirectory, currentFile.CloudId);

                        if (!String.IsNullOrEmpty(fileId) && !fileId.StartsWith("error-"))
                        {
                            currentFile.CloudId = fileId;
                            bc.ProccessCount++;
                            bc.UpdatedCount++;
                            bc.TotalSize += GetFileSize(file);
                        }
                        if (!String.IsNullOrEmpty(fileId) && fileId.StartsWith("error-"))
                        {
                            currentFile.ErrorMessage = fileId.Replace("error-", "");
                            bc.FailedCount++;
                            bc.ProccessCount++;
                        }
                        currentFile.Length = GetFileSize(file);
                        currentFile.ProccessDate = DateTime.Now;
                        if (!String.IsNullOrEmpty(fileId) && !fileId.StartsWith("error-"))
                            currentFile.ResultStatus = Enum.ResultStatus.Success;
                        else
                            currentFile.ResultStatus = Enum.ResultStatus.Error;
                        db.UpdateData(currentFile);
                    }
                }
                else
                {
                    if (platform.CheckAccessTokenExpire())
                    {
                        TokenResponse resp = ResetGoogleToken(devicePlanId);
                        platform.UpdateAccessToken(resp.AccessToken, DateTime.Now.AddSeconds(resp.ExpiresInSeconds.Value));
                        bc.ProccessCount++;
                    }
                    string fileId = platform.UploadFile(file.Name, file.FullName, file.GetType().ToString(), remoteDirectory);

                    CloudFile ff = new CloudFile();
                    if (!String.IsNullOrEmpty(fileId) && !fileId.StartsWith("error-"))
                        ff.CloudId = fileId;
                    ff.DevicePlanId = devicePlanId;
                    if (!String.IsNullOrEmpty(fileId) && fileId.StartsWith("error-"))
                        ff.ErrorMessage = fileId.Replace("error-", "");
                    ff.Length = GetFileSize(file);
                    ff.FullName = file.FullName;
                    ff.ProccessDate = DateTime.Now;
                    if (!String.IsNullOrEmpty(fileId) && !fileId.StartsWith("error-"))
                    {
                        ff.ResultStatus = Enum.ResultStatus.Success;
                        bc.ProccessCount++;
                        bc.CreateFileCount++;
                        bc.TotalSize += GetFileSize(file);
                    }
                    else
                    {
                        ff.ResultStatus = Enum.ResultStatus.Error;
                        bc.FailedCount++;
                        bc.ProccessCount++;
                    }
                    ff.SubDirectory = file.DirectoryName;
                    ff.Type = Enum.FileType.File;
                    db.InsertData(ff);
                }
            }

            List<string> fileFullPaths = Files.Select(f => f.FullName).ToList();

            foreach (var deleteFile in cfList.Where(f => !fileFullPaths.Contains(f.FullName) && f.Type == Enum.FileType.File && f.ResultStatus == Enum.ResultStatus.Success))
            {
                if (!String.IsNullOrEmpty(deleteFile.CloudId))
                {
                    if (platform.CheckAccessTokenExpire())
                    {
                        bc.ProccessCount++;
                        TokenResponse resp = ResetGoogleToken(devicePlanId);
                        platform.UpdateAccessToken(resp.AccessToken, DateTime.Now.AddSeconds(resp.ExpiresInSeconds.Value));
                    }
                    
                    platform.DeleteFile(deleteFile.CloudId);
                    bc.ProccessCount++;
                    bc.DeletedCount++;
                    deleteFile.ProccessDate = DateTime.Now;
                    db.DeleteData(deleteFile);
                }
            }

            foreach (var item in Directory.GetDirectories(directory))
            {

                CloudFile currentFile = cfList.Where(f => f.SubDirectory + @"\" + f.FullName == item && f.ResultStatus == Enum.ResultStatus.Success).FirstOrDefault();
                if (currentFile != null && currentFile.SubDirectory + @"\" + currentFile.FullName == item)
                {
                    SendBackupDirectory(item, currentFile.CloudId, db, devicePlanId, platform, bc);
                }
                else
                {
                    if (platform.CheckAccessTokenExpire())
                    {
                        TokenResponse resp = ResetGoogleToken(devicePlanId);
                        platform.UpdateAccessToken(resp.AccessToken, DateTime.Now.AddSeconds(resp.ExpiresInSeconds.Value));
                        bc.ProccessCount++;
                    }
                    DirectoryInfo df = new DirectoryInfo(item);
                    string directoryId = platform.CreateDirectory(df.Name, remoteDirectory);

                    CloudFile ff = new CloudFile();
                    if (!String.IsNullOrEmpty(directoryId) && !directoryId.StartsWith("error-"))
                        ff.CloudId = directoryId;
                    ff.DevicePlanId = devicePlanId;
                    if (!String.IsNullOrEmpty(directoryId) && directoryId.StartsWith("error-"))
                        ff.ErrorMessage = directoryId.Replace("error-", "");

                    ff.FullName = df.Name;
                    ff.ProccessDate = DateTime.Now;
                    if (!String.IsNullOrEmpty(directoryId) && !directoryId.StartsWith("error-"))
                    { 
                        ff.ResultStatus = Enum.ResultStatus.Success;
                        bc.CreateDirectoryCount++;
                        bc.ProccessCount++;
                    }
                    else
                    { 
                        ff.ResultStatus = Enum.ResultStatus.Error;
                        bc.FailedCount++;
                        bc.ProccessCount++;
                    }
                    ff.SubDirectory = directory;
                    ff.Type = Enum.FileType.Directory;
                    db.InsertData(ff);
                    SendBackupDirectory(item, directoryId, db, devicePlanId, platform,bc);
                }
            }
           
        }





        private void CheckCpuSpeed()
        {

            var searcher = new ManagementObjectSearcher(
            "select MaxClockSpeed from Win32_Processor");
            foreach (var item in searcher.Get())
            {
                var clockSpeed = (uint)item["MaxClockSpeed"];
                Console.WriteLine("Max Clock Speed : " + clockSpeed);
            }


            Stopwatch watch = new Stopwatch();
            Process p = Process.GetCurrentProcess();
            double startUserProcessorTm = p.UserProcessorTime.TotalMilliseconds;


            int matrix_cardination = 100;

            int[,] matrix_a = new int[matrix_cardination, matrix_cardination];
            int[,] matrix_b = new int[matrix_cardination, matrix_cardination];
            int[,] matrix_c = new int[matrix_cardination, matrix_cardination];

            for (int i = 0; i < matrix_cardination; i++)
            {
                for (int j = 0; j < matrix_cardination; j++)
                {
                    matrix_a[i, j] = j + 1;
                    matrix_b[i, j] = j + 1;
                    matrix_c[i, j] = 0;
                }
            }

            for (int i = 0; i < matrix_cardination; i++)
            {
                for (int j = 0; j < matrix_cardination; j++)
                {
                    for (int k = 0; k < matrix_cardination; k++)
                    {
                        matrix_c[i, j] = matrix_c[i, j] + matrix_a[j, k] * matrix_b[k, j];
                    }
                }
            }

            double endUserProcessorTm = p.UserProcessorTime.TotalMilliseconds;
            watch.Stop();

            delayTime = Convert.ToInt32(endUserProcessorTm - startUserProcessorTm);

        }


        private void SendFileList(string directory)
        {

            WriteError("StartFileList");

            List<string> directoryList = new List<string>();

            if (String.IsNullOrEmpty(directory))
            {
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                foreach (DriveInfo d in allDrives)
                {
                    directoryList.Add(d.Name);
                }
            }
            else
            {
                directory = directory.Replace(@"\\", @"\");
                foreach (var item in Directory.GetDirectories(directory))
                    directoryList.Add(item);
            }

            string[] deviceInfo = Common.SecurityKey().Split('|');
            CheckModel model = new CheckModel();
            var result = Common.ApiAndSecretKey();
            model.apiAccessKey = result.Key;
            model.apiSecretKey = result.Value;
            model.cpuId = deviceInfo[0];
            model.diskVolume = deviceInfo[1];
            model.macAddress = deviceInfo[2];
            //if (!String.IsNullOrEmpty(directory))
            model.subDirectory = directory;
            model.directoryListing = directoryList;


            WriteError("EndFileList Count : " + directoryList.Count);



            IRestRequest request = new RestRequest("api/device/filelist", Method.POST);
            request.RequestFormat = RestSharp.DataFormat.Json;
            request.AddBody(model);
            TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();
            RestRequestAsyncHandle handle = client.ExecuteAsync(request, r => { taskCompletion.SetResult(r); });
            IRestResponse response = taskCompletion.Task.Result;




        }



        private void SendCheckOnline()
        {

            string[] deviceInfo = Common.SecurityKey().Split('|');
            CheckModel model = new CheckModel();
            var result = Common.ApiAndSecretKey();
            model.apiAccessKey = result.Key;
            model.apiSecretKey = result.Value;
            model.cpuId = deviceInfo[0];
            model.diskVolume = deviceInfo[1];
            model.macAddress = deviceInfo[2];



            IRestRequest request = new RestRequest("api/device/check", Method.POST);
            request.RequestFormat = RestSharp.DataFormat.Json;
            request.AddBody(model);
            TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();
            RestRequestAsyncHandle handle = client.ExecuteAsync(request, r => { taskCompletion.SetResult(r); });
            IRestResponse response = taskCompletion.Task.Result;
        }
    }
}
