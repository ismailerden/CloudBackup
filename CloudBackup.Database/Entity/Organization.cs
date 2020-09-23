using CloudBackup.Database.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.Entity
{
    public class Organization
    {
        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public string PersonFullName { get; set; }
        public string ContactEmail { get; set; }
        public ActiveStatus ActiveStatus { get; set; }
    }
}
