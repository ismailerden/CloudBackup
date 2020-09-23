using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.ViewModel.DeviceApiModel
{
    public class BackupJobModelGoogle : BackupJobModel
    {
        public string GoogleAccessToken { get; set; }
        public string GoogleApiCode { get; set; }
        public string GoogleTokenExpired { get; set; }
        public string GoogleRefreshToken { get; set; }
    }
}
