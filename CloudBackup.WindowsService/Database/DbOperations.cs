using CloudBackup.WindowsService.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.IO;

namespace CloudBackup.WindowsService
{
    public class DbOperations
    {
        SqlConnection _cnn;
        public DbOperations()
        {
            String path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            path = System.IO.Path.GetDirectoryName(path);
            _cnn = new SqlConnection(@"Server=(localdb)\v11.0;Integrated Security=true;AttachDbFileName=" + path + @"\Database\BackupDatabase.mdf");
        }
        public void InsertData(CloudFile ff)
        {
            string sql = "insert into CloudFile(DevicePlanId,SubDirectory,FullName,Type,ProccessDate,ResultStatus,ErrorMessage,CloudId,Length) values (@DevicePlanId,@SubDirectory,@FullName,@Type,@ProccessDate,@ResultStatus,@ErrorMessage,@CloudId,@Length)";
            _cnn.Execute(sql,ff);
        }
        public List<CloudFile> GetFileList(int devicePlanId , string directory)
        {
            string sql = "select * from CloudFile where SubDirectory=@directory and DevicePlanId=@devicePlanId";
            object data = new { directory = directory , devicePlanId = devicePlanId };
            return _cnn.Query<CloudFile>(sql,data).ToList();
        }
        public List<CloudFile> GetErrorFileList(int devicePlanId , DateTime ProcessDate)
        {
            string sql = "select * from CloudFile where  DevicePlanId=@devicePlanId and ProccessDate > @datetime and ResultStatus=-1";
            object data = new { datetime = ProcessDate , devicePlanId=devicePlanId };
            return _cnn.Query<CloudFile>(sql, data).ToList();
        }

        public void UpdateData(CloudFile ff)
        {
            string sql = "update CloudFile set Length=@Length , ErrorMessage=@ErrorMessage , ProccessDate=@ProccessDate , ResultStatus=@ResultStatus  where Id=@Id ";
            _cnn.Execute(sql, ff);
        }
        public void DeleteData(CloudFile ff)
        {
            string sql = "delete from CloudFile where Id=@Id ";
            _cnn.Execute(sql, ff);
        }
    }
}
