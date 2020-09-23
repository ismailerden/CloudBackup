using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.ViewModel
{
    public class DashboardViewModel
    {
        public int CreatedFile { get; set; }
        public int FailedCount { get; set; }
        public int DirectoryCount { get; set; }
        public int ProccessCount { get; set; }
        public int DeletedCount { get; set; }
        public int UpdatedCount { get; set; }
        public decimal TotalSize { get; set; }
        public int DeviceCount { get; set; }
        public int PlanCount { get; set; }
    }
}
