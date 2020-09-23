using CloudBackup.Database.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.ViewModel.DataTableModel
{
    public class PlanListModel
    {
        public string HashedId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public PlanType Type { get; set; }
        public ActiveStatus ActiveStatus { get; set; }
        public int DeviceCount { get; set; }
        public int Count { get; set; }
    }
}
