using CloudBackup.Database.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.ViewModel
{
    public class ViewSelectList
    {
        public int RealId { get; set; }
        public string Id { get; set; }
        public string Text { get; set; }
        public PlanType Type { get; set; }
    }
}
