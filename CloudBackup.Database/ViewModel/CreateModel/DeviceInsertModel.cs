using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.ViewModel.CreateModel
{
    public class DeviceInsertModel
    {
        public string HashedDeviceId { get; set; }
        public int DeviceId { get; set; }
        public int Id { get; set; }
        public string DeviceName { get; set; }
        public string DeviceDescription { get; set; }
        public bool CloseModal { get; set; }
        public string BakcupJobId { get; set; }
        public string FileListJobId { get; set; }
        public string CheckOnlineJobId { get; set; }
        public int OrganizationId { get; set; }

        public string CpuId { get; set; }
        public string DiskSeriNo { get; set; }
        public string MacAddress { get; set; }
        public string ApiAccessKey { get; set; }
        public string ApiSecretKey { get; set; }
    }
}
