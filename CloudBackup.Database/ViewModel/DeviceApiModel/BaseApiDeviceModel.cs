using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.ViewModel.DeviceApiModel
{
   public class BaseApiDeviceModel
    {
        public int deviceId { get; set; }
        public string apiAccessKey { get; set; }
        public string apiSecretKey { get; set; }
        public string cpuId { get; set; }
        public string macAddress { get; set; }
        public string diskVolume { get; set; }
    }
}
