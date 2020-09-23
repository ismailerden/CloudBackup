using CloudBackup.Database.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;

namespace CloudBackup.Database.Operations
{
    public class BackupLogOperations : BaseOperations
    {
        public BackupLogOperations(string connectionString) : base(connectionString)
        {

        }
        public int InsertBackupLog(BackupLog model)
        {
            model.CreatedDate = DateTime.Now;
            string sql = "insert into " +
" public.\"BackupLog\" (\"CreateFileCount\",\"FailedCount\",\"CreateDirectoryCount\",\"ProccessCount\",\"TotalSize\",\"DevicePlanId\",\"OrganizationId\",\"DeletedCount\" , \"CreatedDate\" , \"UpdatedCount\" ) " +
" values(@CreateFileCount, @FailedCount, @CreateDirectoryCount, @ProccessCount, @TotalSize, @DevicePlanId, @OrganizationId,@DeletedCount,@CreatedDate,@UpdatedCount)   RETURNING \"Id\" ";
            
            return _connection.ExecuteScalar<int>(sql, model);
        }
    }
}
