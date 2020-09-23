using CloudBackup.WindowsService.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.WindowsService
{
    public class CheckModel:BaseDeviceModel
    {
       
        public bool updateData { get; set; }
        public string subDirectory { get; set; }
        public List<string> directoryListing { get; set; }
    }
}
