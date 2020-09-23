using CloudBackup.Database.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.Entity
{
    public class AddressBinding
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
        public int OrganizationId { get; set; }
        public ActiveStatus ActiveStatus { get; set; }
    }
}
