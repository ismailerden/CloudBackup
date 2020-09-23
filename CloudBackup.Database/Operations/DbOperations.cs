using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace CloudBackup.Database.Operations
{
    public class DpOperations
    {
        string _connectionString;
        private OrganizationOperations orgOperations;
        //public string _connectionString;
        public DpOperations()
        {
            var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ;

            var JSON = System.IO.File.ReadAllText(buildDir + "/appsettings.json");
            dynamic config = JObject.Parse(JSON);
            _connectionString = config.ConnectionString.dbConnectionString.ToString();
            orgOperations = new OrganizationOperations(_connectionString);
        }
        public OrganizationOperations Organization { get { return new OrganizationOperations(_connectionString); } }
        public UserOperations User { get { return new UserOperations(_connectionString); } }
        public DeviceOperations Device { get { return new DeviceOperations(_connectionString); } }
        public PlanOperations Plan { get { return new PlanOperations(_connectionString); } }
        public DevicePlanOperations DevicePlan { get { return new DevicePlanOperations(_connectionString); } }
        public LogOperations Log { get { return new LogOperations(_connectionString); } }
        public BackupLogOperations BackupLog { get { return new BackupLogOperations(_connectionString); } }
    }
}
