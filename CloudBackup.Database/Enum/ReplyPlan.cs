using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.Database.Enum
{
    public enum RetryPlan:int
    {
        OneTime = 1,
        Minute30 = 2,
        Hour1 = 3,
        Hour3 = 4,
        Hour6 = 5 ,
        Hour12 = 6,
        Day1 = 7,
        Day2 = 8,
        Day3 = 9,
        Day4 = 10,
        Day5 = 11,
        Day6 = 12,
        Day7 = 13,
        Day8 = 14,
        Day9 = 15,
        Day10 = 16,
        Month1 = 17,
        Month2 = 18,
        Month3 = 19,
        Month4 = 20,
        Year1 = 21
    }
}
