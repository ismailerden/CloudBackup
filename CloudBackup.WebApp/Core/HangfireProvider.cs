using CloudBackup.Database.Operations;
using CloudBackup.Database.ViewModel.CreateModel;
using Hangfire;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudBackup.WebApp.Core
{
    public static class HangfireProvider
    {

        public static void SendBackupJob(int devicePlanJobId, int organizationId)
        {
            SendQueue(devicePlanJobId, "B", organizationId);
        }
        public static void SendFileListCommand(int deviceId, int organizationId, string directory)
        {
            SendQueue(deviceId, "F", organizationId, directory);
        }
        private static void SendQueue(int deviceId, string type, int organizationId, string directory = "")
        {
            DpOperations operations = new DpOperations();
            int devicePlanId = 0;
            if (type == "B")
                devicePlanId = deviceId;

            DeviceInsertModel mdl;
            if (devicePlanId != 0)
                mdl = operations.Device.GetDeviceById(organizationId, operations.DevicePlan.GetDevicePlanById(organizationId, devicePlanId).DeviceId);
            else
                mdl = operations.Device.GetDeviceById(organizationId, deviceId);


            //Declare queue name. This can be anything you like
            string QueueName = WebUtilities.SecurityKey(mdl.ApiAccessKey, mdl.ApiSecretKey, mdl.CpuId, mdl.MacAddress, mdl.DiskSeriNo);
            // Create a new connection factory for the queue
            var factory = new ConnectionFactory();
            factory.UserName = "mquser";
            factory.Password = "mq321user";
            // Because Rabbit is installed locally, we can run it on localhost
            factory.HostName = "178.128.207.112";
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                // mark all messages as persistent
                const bool durable = false;
              
                channel.QueueDeclare(QueueName, durable, false, false, null);

                // Set delivery mode (1 = non Persistent | 2 = Persistent)
                IBasicProperties props = channel.CreateBasicProperties();
                props.DeliveryMode = 2;
                props.Expiration = "3600000";

                string message = type + "|" + (devicePlanId == 0 ? deviceId : devicePlanId) + "|" + directory;
                message = WebUtilities.EncryptDeviceInformation(message);
                
                byte[] body = System.Text.Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("", QueueName, props, body);
            }
        }
        public static void CheckDeviceOnline(int deviceId, int organizationId)
        {
            SendQueue(deviceId, "C", organizationId);
        }
        public static void InsertJobOneTimeBackup(DateTime dtInsertDate, int devicePlanJobId, int deviceId, int organizationId)
        {
            DpOperations operations = new DpOperations();
            var jobId = BackgroundJob.Schedule(
     () => HangfireProvider.SendBackupJob(devicePlanJobId, organizationId),
     dtInsertDate);
            operations.Device.UpdateDevicePlanBackupJobId(devicePlanJobId, "S-B-" + jobId, organizationId);
            // return "S-B-" + jobId;
        }
        public static void ScheduleRecurringBackupJob(DateTime dtInsertDate, int devicePlanJobId, string cron, int deviceId, int organizationId)
        {
            DpOperations operations = new DpOperations();
            var jobId = BackgroundJob.Schedule(
   () => HangfireProvider.InsertJobTimePlanBackup(devicePlanJobId, cron, deviceId, organizationId),
   dtInsertDate);
            operations.Device.UpdateDevicePlanBackupJobId(devicePlanJobId, "S-B-" + jobId, organizationId);
            // return "S-B-" + jobId;
        }
        public static void InsertJobTimePlanBackup(int devicePlanJobId, string cron, int deviceId, int organizationId)
        {
            DpOperations operations = new DpOperations();
            RecurringJob.AddOrUpdate("R-B-" + devicePlanJobId.ToString(),
     () => HangfireProvider.SendBackupJob(devicePlanJobId, organizationId), cron
     );
            operations.Device.UpdateDevicePlanBackupJobId(devicePlanJobId, "R-B-" + devicePlanJobId.ToString(), organizationId);
            // return "R-B-" + devicePlanJobId.ToString();
        }
        public static void InsertJobOneTimeFileList(DateTime dtInsertDate, int deviceId, int organizationId)
        {
            DpOperations operations = new DpOperations();
            var jobId = BackgroundJob.Schedule(
     () => HangfireProvider.SendFileListCommand(deviceId, organizationId, ""),
     dtInsertDate);
            operations.Device.UpdateDeviceFileListJobId(deviceId, "S-F-" + jobId, organizationId);
            // return "S-F-" + jobId;
        }
        public static void ScheduleRecurringFileList(DateTime dtInsertDate, int deviceId, string cron, int organizationId)
        {
            DpOperations operations = new DpOperations();
            var jobId = BackgroundJob.Schedule(
   () => HangfireProvider.InsertJobTimeFileList(deviceId, cron, organizationId),
   dtInsertDate);
            operations.Device.UpdateDeviceFileListJobId(deviceId, "S-F-" + jobId, organizationId);
            // return "S-F-" + jobId;
        }
        public static void InsertJobTimeFileList(int deviceId, string cron, int organizationId)
        {
            DpOperations operations = new DpOperations();
            RecurringJob.AddOrUpdate("R-F-" + deviceId.ToString(),
    () => HangfireProvider.SendFileListCommand(deviceId, organizationId, ""), cron
    );
            operations.Device.UpdateDeviceFileListJobId(deviceId, "R-F-" + deviceId.ToString(), organizationId);
            // return "R-F-" + devicePlanJobId.ToString();
        }
        public static void InsertJobOneTimeCheckOnline(DateTime dtInsertDate, int deviceId, int organizationId)
        {
            DpOperations operations = new DpOperations();
            var jobId = BackgroundJob.Schedule(
     () => HangfireProvider.CheckDeviceOnline(deviceId, organizationId),
     dtInsertDate);
            operations.Device.UpdateDeviceCheckStatusJobId(deviceId, "S-C-" + jobId, organizationId);
            // return "S-F-" + jobId;
        }
        public static void ScheduleRecurringCheckOnline(DateTime dtInsertDate, int deviceId, string cron, int organizationId)
        {
            DpOperations operations = new DpOperations();
            var jobId = BackgroundJob.Schedule(
   () => HangfireProvider.InsertJobTimeCheckOnline(deviceId, cron, organizationId),
   dtInsertDate);
            operations.Device.UpdateDeviceCheckStatusJobId(deviceId, "S-C-" + jobId, organizationId);
            // return "S-F-" + jobId;
        }
        public static void InsertJobTimeCheckOnline(int deviceId, string cron, int organizationId)
        {
            DpOperations operations = new DpOperations();
            RecurringJob.AddOrUpdate("R-C-" + deviceId,
    () => HangfireProvider.CheckDeviceOnline(deviceId, organizationId), cron
    );
            operations.Device.UpdateDeviceCheckStatusJobId(deviceId, "R-C-" + deviceId, organizationId);
            // return "R-F-" + devicePlanJobId.ToString();
        }
        public static void DeleteHangFireJobs(List<string> ids)
        {
            foreach (var item in ids)
            {
                if (!String.IsNullOrEmpty(item))
                {
                    if (item.StartsWith("S-"))
                    {
                        string id = item.Substring(4, item.Length - 4);
                        BackgroundJob.Delete(id);
                    }
                    else if (item.StartsWith("R-"))
                    {
                        //string id = item.Substring(4, item.Length - 4);
                        RecurringJob.RemoveIfExists(item);
                    }
                }
            }
        }
    }
}
