using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.ViewModel.CreateModel
{
    public class OrganizationInsertModel
    {
        public int Id { get; set; }
        public string HashedId { get; set; }
        public bool CloseModal { get; set; }
        public string Name { get; set; }
        public string PersonFullName { get; set; }
        public string ContactEmail { get; set; }
        public string AddressBindings { get; set; }
    }
}
