using CloudBackup.Database.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.Entity
{
    public class Device
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CpuId { get; set; }
        public string DiskSeriNo { get; set; }
        public string MacAddress { get; set; }
        public string ApiAccessKey { get; set; }
        public string ApiSecretKey { get; set; }
        public string BakcupJobId { get; set; }
        public string FileListJobId { get; set; }
        public string CheckOnlineJobId { get; set; }
        public int OrganizationId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public ActiveStatus ActiveStatus { get; set; }
        public DeviceStatus DeviceStatus { get; set; }

    }
}
