using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.Entity
{
    public class LoginAudit
    {
        public int LoginAuditId { get; set; }
        public int UserId { get; set; }
        public int OrganizationId { get; set; }
        public DateTime LoginDate { get; set; }
        public bool Result { get; set; }
    }
}
