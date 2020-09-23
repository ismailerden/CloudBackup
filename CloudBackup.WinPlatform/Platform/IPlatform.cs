using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudBackup.WinPlatform.Platform
{
    public interface IPlatform
    {
        List<KeyValuePair<string, object>> ValueList { get; set; }
        string UploadFile(string fileName, string filePath, string fileType, string directoryId);
        string UpdateFile(string fileName, string filePath, string fileType, string directoryId, string fileId);
        bool CheckAccessTokenExpire();
        void UpdateAccessToken(string accessToken, DateTime expireDate);
        void DeleteFile(string fileId);
        string CreateDirectory(string folderName, string parent);
    }
}
