using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CloudBackup.WindowsService
{
    public static class Common
    {
      
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
        public static void InsertApiAndSecretKey(string apiKey, string secretKey)
        {
            RegistryKey rb = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
           
             var registreyEntry =   rb.CreateSubKey(@"SOFTWARE\CloudBackupApp", RegistryKeyPermissionCheck.ReadWriteSubTree);
            //RegistryKey rb2 = rb.OpenSubKey(@"SOFTWARE\CloudBackupApp");

            registreyEntry.SetValue("ApiAndSecret", apiKey + "|" + secretKey);

        }
       
        public static KeyValuePair<string, string> ApiAndSecretKey()
        {
            RegistryKey rb = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey rb2 = rb.OpenSubKey(@"SOFTWARE\CloudBackupApp");


            if (rb2!=null && rb2.GetValue("ApiAndSecret") != null && rb2.GetValue("ApiAndSecret").ToString().Split('|').Count() == 2)
            {
                return new KeyValuePair<string, string>(rb2.GetValue("ApiAndSecret").ToString().Split('|')[0], rb2.GetValue("ApiAndSecret").ToString().Split('|')[1]);
            }
            else
                return new KeyValuePair<string, string>("", "");

        }
        public static string SecurityKey()
        {
           
         
            var mbs = new ManagementObjectSearcher("Select * From Win32_processor");
            ManagementObjectCollection mbsList = mbs.Get();
            string cpuId = "";
            foreach (ManagementObject mo in mbsList)
            {
                cpuId = mo["ProcessorId"].ToString();
                break;
            }
            mbs.Dispose();
            mbsList.Dispose();

            string drive = "";
            //Find first drive
            foreach (DriveInfo compDrive in DriveInfo.GetDrives())
            {
                if (compDrive.IsReady)
                {
                    drive = compDrive.RootDirectory.ToString();
                    break;
                }
            }

            if (drive.EndsWith(":\\"))
            {
                //C:\ -> C
                drive = drive.Substring(0, drive.Length - 2);
            }

            ManagementObject disk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + drive + @":""");
            disk.Get();

            string volumeSerial = disk["VolumeSerialNumber"].ToString();
            disk.Dispose();

            String firstMacAddress = NetworkInterface
     .GetAllNetworkInterfaces()
     .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
     .Select(nic => nic.GetPhysicalAddress().ToString())
     .FirstOrDefault();

            return cpuId + "|" + volumeSerial + "|" + firstMacAddress + "|";

        }
        public static void InstallMeOnStartUp()
        {
            try
            {
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                Assembly curAssembly = Assembly.GetExecutingAssembly();
                key.SetValue(curAssembly.GetName().Name, curAssembly.Location);
            }
            catch { }
        }
    }
}
