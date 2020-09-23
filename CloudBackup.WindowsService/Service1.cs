using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CloudBackup.WindowsService
{
    public partial class ECloudBackupService : ServiceBase
    {
        public ECloudBackupService()
        {
            InitializeComponent();
        }
        public void WriteError(string error)
        {
            StreamWriter Dosya = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + "log.txt");
            Dosya.WriteLine(error + "  Tarih: " + DateTime.Now.ToString());
            Dosya.Close();
        }
        protected override void OnStart(string[] args)
        {
            try
            {
                RabitMqReciever rb = new RabitMqReciever();
                KeyValuePair<string, string> apiKey = Common.ApiAndSecretKey();
                rb.ReadQue(apiKey.Key, apiKey.Value);
            }
            catch(Exception ex)
            {
                WriteError(ex.Message);   
            }
        }

        protected override void OnStop()
        {

        }
    }
}
