using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CloudBackup.WebApp.Core
{
    public static class WebUtilities
    {
        private static string _idEncryptKey = "dGrtY4&%%+1d+rtYr";
        private static string _deviceEncryptKey = "dVVvdf+564Mdb3B%ejw";
        public static string InsertNotification(string message)
        {
            return "bootbox.alert('" + message + "')";
        }
        private static string EncryptString(string text, string keyString)
        {
            keyString = EncodeTo64(keyString);
            if (keyString.Length < 32)
            {
                int incCount = 32 - keyString.Length;
                for (int i = 0; i < 32 - incCount; i++)
                    keyString += "=";
            }
            keyString = keyString.Substring(0, 32);
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }
        private static string DecryptString(string cipherText, string keyString)
        {
            keyString = EncodeTo64(keyString);
            if (keyString.Length < 32)
            {
                int incCount = 32 - keyString.Length;
                for (int i = 0; i < 32 - incCount; i++)
                    keyString += "=";
            }
            keyString = keyString.Substring(0, 32);

            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }
        public static string EncryptId(int id, string userName, int organizationId)
        {
            return WebUtility.UrlEncode(EncryptString(id.ToString(), userName + organizationId + _idEncryptKey));
        }
        public static int DecryptId(string hashedId, string userName, int organizationId)
        {
            //hashedId = WebUtility.UrlDecode(hashedId);
            int id = 0;
            try
            {
                string value = DecryptString(hashedId, userName + organizationId + _idEncryptKey);
                int.TryParse(value, out id);
                return id;
            }
            catch
            {
                return id;
            }
        }
        private static string EncodeTo64(string toEncode)

        {

            byte[] toEncodeAsBytes

                  = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);

            string returnValue

                  = System.Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;

        }
        private static string DecodeFrom64(string encodedData)

        {

            byte[] encodedDataAsBytes

                = System.Convert.FromBase64String(encodedData);

            string returnValue =

               System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);

            return returnValue;

        }
        public static string EncryptDeviceInformation(string data)
        {
            return EncryptString(data,_deviceEncryptKey);
        }
        public static string DecryptDeviceInformation(string data)
        {
            return DecryptString(data, _deviceEncryptKey);
        }
        public static string SecurityKey(string apiAccessKey , string apiSecretKey,string cpuId,string macAddress,string diskId )
        {

            string apiKey = apiAccessKey;
            string apiSecret = apiSecretKey;
            string volumeSerial = diskId;
            string firstMacAddress = macAddress;

            return cpuId + "|" + volumeSerial + "|" + firstMacAddress + "|" + MD5Hash(apiKey + "|" + apiSecret);

        }
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
        public static DateTime ConvertDatabaseDateTime(string dbDate)
        {
            string[] dateOrTime = dbDate.Split(' ');
            string date = dateOrTime[0];
            string[] dateParse = date.Split('.');
            string time = dateOrTime[1];
            string[] timeParse = time.Split(':');
            return new DateTime(Convert.ToInt32(dateParse[2]), Convert.ToInt32(dateParse[1]), Convert.ToInt32(dateParse[0]), Convert.ToInt32(timeParse[0]), Convert.ToInt32(timeParse[1]), Convert.ToInt32(timeParse[2]));
        }
        public static dynamic GetConfig(string rootPath)
        {
            var JSON = System.IO.File.ReadAllText(rootPath + "/appsettings.json");
            return JObject.Parse(JSON);

        }
    }
}
