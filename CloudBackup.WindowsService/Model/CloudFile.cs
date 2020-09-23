using CloudBackup.WindowsService.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudBackup.WindowsService.Model
{
    public class CloudFile
    {
        public int Id { get; set; }
        public int? DevicePlanId { get; set; }
        public string SubDirectory { get; set; }
        public string FullName { get; set; }
        public FileType? Type { get; set; }
        public DateTime? ProccessDate { get; set; }
        public ResultStatus? ResultStatus { get; set; }
        public string ErrorMessage { get; set; }
        public string CloudId { get; set; }
        public double Length { get; set; }
    }
}
