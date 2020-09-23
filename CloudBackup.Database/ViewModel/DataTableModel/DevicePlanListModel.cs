using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.ViewModel.DataTableModel
{
    public class DevicePlanListModel
    {
        public string HashedId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PlanName { get; set; }
        public string RetryPlan { get; set; }
        public DateTime? LastProcessTime { get; set; }
        public int Count { get; set; }
    }
}
