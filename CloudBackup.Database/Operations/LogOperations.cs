using CloudBackup.Database.ViewModel.DataTableModel;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Linq;
using CloudBackup.Database.Entity;

namespace CloudBackup.Database.Operations
{
    public class LogOperations : BaseOperations
    {
        public LogOperations(string connectionString) : base(connectionString)
        {

        }
        public List<LogListModel> GetListModel(int organizationId, int devicePlanId, int skip, int take)
        {
            if (take == -1)
                take = 5;
            string sql = "select dpl.\"Id\",dp.\"Name\" \"PlanName\" , d.\"Name\" \"DeviceName\" , dpl.\"Description\" \"LogText\", " +
" dpl.\"CreatedDate\" \"ProcessDate\" ,count(*) OVER () \"Count\" from public.\"DevicePlanLog\" dpl " +
" left join public.\"DevicePlan\" dp on dp.\"Id\" = dpl.\"DevicePlanId\" and dp.\"ActiveStatus\" = 1 " +
" left join public.\"Plan\" p on p.\"Id\" = dp.\"PlanId\" and p.\"ActiveStatus\" = 1 " +
" inner join public.\"Device\" d on d.\"Id\" = dpl.\"DeviceId\" and d.\"ActiveStatus\" = 1 " +
" where dpl.\"OrganizationId\" = @organizationId  " +
" and d.\"OrganizationId\" = @organizationId " +
" and (dp.\"Id\" = @devicePlanId or @devicePlanId= 0) " +
" order by 5 desc " +
" offset @skip limit @take ";
            object data = new { organizationId = organizationId, skip = skip, take = take, devicePlanId = devicePlanId };
            return _connection.Query<LogListModel>(sql, data).ToList();
        }
        public void InsertLog(DevicePlanLog log)
        {
            string sql = "insert into public.\"DevicePlanLog\"(\"Description\",\"DevicePlanId\",\"OrganizationId\",\"DeviceId\",\"Detail\",\"CreatedDate\") " +
" values(@Description, @DevicePlanId, @OrganizationId, @DeviceId, @Detail, @CreatedDate)";
            object data = new { Description = log.Description,
                DevicePlanId = log.DevicePlanId,
                OrganizationId = log.OrganizationId,
                DeviceId = log.DeviceId,
                Detail = log.Detail,
                CreatedDate = DateTime.Now
            };
            _connection.Execute(sql, data);
        }
    }
}
