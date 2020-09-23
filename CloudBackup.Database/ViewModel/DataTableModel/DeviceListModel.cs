using CloudBackup.Database.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.ViewModel
{
    public class DeviceListModel
    {
        public string HashedId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public DeviceStatus DeviceStatus { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastProcessTime { get; set; }
        public int Count { get; set; }
    }
}
