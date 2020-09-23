using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.Enum
{
    public enum DeviceStatus:int
    {
        MissingApiAndSecretKey = -1,
        Online = 1 ,
        Offline = 2
    }
}
