using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudBackup.WindowsService.Model
{
    public class LogDeviceModel : BaseDeviceModel
    {
        public List<string> errorList { get; set; }
        public int devicePlanId { get; set; }
    }
}
