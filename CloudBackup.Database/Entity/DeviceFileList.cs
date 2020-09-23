using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.Entity
{
    public class DeviceFileList
    {
        public int Id { get; set; }
        public string SubDirectory { get; set; }
        public string Directory { get; set; }
        public int DeviceId { get; set; }
        public int OrganizationId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
