using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.WindowsAppControlService
{
    public class CheckModel
    {
        public int deviceId { get; set; }
        public string apiAccessKey { get; set; }
        public string apiSecretKey { get; set; }
        public string cpuId { get; set; }
        public string macAddress { get; set; }
        public string diskVolume { get; set; }
        public bool updateData { get; set; }
    }
}
