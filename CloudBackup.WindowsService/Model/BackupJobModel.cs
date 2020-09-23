using CloudBackup.Database.Enum;
using CloudBackup.WindowsService.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.ViewModel.DeviceApiModel
{
    public class BackupJobModel:BaseDeviceModel
    {
        public PlanType PlanType { get; set; }
        public string PlanJsonData { get; set; }
        public string LocalDirectory { get; set; }
        public string RemoteDirectory { get; set; }
        public string DevicePlanId { get; set; }
    }
}
