using CloudBackup.Database.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.ViewModel.CreateModel
{
    public class UsersInsertModel
    {
        public int Id { get; set; }
        public string HashedId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool CloseModal { get; set; }
        public int OrganizationId { get; set; }
        public ActiveStatus ActiveStatus { get; set; }
        public int FailedLoginCount { get; set; }
    }
}
