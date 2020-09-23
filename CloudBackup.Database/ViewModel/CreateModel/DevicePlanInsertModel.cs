using CloudBackup.Database.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.ViewModel.CreateModel
{
    public class DevicePlanInsertModel
    {
        public int Id { get; set; }
        public string HashedDevicePlanId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BackupStartDate { get; set; }
        public string BackupStartTime { get; set; }
        public DateTime? RealInsertBackupStartDate { get; set; }
        public RetryPlan RetryPlan { get; set; }
        public string HashedPlanId { get; set; }
        public int PlanId { get; set; }
        public string LocalSource { get; set; }
        public string RemoteSource { get; set; }
        public int OrganizationId { get; set; }
        public string DeviceHashedId { get; set; }
        public int DeviceId { get; set; }
        public string BackgroundJobId { get; set; }
        public bool CloseModal { get; set; }
    }
}
