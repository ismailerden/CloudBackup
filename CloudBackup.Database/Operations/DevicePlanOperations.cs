using CloudBackup.Database.ViewModel.DataTableModel;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Linq;
using CloudBackup.Database.ViewModel.CreateModel;

namespace CloudBackup.Database.Operations
{
    public class DevicePlanOperations : BaseOperations
    {
        public DevicePlanOperations(string connectionString) : base(connectionString)
        {

        }
        public List<DevicePlanListModel> GetListModel(int organizationId, int skip, int take,int deviceId)
        {
            string sql = "select dp.\"Id\",dp.\"Name\",dp.\"Description\", pl.\"Name\" \"PlanName\",dp.\"RetryPlan\" ,count(*) OVER () \"Count\", " +
" (select \"CreatedDate\" from \"DevicePlanLog\" dpl where dpl.\"DevicePlanId\" = dp.\"Id\" order by \"Id\" desc limit 1) \"LastProcessTime\" "+
" from public.\"DevicePlan\" dp " +
" inner join public.\"Plan\" pl on pl.\"Id\" = dp.\"PlanId\" and pl.\"ActiveStatus\" = 1 "+
" where dp.\"ActiveStatus\" = 1 and dp.\"OrganizationId\" = @organizationId and dp.\"DeviceId\" = @deviceId "+
" order by 1 desc " +
" offset @skip limit @take ";
            object data = new { organizationId = organizationId, skip = skip, take = take , deviceId = deviceId };
            return _connection.Query<DevicePlanListModel>(sql, data).ToList();
        }
        public int InsertDevicePlan(DevicePlanInsertModel model)
        {
            string sql = "insert into " +
" public.\"DevicePlan\" (\"Name\",\"Description\",\"BackupStartDate\",\"RetryPlan\",\"PlanId\",\"LocalSource\",\"RemoteSource\",\"OrganizationId\",\"DeviceId\",\"ActiveStatus\") "+
" values(@Name, @Description, @BackupStartDate, @RetryPlan, @PlanId, @LocalSource, @RemoteSource, @OrganizationId, @DeviceId,1)   RETURNING \"Id\" ";
            object data = new
            {
                Name = model.Name,
                Description = model.Description,
                BackupStartDate = model.RealInsertBackupStartDate,
                RetryPlan = model.RetryPlan,
                PlanId = model.PlanId,
                LocalSource = model.LocalSource,
                RemoteSource = model.RemoteSource,
                OrganizationId = model.OrganizationId,
                DeviceId = model.DeviceId
            };
            return _connection.ExecuteScalar<int>(sql, data);
        }
        public DevicePlanInsertModel GetDevicePlanById(int organizationId, int planId)
        {
            string sql = "select \"Id\",\"Name\",\"Description\",\"PlanId\",\"LocalSource\",\"RemoteSource\",\"DeviceId\",\"RetryPlan\",\"BackupStartDate\" \"RealInsertBackupStartDate\" , \"BackgroundJobId\" from public.\"DevicePlan\" where \"Id\" = @id and \"OrganizationId\" = @organizationId ";
            object data = new { id = planId, organizationId = organizationId };
            return _connection.Query<DevicePlanInsertModel>(sql, data).FirstOrDefault();
        }
        public void UpdateDevicePlan(DevicePlanInsertModel model, int organizationId)
        {
            string sql = "update public.\"DevicePlan\" set \"Name\" = @name , \"Description\" = @description , \"BackupStartDate\" = @backupStartDate, "+
" \"RetryPlan\" = @retryPlan , \"PlanId\" = @planId , \"LocalSource\" = @localSource , \"RemoteSource\" = @remoteSource " +
" where \"OrganizationId\" = @organizationId and \"Id\" = @id ";
            object data = new {
                name = model.Name,
                description = model.Description,
                backupStartDate = model.RealInsertBackupStartDate,
                retryPlan = model.RetryPlan,
                planId = model.PlanId,
                localSource = model.LocalSource,
                remoteSource = model.RemoteSource,
                organizationId = organizationId,
                id = model.Id
            };
            _connection.Execute(sql, data);
        }
        public void DeleteDevicePlan(int Id, int organizationId)
        {
            string sql = "update public.\"DevicePlan\" set \"ActiveStatus\" = -1 where \"OrganizationId\" = @organizationId and \"Id\" = @id";
            object data = new { organizationId = organizationId, id = Id };
            _connection.Execute(sql, data);
        }
    }
}
