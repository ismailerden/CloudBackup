using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.ViewModel.DataTableModel
{
    public class LogListModel
    {
        public string HashedId { get; set; }
        public int Id { get; set; }
        public string PlanName { get; set; }
        public string DeviceName { get; set; }
        public string LogText { get; set; }
        public DateTime? ProcessDate { get; set; }
        public int Count { get; set; }
    }
}
