using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.ViewModel
{
    public class ProfileViewModel
    {
        public int UserId { get; set; }
        public string HashedUserId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyPersonName { get; set; }
        public string CompanyPersonEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string UserEmailAddress { get; set; }
        public int OrganizationId { get; set; }

    }
}
