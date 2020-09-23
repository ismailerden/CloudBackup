using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CloudBackup.WebPlatforms.AmazonS3
{
    public class AmazonS3 : IPlatform
    {
        public List<FileListModel> GetDirectoryList(string directory, string accessToken)
        {
            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.USWest1, // MUST set this before setting ServiceURL and it should match the `MINIO_REGION` enviroment variable.
                ForcePathStyle = true // MUST be true to work correctly with Minio server
            };
            string region = accessToken.Split("-")[2];
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

            //var amazonS3Client = new AmazonS3Client("AKIAIZ7ZNECIUB4MEKRQ", "g2OZECVv8kRsbixfiP52xnKhHo/FSny5hm0qmb4q", config);
            var amazonS3Client = new AmazonS3Client(accessToken.Split("-")[0], accessToken.Split("-")[1], config);
            List<FileListModel> list = new List<FileListModel>();
            if (String.IsNullOrEmpty(directory))
            {
                var listBucketResponse = (amazonS3Client.ListBucketsAsync()).Result;
                foreach (var item in listBucketResponse.Buckets)
                {
                    list.Add(new FileListModel() { CloudId = item.BucketName, Directory = item.BucketName });
                }
            }
            else
            {
                string bucket = Regex.Split(directory, "/")[0];
                string innerDirectory = "";
                string[] directoryArray = directory.Split("/");
                ListObjectsV2Request request = new ListObjectsV2Request
                {
                    BucketName = bucket,
                    Delimiter = @"/",
                };
                if (directoryArray.Length > 1)
                {
                  
                    for (int i = 1; i < directoryArray.Length - 1; i++)
                    {
                        innerDirectory = directoryArray[i] + "/";
                    }
                    request.Prefix = innerDirectory;
                }
                else
                {


                }
                var bucketDirectory = (amazonS3Client.ListObjectsV2Async(request)).Result;
                foreach (var item in bucketDirectory.CommonPrefixes)
                {
                    list.Add(new FileListModel() { CloudId = bucket + "/" + item, Directory = bucket + "/" + item });
                }

            }


            return list;
        }
    }
}
