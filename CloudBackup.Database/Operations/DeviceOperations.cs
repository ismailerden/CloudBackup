using CloudBackup.Database.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Linq;
using CloudBackup.Database.Enum;
using CloudBackup.Database.ViewModel.CreateModel;
using CloudBackup.Database.ViewModel.DeviceApiModel;
using CloudBackup.Database.Entity;
using CloudBackup.WebPlatforms;

namespace CloudBackup.Database.Operations
{
    public class DeviceOperations : BaseOperations
    {
        public DeviceOperations(string connectionString) : base(connectionString)
        {

        }
        public DeviceInsertModel GetApiAccessAndSecretKeyDevice(string accessKey, string secretKey)
        {
            string sql = "select * from public.\"Device\" where \"ApiAccessKey\" = @apiAccessKey and \"ApiSecretKey\" =@apiSecretKey";
            object data = new { apiAccessKey = accessKey, apiSecretKey = secretKey };
            return _connection.Query<DeviceInsertModel>(sql, data).FirstOrDefault();
        }
        public bool ResetKeyAndAccesKey(int organizationId, int deviceId)
        {
            string sql = "update public.\"Device\" set \"ApiAccessKey\" =@apiAccessKey , \"ApiSecretKey\" = @apiSecretKey  " +
" where \"Id\" = @id and \"OrganizationId\" = @organizationId";
            object data = new
            {
                apiAccessKey = Guid.NewGuid().ToString("N"),
                apiSecretKey = Guid.NewGuid().ToString("N"),
                id = deviceId,
                organizationId = organizationId
            };
            _connection.Execute(sql, data);
            return true;
        }
        public void UpdateDevicePlanBackupJobId(int deviceJobId, string jobId, int organizationId)
        {
            string sql = "update public.\"DevicePlan\" set \"BackgroundJobId\" = @backgroundJobId where \"Id\" = @id and \"OrganizationId\" = @organizationId";
            object data = new { id = deviceJobId, organizationId = organizationId, backgroundJobId = jobId };
            _connection.Execute(sql, data);
        }
        public void UpdateDeviceFileListJobId(int deviceId, string jobId, int organizationId)
        {
            string sql = "update public.\"Device\" set \"FileListJobId\" = @backgroundJobId where \"Id\" = @id and \"OrganizationId\" = @organizationId";
            object data = new { id = deviceId, organizationId = organizationId, backgroundJobId = jobId };
            _connection.Execute(sql, data);
        }
        public void UpdateDeviceCheckStatusJobId(int deviceId, string jobId, int organizationId)
        {
            string sql = "update public.\"Device\" set \"CheckOnlineJobId\" = @backgroundJobId where \"Id\" = @id and \"OrganizationId\" = @organizationId";
            object data = new { id = deviceId, organizationId = organizationId, backgroundJobId = jobId };
            _connection.Execute(sql, data);
        }
        public DeviceApiInformation GetDeviceApiInformation(int deviceId, int organizationId)
        {
            string sql = "select \"Id\",\"Name\",\"ApiAccessKey\",\"ApiSecretKey\" from public.\"Device\" where \"Id\" = @id and \"OrganizationId\" = @organizationId";
            object data = new { id = deviceId, organizationId = organizationId };
            return _connection.Query<DeviceApiInformation>(sql, data).FirstOrDefault();
        }
        public List<DeviceListModel> GetListModel(int organizationId, int skip, int take)
        {
            string sql = "select \"Id\",\"Name\",\"CreatedDate\",dd.\"DeviceStatus\",count(*) OVER () \"Count\", " +
"(select \"CreatedDate\" from \"DevicePlan\" dp inner join \"DevicePlanLog\" dpl on dp.\"Id\" = dpl.\"DevicePlanId\" where dp.\"DeviceId\" = dd.\"Id\" order by dpl.\"CreatedDate\" desc OFFSET 0 LIMIT 1) \"LastProcessTime\" " +
"from public.\"Device\" dd " +
"where dd.\"OrganizationId\" = @organizationId and dd.\"ActiveStatus\"=1" +
"order by 3 desc " +
"OFFSET @skip LIMIT @take ";
            object data = new { organizationId = organizationId, skip = skip, take = take };
            return _connection.Query<DeviceListModel>(sql, data).ToList();
        }
        public int InsertJob(int organizationId, string deviceName, string deviceDescription)
        {
            string sql = "insert into public.\"Device\"(\"Name\",\"Description\",\"DeviceStatus\",\"CreatedDate\",\"OrganizationId\",\"ActiveStatus\",\"ApiAccessKey\",\"ApiSecretKey\") " +
"values(@name,@description,@deviceStatus,@createdDate,@organizationId,@activeStatus,@apiAccessKey,@apiSecretKey)  RETURNING \"Id\" ";
            object data = new
            {
                name = deviceName,
                description = deviceDescription,
                createdDate = DateTime.Now,
                organizationId = organizationId,
                activeStatus = ActiveStatus.Active,
                deviceStatus = DeviceStatus.Offline,
                apiAccessKey = Guid.NewGuid().ToString("N"),
                apiSecretKey = Guid.NewGuid().ToString("N")
            };
            return _connection.ExecuteScalar<int>(sql, data);
        }
        public DeviceInsertModel GetDeviceById(int organizationId, int deviceId)
        {
            string sql = "select \"Id\" \"DeviceId\", \"Name\" \"DeviceName\",\"Description\" \"DeviceDescription\" ,\"BakcupJobId\",\"FileListJobId\",\"CheckOnlineJobId\",\"CpuId\",\"DiskSeriNo\",\"MacAddress\",\"ApiAccessKey\",\"ApiSecretKey\" from public.\"Device\" where \"ActiveStatus\" = 1 and \"Id\"=@id and \"OrganizationId\"=@organizationId";
            object data = new { id = deviceId, organizationId = organizationId };
            return _connection.Query<DeviceInsertModel>(sql, data).FirstOrDefault();
        }
        public void UpdateDevice(DeviceInsertModel model, int organizationId)
        {
            string sql = "update public.\"Device\" set \"Name\" = @name , \"Description\" = @description where \"OrganizationId\" = @organizationId and \"Id\" = @id";
            object data = new { name = model.DeviceName, description = model.DeviceDescription, organizationId = organizationId, id = model.DeviceId };
            _connection.Execute(sql, data);
        }
        public void UpdateDeviceInformation(CheckModel model)
        {
            string sql = "update public.\"Device\" set \"CpuId\" =@cpuId , \"DiskSeriNo\"=@diskSeriNo,\"MacAddress\"=@macAddress where \"Id\"=@id";
            object data = new { id = model.deviceId, macAddress = model.macAddress, diskSeriNo = model.diskVolume, cpuId = model.cpuId };
            _connection.Execute(sql, data);
        }
        public void DeleteDevice(int Id, int organizationId)
        {
            string sql = "update public.\"Device\" set \"ActiveStatus\" = -1 where \"OrganizationId\" = @organizationId and \"Id\" = @id";
            object data = new { organizationId = organizationId, id = Id };
            _connection.Execute(sql, data);
        }
        public int DevicePlanCount(int deviceId, int organizationId)
        {
            string sql = "select count(1) from public.\"DevicePlan\"  where \"OrganizationId\" = @organizationId and \"DeviceId\" = @id and \"ActiveStatus\" = 1";
            object data = new { organizationId = organizationId, id = deviceId };
            return _connection.ExecuteScalar<int>(sql, data);
        }
        public int PlanDeviceCount(int planId, int organizationId)
        {
            string sql = "select count(1) from public.\"DevicePlan\"  where \"OrganizationId\" = @organizationId and \"PlanId\" = @id and \"ActiveStatus\" = 1";
            object data = new { organizationId = organizationId, id = planId };
            return _connection.ExecuteScalar<int>(sql, data);
        }
        public void InsertFileList(List<DeviceFileList> model, string subDirectory)
        {
            if (model.Count > 0)
            {
                string deleteSql = "delete from public.\"DeviceFileList\" where \"DeviceId\" = @id and \"OrganizationId\" = @organizationId and \"SubDirectory\" " + (String.IsNullOrEmpty(subDirectory) == true ? " is null " : " = @subDirectory ") + "";
                object deleteData = new { id = model.FirstOrDefault().DeviceId, organizationId = model.FirstOrDefault().OrganizationId, subDirectory = subDirectory };
                _connection.Execute(deleteSql, deleteData);

                string sql = "insert into public.\"DeviceFileList\"(\"SubDirectory\",\"Directory\",\"DeviceId\",\"OrganizationId\",\"CreatedDate\") " +
    " values(@SubDirectory, @Directory, @DeviceId, @OrganizationId, @CreatedDate)";
                _connection.Execute(sql, model);
            }
        }
        public void DeleteFileList(int deviceId, int organizationId, string directory)
        {
            string deleteSql = "delete from public.\"DeviceFileList\" where \"DeviceId\" = @id and \"OrganizationId\" = @organizationId and \"SubDirectory\"=@subDirectory";
            object deleteData = new { id = deviceId, organizationId = organizationId, subDirectory = directory };
            _connection.Execute(deleteSql, deleteData);
        }
        public List<FileListModel> GetFileList(int deviceId, int organizationId, string directory)
        {
            if (!String.IsNullOrEmpty(directory))
                directory = directory.Replace(@"\\", @"\");
            string sql = "select \"Id\",\"Directory\" from public.\"DeviceFileList\" where \"DeviceId\" = @deviceId and \"OrganizationId\" = @organizationId and \"SubDirectory\" " + (String.IsNullOrEmpty(directory) == true ? " is null " : " =  @subDirectory ") + " ";
            object data = new { deviceId = deviceId, organizationId = organizationId, subDirectory = directory };
            return _connection.Query<FileListModel>(sql, data).ToList();
        }
        public double IsFileListIncoming(int deviceId, int organizationId, string directory)
        {
            if (!String.IsNullOrEmpty(directory))
                directory = directory.Replace(@"\\", @"\");
            string sql = "select DATEDIFF('second', \"CreatedDate\"::timestamp, (now() AT TIME ZONE 'Europe/Moscow')::timestamp) from public.\"DeviceFileList\" where \"DeviceId\" = @deviceId and \"OrganizationId\" = @organizationId and \"SubDirectory\" " + (String.IsNullOrEmpty(directory) == true ? " is null " : " =  @subDirectory ") + " order by \"CreatedDate\" desc limit 1";
            object data = new { deviceId = deviceId, organizationId = organizationId, subDirectory = directory };
            return _connection.ExecuteScalar<double>(sql, data);
        }
        public string GetDirectoryById(int id, int organizationId)
        {
            string sql = "select \"Directory\" from public.\"DeviceFileList\" where \"Id\" = @id and \"OrganizationId\" = @organizationId  ";
            object data = new { id = id, organizationId = organizationId};
            return _connection.ExecuteScalar<string>(sql, data);
        }
    }
}
