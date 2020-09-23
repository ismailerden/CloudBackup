﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.WindowsService.Model
{
    public class BackupLog:CheckModel
    {
        public int Id { get; set; }
        public int CreateFileCount { get; set; }
        public int FailedCount { get; set; }
        public int CreateDirectoryCount { get; set; }
        public int ProccessCount { get; set; }
        public double TotalSize { get; set; }
        public int DevicePlanId { get; set; }
        public int OrganizationId { get; set; }
        public int DeletedCount { get; set; }
       public int UpdatedCount { get; set; }
    }
}
