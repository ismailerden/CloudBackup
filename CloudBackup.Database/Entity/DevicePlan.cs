using CloudBackup.Database.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.Entity
{
    public class DevicePlan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime BackupStartDate { get; set; }
        public string RetryPlan { get; set; }
        public int PlanId { get; set; }
        public string LocalSource { get; set; }
        public string RemoteSource { get; set; }
        public ActiveStatus ActiveStatus { get; set; }
        public int OrganizationId { get; set; }
        public string BackgroundJobId { get; set; }
    }
}
