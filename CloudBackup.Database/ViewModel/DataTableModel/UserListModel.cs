using CloudBackup.Database.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.ViewModel.DataTableModel
{
    public class UserListModel
    {
        public int Id { get; set; }
        public string HashedId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int Count { get; set; }
        public ActiveStatus ActiveStatus { get; set; }
        public int OrganizationId { get; set; }
    }
}
