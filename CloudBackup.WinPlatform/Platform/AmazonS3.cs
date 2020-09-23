using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CloudBackup.WinPlatform.Platform
{
    public class AmazonS3 : IPlatform
    {
        private AmazonS3Client _amazonClient;

        public AmazonS3(string apiAccessKey, string apiSecretKey, string region)
        {
            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.USWest1, // MUST set this before setting ServiceURL and it should match the `MINIO_REGION` enviroment variable.
                ForcePathStyle = true // MUST be true to work correctly with Minio server
            };
            if (region == "USWest1")
                config.RegionEndpoint = RegionEndpoint.USWest1;
            else if (region == "USWest2")
                config.RegionEndpoint = RegionEndpoint.USWest2;
            else if (region == "APNortheast1")
                config.RegionEndpoint = RegionEndpoint.APNortheast1;
            else if (region == "APNortheast2")
                config.RegionEndpoint = RegionEndpoint.APNortheast2;
            else if (region == "APSouth1")
                config.RegionEndpoint = RegionEndpoint.APSouth1;
            else if (region == "APSoutheast1")
                config.RegionEndpoint = RegionEndpoint.APSoutheast1;
            else if (region == "APSoutheast2")
                config.RegionEndpoint = RegionEndpoint.APSoutheast2;
            else if (region == "CACentral1")
                config.RegionEndpoint = RegionEndpoint.CACentral1;
            else if (region == "CNNorth1")
                config.RegionEndpoint = RegionEndpoint.CNNorth1;
            else if (region == "CNNorthWest1")
                config.RegionEndpoint = RegionEndpoint.CNNorthWest1;
            else if (region == "EUCentral1")
                config.RegionEndpoint = RegionEndpoint.EUCentral1;
            else if (region == "EUWest2")
                config.RegionEndpoint = RegionEndpoint.EUWest2;
            else if (region == "EUWest3")
                config.RegionEndpoint = RegionEndpoint.EUWest3;
            else if (region == "SAEast1")
                config.RegionEndpoint = RegionEndpoint.SAEast1;
            else if (region == "USEast1")
                config.RegionEndpoint = RegionEndpoint.USEast1;
            else if (region == "USEast2")
                config.RegionEndpoint = RegionEndpoint.USEast2;
            else if (region == "EUWest1")
                config.RegionEndpoint = RegionEndpoint.EUWest1;
            else if (region == "USGovCloudWest1")
                config.RegionEndpoint = RegionEndpoint.USGovCloudWest1;


            _amazonClient = new AmazonS3Client(apiAccessKey, apiSecretKey, config);
        }

        public List<KeyValuePair<string, object>> ValueList { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool CheckAccessTokenExpire()
        {
            return false;
        }

        public string CreateDirectory(string folderName, string parent)
        {
            PutObjectRequest request = new PutObjectRequest() { BucketName = parent.Split('/')[0] };
            if (parent.IndexOf("/") > 0)
                request.Key = parent.Substring(parent.IndexOf('/') + 1, parent.Length - (parent.IndexOf('/') + 1)) + "/" + folderName;
            else
                request.Key = folderName;
            request.InputStream = new MemoryStream();
            _amazonClient.PutObject(request);
            return parent + "/" + folderName;
        }

        public void DeleteFile(string fileId)
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = fileId.Split('/')[0],
                Key = fileId.Substring(fileId.IndexOf('/') + 1, fileId.Length - (fileId.IndexOf('/') + 1)) 
            };

            _amazonClient.DeleteObjectAsync(deleteObjectRequest);
        }

        public void UpdateAccessToken(string accessToken, DateTime expireDate)
        {
            throw new NotImplementedException();
        }

        public string UpdateFile(string fileName, string filePath, string fileType, string directoryId, string fileId)
        {
            try
            {
                long totalSize = 0;
                var path = filePath;
                var filename = Path.GetFileName(path);
                var mimetype = MimeMapping.GetMimeMapping(filename);

                var fileTransferUtility =
                   new TransferUtility(_amazonClient);

                fileTransferUtility.UploadAsync(filePath, directoryId).Wait();

                return "TRUE";

            }
            catch (Exception ex)
            {
                if (ex != null && ex.Message != null)
                    return "error-" + ex.Message;
                else
                    return "error-Internal Error";
            }
        }

        public string UploadFile(string fileName, string filePath, string fileType, string directoryId)
        {
            try
            {
                long totalSize = 0;
                var path = filePath;
                var filename = Path.GetFileName(path);
                var mimetype = MimeMapping.GetMimeMapping(filename);

                var fileTransferUtility =
                   new TransferUtility(_amazonClient);

                fileTransferUtility.UploadAsync(filePath, directoryId).Wait();

                return directoryId + "/" + filename;

            }
            catch (Exception ex)
            {
                if (ex != null && ex.Message != null)
                    return "error-" + ex.Message;
                else
                    return "error-Internal Error";
            }

        }

    }
}
