using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.ViewModel.CreateModel
{
   public  class AddressBindingInsertModel
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
        public int OrganizationId { get; set; }
    }
}
