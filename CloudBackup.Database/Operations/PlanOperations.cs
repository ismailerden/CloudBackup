using CloudBackup.Database.ViewModel;
using CloudBackup.Database.ViewModel.DataTableModel;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Linq;
using CloudBackup.Database.ViewModel.CreateModel;
using CloudBackup.Database.Enum;

namespace CloudBackup.Database.Operations
{
    public class PlanOperations:BaseOperations
    {
        public PlanOperations(string connectionString) : base(connectionString)
        {

        }
        public List<PlanListModel> GetListModel(int organizationId, int skip, int take)
        {
            string sql = "select \"Id\",\"Name\",\"Type\",\"ActiveStatus\", count(*) OVER () \"Count\", " +
"(select count(1) from public.\"DevicePlan\" dp where dp.\"PlanId\" = pl.\"Id\" and dp.\"ActiveStatus\" = 1) \"DeviceCount\"  from public.\"Plan\" pl "+
                  "where pl.\"OrganizationId\" = @organizationId and pl.\"ActiveStatus\" = 1 "+
                  "order by 1 desc " +
                  "offset @skip limit @take ";
            object data = new { organizationId = organizationId, skip = skip, take = take };
            return _connection.Query<PlanListModel>(sql, data).ToList();
        }
        public int InsertPlatform(PlatformInsertModel model)
        {
            string sql = "insert into public.\"Plan\"(\"Name\",\"Type\",\"OrganizationId\",\"PlanData\",\"ActiveStatus\") "+
"values(@Name, @Type, @OrganizationId, @PlanData,1)   RETURNING \"Id\" ";
            object data = new { Name = model.Name,
                Type = model.Type,
                OrganizationId = model.OrganizationId,
                PlanData = model.JsonData,
                ActiveStatus = ActiveStatus.Active
            };
            return _connection.ExecuteScalar<int>(sql, data);
        }
        public List<ViewSelectList> GetSelectList(int organizationId)
        {
            string sql = "select \"Id\" \"RealId\",\"Name\" \"Text\",\"Type\" from public.\"Plan\" where \"ActiveStatus\" = 1 and \"OrganizationId\" = @organizationId ";
            object data = new { organizationId  = organizationId};
            return _connection.Query<ViewSelectList>(sql, data).ToList();
        }
        public PlatformInsertModel GetPlanById(int organizationId, int planId)
        {
            string sql = "select \"Id\",\"Name\",\"Type\",\"PlanData\" \"JsonData\" from public.\"Plan\" where \"Id\" = @id and \"OrganizationId\" = @organizationId ";
            object data = new { id = planId, organizationId = organizationId };
            return _connection.Query<PlatformInsertModel>(sql, data).FirstOrDefault();
        }
        public void UpdatePlan(PlatformInsertModel model, int organizationId)
        {
            string sql = "update public.\"Plan\" set \"Name\" = @name , \"Type\" = @type , \"PlanData\" = @planData "+
"where \"Id\" = @id and \"OrganizationId\" = @organizationId ";
            object data = new { name = model.Name, type = model.Type, organizationId = organizationId, planData = model.JsonData, id = model.Id };
            _connection.Execute(sql, data);
        }
        public void DeletePlan(int Id, int organizationId)
        {
            string sql = "update public.\"Plan\" set \"ActiveStatus\" = -1 where \"OrganizationId\" = @organizationId and \"Id\" = @id";
            object data = new { organizationId = organizationId, id = Id };
            _connection.Execute(sql, data);
        }
    }
}
