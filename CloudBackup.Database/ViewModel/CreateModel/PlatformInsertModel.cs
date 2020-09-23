using CloudBackup.Database.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.ViewModel.CreateModel
{
    public class PlatformInsertModel
    {
        public int Id { get; set; }
        public string HashedPlatformId { get; set; }
        public string Name { get; set; }
        public PlanType Type { get; set; }
        public string JsonData { get; set; }
        public int OrganizationId { get; set; }
        public bool CloseModal { get; set; }
    }
}
