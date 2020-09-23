using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Upload;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CloudBackup.WinPlatform.Platform
{
    public class PlatformGoogle : IPlatform
    {

        public List<KeyValuePair<string, object>> ValueList { get; set; }
        public PlatformGoogle(DriveService service, string accessToken, string apiToken, DateTime tokenExpire)
        {
            ValueList = new List<KeyValuePair<string, object>>();
            ValueList.Add(new KeyValuePair<string, object>("service", service));
            ValueList.Add(new KeyValuePair<string, object>("accessToken", accessToken));
            ValueList.Add(new KeyValuePair<string, object>("apiToken", apiToken));
            ValueList.Add(new KeyValuePair<string, object>("tokenExpire", tokenExpire));
        }
        public string UploadFile(string fileName, string filePath, string fileType, string directoryId)
        {
            try
            {
                long totalSize = 0;
                var path = filePath;
                var filename = Path.GetFileName(path);
                var mimetype = MimeMapping.GetMimeMapping(filename);

                var metadata = new Google.Apis.Drive.v3.Data.File() { Name = filename, Parents = new List<string> { directoryId } };
                FilesResource.CreateMediaUpload request;
                using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Open))
                {
                    request = ((DriveService)ValueList.Where(f => f.Key == "service").FirstOrDefault().Value).Files.Create(
                        metadata, stream, mimetype);
                    request.Fields = "id";
                    request.OauthToken = ValueList.Where(f => f.Key == "accessToken").FirstOrDefault().Value.ToString();
                    totalSize = stream.Length;
                    request.Upload();
                }
                var response = request.ResponseBody;
                if (response != null && !String.IsNullOrEmpty(response.Id))
                {
                    return response.Id;
                    //inf.SuccessCount += 1;
                    //inf.TotalSize += totalSize; 
                }
                else
                {
                    return "error-Response Is Null";
                    //inf.ErrorCount += 1;
                    ////// Hatalı kopyalama mesajı atılacak.
                }
            }
            catch (Exception ex)
            {
                if (ex != null && ex.Message != null)
                    return "error-" + ex.Message;
                else
                    return "error-Internal Error";
                ////// Hatalı kopyalama mesajı atılacak.
            }

        }

        public string UpdateFile(string fileName, string filePath, string fileType, string directoryId, string fileId)
        {
            try
            {

                long totalSize = 0;
                var path = filePath;
                var filename = Path.GetFileName(path);
                var mimetype = MimeMapping.GetMimeMapping(filename);

                var metadata = new Google.Apis.Drive.v3.Data.File() { Name = filename };
                FilesResource.UpdateMediaUpload request;
                using (var stream = new System.IO.FileStream(path, System.IO.FileMode.Open))
                {
                    request = ((DriveService)ValueList.Where(f => f.Key == "service").FirstOrDefault().Value).Files.Update(
                        metadata, fileId, stream, mimetype);
                    request.Fields = "id";
                    request.OauthToken = ValueList.Where(f => f.Key == "accessToken").FirstOrDefault().Value.ToString();
                    totalSize = stream.Length;
                    request.ProgressChanged += Upload_ProgressChanged;
                    request.ResponseReceived += Upload_ResponseReceived;
                    request.Upload();
                }
                var response = request.ResponseBody;
                if (response != null && !String.IsNullOrEmpty(response.Id))
                {
                    return response.Id;
                    //inf.SuccessCount += 1;
                    //inf.TotalSize += totalSize; 
                }
                else
                {
                    return "error-Response Is Null";
                    //inf.ErrorCount += 1;
                    ////// Hatalı kopyalama mesajı atılacak.
                }
            }
            catch (Exception ex)
            {
                if (ex != null && ex.Message != null)
                    return "error-" + ex.Message;
                else
                    return "error-Internal Error";
                ////// Hatalı kopyalama mesajı atılacak.
            }

        }

        private void Upload_ResponseReceived(Google.Apis.Drive.v3.Data.File obj)
        {
            //throw new NotImplementedException();
        }

        private void Upload_ProgressChanged(IUploadProgress obj)
        {
            //throw new NotImplementedException();
        }

        public void DeleteFile(string fileId)
        {
            try
            {


                var request = ((DriveService)ValueList.Where(f => f.Key == "service").FirstOrDefault().Value).Files.Delete(fileId);
                request.OauthToken = ValueList.Where(f => f.Key == "accessToken").FirstOrDefault().Value.ToString();
                var file = request.Execute();

            }
            catch (Exception ex)
            {

            }
        }

        public string CreateDirectory(string folderName, string parent)
        {
            try
            {


                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = folderName,
                    MimeType = "application/vnd.google-apps.folder",
                    Parents = new List<string> { parent }
                };
                var request = ((DriveService)ValueList.Where(f => f.Key == "service").FirstOrDefault().Value).Files.Create(fileMetadata);
                request.OauthToken = ValueList.Where(f => f.Key == "accessToken").FirstOrDefault().Value.ToString();
                request.Fields = "id";
                var file = request.Execute();

                if (file != null && !String.IsNullOrEmpty(file.Id))
                {
                    return file.Id;
                    //inf.SuccessCount += 1;
                    //inf.TotalSize += totalSize; 
                }
                else
                {
                    return "error-Response Is Null";
                    //inf.ErrorCount += 1;
                    ////// Hatalı kopyalama mesajı atılacak.
                }
            }
            catch (Exception ex)
            {
                if (ex != null && ex.Message != null)
                    return "error-" + ex.Message;
                else
                    return "error-Internal Error";
            }
        }



        public bool CheckAccessTokenExpire()
        {
            if (DateTime.Now >= Convert.ToDateTime(ValueList.Where(f => f.Key == "tokenExpire").FirstOrDefault().Value.ToString()))
            {
                return true;
            }
            else
                return false;
        }

        public void UpdateAccessToken(string accessToken, DateTime expireDate)
        {
            ValueList[ValueList.FindIndex(s => s.Key == "accessToken")] = new KeyValuePair<string, object>("accessToken", accessToken);
            ValueList[ValueList.FindIndex(s => s.Key == "tokenExpire")] = new KeyValuePair<string, object>("tokenExpire", expireDate);
        }
    }
}
