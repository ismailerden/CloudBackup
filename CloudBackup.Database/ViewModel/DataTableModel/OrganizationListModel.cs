using CloudBackup.Database.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.ViewModel.DataTableModel
{
    public class OrganizationListModel
    {
        public int Id { get; set; }
        public string HashedId { get; set; }
        public string Name { get; set; }
        public string PersonFullName { get; set; }
        public string ContactEmail { get; set; }
        public string AddressBindings { get; set; }
        public ActiveStatus ActiveStatus { get; set; }
        public int Count { get; set; }
    }
}
