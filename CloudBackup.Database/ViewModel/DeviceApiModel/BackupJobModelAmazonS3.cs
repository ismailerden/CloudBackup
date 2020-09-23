using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.ViewModel.DeviceApiModel
{
    public class BackupJobModelAmazonS3 : BackupJobModel
    {
        public string apiAccessKey { get; set; }
        public string apiSecretKey { get; set; }
        public string region { get; set; }
    }
}
