using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudBackup.WindowsService.Model
{
   public class BaseDeviceModel
    {
        public int deviceId { get; set; }
        public string apiAccessKey { get; set; }
        public string apiSecretKey { get; set; }
        public string cpuId { get; set; }
        public string macAddress { get; set; }
        public string diskVolume { get; set; }
    }
}
