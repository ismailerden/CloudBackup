using CloudBackup.Database.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Linq;
using CloudBackup.Database.ViewModel;
using CloudBackup.Database.ViewModel.DataTableModel;
using CloudBackup.Database.ViewModel.CreateModel;

namespace CloudBackup.Database.Operations
{
    public class OrganizationOperations:BaseOperations
    {
        public OrganizationOperations(string connectionString):base(connectionString)
        {

        }
        public AddressBinding GetAddress(string url,int port)
        {
            return _connection.Query<AddressBinding>("select * from public.\"AddressBinding\" where \"Address\"=@address and \"Port\" = @port",
                new { address = url , port = port }).FirstOrDefault();
        }
        public DashboardViewModel GetDashboard(int organizationId)
        {
            string sql = "select SUM(\"CreateFileCount\") \"CreatedFile\" ,  " +
" SUM(\"FailedCount\") \"FailedCount\" , "+
" SUM(\"CreateDirectoryCount\") \"DirectoryCount\" ,  "+
" SUM(\"ProccessCount\") \"ProccessCount\" , "+
" SUM(\"DeletedCount\") \"DeletedCount\" , "+
" SUM(\"UpdatedCount\") \"UpdatedCount\" , "+
" SUM(\"TotalSize\") \"TotalSize\" , "+
" (select count(1) from public.\"Device\" where \"OrganizationId\" = @organizationId and \"ActiveStatus\" = 1) \"DeviceCount\", "+
" (select count(1) from public.\"Plan\" where \"OrganizationId\" = @organizationId and \"ActiveStatus\" = 1) \"PlanCount\"   " +
" from public.\"BackupLog\" where \"OrganizationId\" = @organizationId and \"CreatedDate\" < CURRENT_DATE + INTERVAL '1 month' ";
            object data = new { organizationId = organizationId };
            return _connection.Query<DashboardViewModel>(sql,data).FirstOrDefault();

        }
        public List<OrganizationListModel> GetOrganizationList(int skip,int take,int organizationId)
        {
            string sql = "select count(*) OVER () \"Count\",*, " +
 " (select  array_to_string(array_agg(distinct adr.\"Address\" || ':' || adr.\"Port\"), ' , ') from public.\"AddressBinding\" adr where adr.\"OrganizationId\" = org.\"Id\" group by adr.\"OrganizationId\") \"AddressBindings\" " +
 " from public.\"Organization\" org where (org.\"ActiveStatus\"= 1 or org.\"ActiveStatus\"= 0) and (org.\"Id\" = @organizationId or @organizationId = 0) "+
 "OFFSET @skip LIMIT @take ";
            return _connection.Query<OrganizationListModel>(sql, new { skip = skip , take = take , organizationId = organizationId }).ToList();
        }
        public void ChangeOrganizationActiveStatus(int organizationId)
        {
            string sql = "update public.\"Organization\" set \"ActiveStatus\" = CASE \"ActiveStatus\" WHEN 0 THEN 1 WHEN 1 THEN 0 ELSE 0 END where \"Id\" = @organizationId";
            object data = new { organizationId = organizationId };
            _connection.Execute(sql,data);
        }
        public int CreateOrganization(OrganizationInsertModel model)
        {
            string sql = "insert into public.\"Organization\" (\"Name\",\"PersonFullName\",\"ContactEmail\",\"ActiveStatus\") "+
" values(@Name, @PersonFullName, @ContactEmail, 1) RETURNING \"Id\"";
            return _connection.ExecuteScalar<int>(sql,model);
        }
        public void UpdateOrganization(OrganizationInsertModel model)
        {
            string sql = "update public.\"Organization\" set \"Name\" = @Name , \"PersonFullName\" = @PersonFullName , \"ContactEmail\" = @ContactEmail where \"Id\" = @Id";
            _connection.Execute(sql,model);
        }
        public void DeleteAddressBinding (int organizationId)
        {
            string sql = "delete from public.\"AddressBinding\" where \"OrganizationId\" = @organizationId ;";
            _connection.Execute(sql,new { organizationId=organizationId });
        }
        public void InsertAddressBindings(int organizationId , List<AddressBindingInsertModel> insertModel)
        {
            string sql = " insert into public.\"AddressBinding\" (\"Address\",\"Port\",\"OrganizationId\",\"ActiveStatus\") "+
" values(@Address, @Port, @OrganizationId,1)";
            _connection.Execute(sql,insertModel);
        }
    }
}
