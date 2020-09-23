using CloudBackup.WindowsService;
using Microsoft.Win32;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CloudBackup.WindowsAppControlService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();


           
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            SetServiceStatus();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            ServiceController sc = new ServiceController("ECloudBackup Service", System.Environment.MachineName);
            if (sc.Status == ServiceControllerStatus.Stopped)
            { 
                sc.Start();
                SetServiceStatus();
            }
            else
            {
                SetServiceStatus();
            }
            

        }
        public void SetServiceStatus()
        {
            try
            {
                ServiceController sc = new ServiceController("ECloudBackup Service");

                if (sc.Status == ServiceControllerStatus.Running)
                {
                    lblServiceStatus.Content = "Servis çalışıyor";
                    lblServiceStatus.Foreground = Brushes.Green;
                }
                else if (sc.Status == ServiceControllerStatus.ContinuePending)
                {
                    lblServiceStatus.Content = "Servis durdurulmuş ve başlatılıyor";
                    lblServiceStatus.Foreground = Brushes.Green;
                }
                else if (sc.Status == ServiceControllerStatus.Paused)
                {
                    lblServiceStatus.Content = "Servis durdurulmuş";
                    lblServiceStatus.Foreground = Brushes.Yellow;
                }
                else if (sc.Status == ServiceControllerStatus.PausePending)
                {
                    lblServiceStatus.Content = "Servis durduruluyor";
                    lblServiceStatus.Foreground = Brushes.Yellow;
                }
                else if (sc.Status == ServiceControllerStatus.StartPending)
                {
                    lblServiceStatus.Content = "Servis başlatılıyor";
                    lblServiceStatus.Foreground = Brushes.Yellow;
                }
                else if (sc.Status == ServiceControllerStatus.Stopped)
                {
                    lblServiceStatus.Content = "Servis çalışmıyor";
                    lblServiceStatus.Foreground = Brushes.Red;
                }
                else if (sc.Status == ServiceControllerStatus.StopPending)
                {
                    lblServiceStatus.Content = "Servis durduruluyor";
                    lblServiceStatus.Foreground = Brushes.Yellow;
                }
            }
            catch
            {
                lblServiceStatus.Content = "Servis bulunamadı";
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {



            SetServiceStatus();
            var result = Common.ApiAndSecretKey();
            txtApiKey.Text = result.Key;
            txtSecretKey.Text = result.Value;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            SetServiceStatus();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            ServiceController sc = new ServiceController("ECloudBackup Service");
            if (sc.Status == ServiceControllerStatus.Running)
                sc.Stop();
            
        }

        private void btnSaveKey_Click(object sender, RoutedEventArgs e)
        {
            Common.InsertApiAndSecretKey(txtApiKey.Text, txtSecretKey.Text);

            string[] deviceInfo = Common.SecurityKey().Split('|');
            CheckModel model = new CheckModel();
            model.apiAccessKey = txtApiKey.Text;
            model.apiSecretKey = txtSecretKey.Text;
            model.cpuId = deviceInfo[0];
            model.diskVolume = deviceInfo[1];
            model.macAddress = deviceInfo[2];
            model.updateData = true;

            RestClient client = new RestClient("https://api.ebackup.cloud");

            IRestRequest request = new RestRequest("api/device/check", Method.POST);
            request.RequestFormat = RestSharp.DataFormat.Json;
            request.AddBody(model);
            TaskCompletionSource<IRestResponse> taskCompletion = new TaskCompletionSource<IRestResponse>();
            RestRequestAsyncHandle handle = client.ExecuteAsync(request, r => { taskCompletion.SetResult(r); });
            IRestResponse response = taskCompletion.Task.Result;

            ServiceController sc = new ServiceController("ECloudBackup Service");

            if (sc.Status == ServiceControllerStatus.Running)
            {
                sc.Stop();
                while(new ServiceController("ECloudBackup Service").Status != ServiceControllerStatus.Stopped)
                {
                    Thread.Sleep(1000);
                }
                sc.Start();
                SetServiceStatus();
            }
            else if (sc.Status == ServiceControllerStatus.Stopped)
            {
                sc.Start();
                SetServiceStatus();
            }



            MessageBox.Show("Bilgiler Kaydedilmiştir. Bu aşamada Sunucuda Doğrulama Yapılacaktır. Doğrulamadan Sonra Keyler Girilmelidir. ");



        }
    }
}
