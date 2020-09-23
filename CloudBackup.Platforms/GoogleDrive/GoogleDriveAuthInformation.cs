using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.WebPlatforms.GoogleDrive
{
    public class GoogleDriveAuthInformation
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string userName { get; set; }
    }
}
