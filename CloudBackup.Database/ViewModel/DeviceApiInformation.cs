using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.ViewModel
{
    public class DeviceApiInformation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HashedDeviceId { get; set; }
        public string ApiAccessKey { get; set; }
        public string ApiSecretKey { get; set; }
    }
}
