using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.Entity
{
    public class DevicePlanLog
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? DevicePlanId { get; set; }
        public int DeviceId { get; set; }
        public int OrganizationId { get; set; }
    }
}
