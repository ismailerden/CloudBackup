using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.ViewModel.DeviceApiModel
{
    public class DeviceFileListModel : BaseApiDeviceModel
    {
        public string subDirectory { get; set; }
        public List<string> directoryListing { get; set; }
    }
}
