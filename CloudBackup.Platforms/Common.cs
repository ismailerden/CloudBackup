using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.WebPlatforms
{
    public static class Common
    {
        public static dynamic GetConfig(string rootPath)
        {
            var JSON = System.IO.File.ReadAllText(rootPath + "/appsettings.json");
            return JObject.Parse(JSON);

        }
    }
}
