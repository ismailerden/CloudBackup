using System;
using System.Collections.Generic;
using System.Text;

namespace CloudBackup.WebPlatforms
{
    public interface IPlatform
    {
        List<FileListModel> GetDirectoryList(string directory,string accessToken);
    }
}
