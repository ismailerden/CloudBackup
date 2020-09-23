using CloudBackup.Database.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Linq;
using CloudBackup.Database.ViewModel;
using CloudBackup.Database.Enum;
using CloudBackup.Database.ViewModel.DataTableModel;
using CloudBackup.Database.ViewModel.CreateModel;

namespace CloudBackup.Database.Operations
{
    public class UserOperations : BaseOperations
    {
        public UserOperations(string connectionString) : base(connectionString)
        {

        }
        public int InsertLoginAudit(LoginAudit model)
        {
            string sql = "insert into \"LoginAudit\"(\"UserId\",\"OrganizationId\",\"LoginDate\",\"Result\") values(@userId, @organizationId, @loginDate, B'@Status') RETURNING \"LoginAuditId\"";
            object data = new { userId = model.UserId , organizationId=model.OrganizationId , loginDate = DateTime.Now , Status =model.Result };
            return _connection.ExecuteScalar<int>(sql,data);
        }
        public void UpdateFailedLoginCount(int userId)
        {
            string sql = "update public.\"User\" set \"FailedLoginCount\" = coalesce(\"FailedLoginCount\", 0) + 1 where \"Id\" = @userId";
            object data = new { userId = userId };
            _connection.Execute(sql,data);
        }
        public void ResetFailedCount(int userId)
        {
            string sql = "update public.\"User\" set \"FailedLoginCount\" = 0 where \"Id\" = @userId";
            object data = new { userId = userId };
            _connection.Execute(sql, data);
        }
        public User GetUserByUserName(string userName , int organizationId)
        {
            string sql = "select * from public.\"User\" where \"UserName\" = @userName and \"OrganizationId\"= @organizationId and \"ActiveStatus\" = 1 and exists (select 1 from public.\"Organization\" org where org.\"Id\" = \"OrganizationId\" and org.\"ActiveStatus\" = 1 limit 1 ) ";
            object data = new { userName = userName , organizationId = organizationId };
            return _connection.Query<User>(sql, data).FirstOrDefault();
        }
        public ProfileViewModel GetProfile(int userId , int organizationId)
        {
            string sql = "select org.\"Name\" \"CompanyName\" , org.\"PersonFullName\" \"CompanyPersonName\" , org.\"ContactEmail\" \"CompanyPersonEmail\" , "+
" u.\"FirstName\" , u.\"LastName\" , u.\"UserName\" , u.\"Email\" \"UserEmailAddress\" , u.\"Id\" \"UserId\" from public.\"User\" u " +
" inner join public.\"Organization\" org on org.\"Id\" = u.\"OrganizationId\" "+
" where u.\"Id\" = @userId and u.\"OrganizationId\" = @organizationId";
            return _connection.QueryFirstOrDefault<ProfileViewModel>(sql , new { organizationId = organizationId , userId = userId });
        }
        public void UpdateProfile(ProfileViewModel model)
        {
            string sql = " update public.\"Organization\" set \"Name\" = @CompanyName , \"PersonFullName\" = @CompanyPersonName , \"ContactEmail\" = @CompanyPersonEmail   where \"Id\" = @OrganizationId; "+
                         " update public.\"User\" set \"FirstName\" = @FirstName , \"LastName\" = @LastName , \"Email\" = @UserEmailAddress where \"Id\" = @UserId and \"OrganizationId\" = @OrganizationId ; ";
            _connection.Execute(sql,model);
        }
        public void ChangeActiveStatusOrganization(int organizationId , ActiveStatus activeStatus)
        {
            string sql = "update public.\"Organization\" set \"ActiveStatus\" = @activeStatus where \"Id\" = @organizationId ";
            _connection.Execute(sql , new { activeStatus = activeStatus , organizationId = organizationId });
        }
        public void ChangeActiveStatusUser(int userId)
        {
            string sql = "update public.\"User\" set \"ActiveStatus\" = CASE \"ActiveStatus\" WHEN 0 THEN 1 WHEN 1 THEN 0 ELSE 0 END where \"Id\" = @userId ";
            _connection.Execute(sql, new {  userId = userId });
        }
        public void ChangePassword(int userId , string newPassword)
        {
            string sql = "update public.\"User\" set \"Password\"=@password where \"Id\" = @userId";
            _connection.Execute(sql,new { password = newPassword, userId=userId });
        }
        public List<UserListModel> GetUserList(int skip, int take, int organizationId)
        {
            string sql = "select  count(*) OVER () \"Count\",\"FirstName\" || \"LastName\" \"FullName\",* from public.\"User\" where \"OrganizationId\" = @organizationId " +
 "OFFSET @skip LIMIT @take ";
            return _connection.Query<UserListModel>(sql, new { skip = skip, take = take, organizationId = organizationId }).ToList();
        }
        public UsersInsertModel GetUserById(int userId)
        {
            string sql = "select * from public.\"Users\" where \"Id\"= @userId  ";
            object data = new { userId = userId };
            return _connection.QueryFirstOrDefault(sql, data);
        }
        public int InsertUsers(UsersInsertModel model)
        {
            string sql = "insert into public.\"Users\"(\"UserName\",\"Password\",\"FirstName\",\"LastName\",\"Email\",\"OrganizationId\",\"ActiveStatus\",\"FailedLoginCount\") values (@UserName,@Password,@FirstName,@LastName,@Email,@OrganizationId,@ActiveStatus,@FailedLoginCount) RETURNING \"Id\"";
            return _connection.ExecuteScalar<int>(sql,model);
        }
    }
}
