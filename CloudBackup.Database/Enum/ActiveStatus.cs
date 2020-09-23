using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.Enum
{
    public enum ActiveStatus:int
    {
        Deleted = -1 ,
        Active = 1 ,
        Passive = 0
    }
}
