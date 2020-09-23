using CloudBackup.Database.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.ViewModel.DeviceApiModel
{
    public class BackupJobModel
    {
        public PlanType PlanType { get; set; }
        public string LocalDirectory { get; set; }
        public string RemoteDirectory { get; set; }
        public string DevicePlanId { get; set; }

      
    }
}
