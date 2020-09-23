using CloudBackup.Database.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.Entity
{
    public class Plan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PlanType Type { get; set; }
        public int OrganizationId { get; set; }
        public string PlanData { get; set; }
        public ActiveStatus ActiveStatus { get; set; }
    }
}
