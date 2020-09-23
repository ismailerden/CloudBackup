using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.ViewModel.DeviceApiModel
{
    public class LogDeviceModel : BaseApiDeviceModel
    {
        public List<string> errorList { get; set; }
        public int devicePlanId { get; set; }
    }
}
