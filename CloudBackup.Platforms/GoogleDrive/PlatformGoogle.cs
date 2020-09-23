using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace CloudBackup.WebPlatforms.GoogleDrive
{
    public class PlatformGoogle : IPlatform
    {
        private string[] _googelDriveScopes;
        private string _googleDriveClientId;
        private string _googleDriveSecretId;
        private string _googleApiKey;
        private string _googleApplicationName;

        Google.Apis.Drive.v3.DriveService s;
        public PlatformGoogle(string webRootPath)
        {
            dynamic config = Common.GetConfig(webRootPath);

            _googleDriveClientId = config.GoogleApiSettings.ClientId.ToString();
            _googleDriveSecretId = config.GoogleApiSettings.SecretId.ToString();
            _googelDriveScopes = config.GoogleApiSettings.Scope.ToString().Split(',');
            _googleApiKey = config.GoogleApiSettings.ApiKey.ToString();
            _googleApplicationName = config.GoogleApiSettings.ApplicationName.ToString();

          
            var services = new DriveService(new BaseClientService.Initializer()
            {
                ApiKey = _googleApiKey,  // from https://console.developers.google.com (Public API access)
                ApplicationName = _googleApplicationName,

            });
            s = services;

        }

        public List<FileListModel> GetDirectoryList(string directory, string accessToken)
        {
           

            FilesResource.ListRequest listRequest = s.Files.List();
            //listRequest.PageSize = 10;

            listRequest.Q = "mimeType = 'application/vnd.google-apps.folder' and '" + directory + "' in parents";
            listRequest.OauthToken = accessToken;

            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
            .Files;

            List<FileListModel> returnModel = new List<FileListModel>();

            foreach (var item in files)
            {
                returnModel.Add(new FileListModel { CloudId = item.Id, Directory = item.Name });
            }

            return returnModel;
        }
        
        public TokenResponse AuthCodeToAccessToken(string code, string redirectUri, string refreshToken)
        {


            string url = "https://www.googleapis.com/oauth2/v4/token";
            string postData = "";
            if (String.IsNullOrEmpty(refreshToken))
                postData = string.Format("code={0}&client_id={1}&client_secret={2}&redirect_uri={3}&grant_type=authorization_code", code, _googleDriveClientId, _googleDriveSecretId, redirectUri);
            else
                postData = string.Format("client_id={0}&client_secret={1}&grant_type=refresh_token&refresh_token={2}", _googleDriveClientId, _googleDriveSecretId, refreshToken);

            WebRequest request = WebRequest.Create(url);

            request.Method = "POST";

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();

            TokenResponse tmp = JsonConvert.DeserializeObject<TokenResponse>(responseFromServer);

            reader.Close();
            dataStream.Close();
            response.Close();
            return tmp;
        }
        public string AuthRequestUrl(string hostUrl)
        {

            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = _googleDriveClientId,
                    ClientSecret = _googleDriveSecretId

                },
                Scopes = _googelDriveScopes,

            });

            var url = flow.CreateAuthorizationCodeRequest("https://" + hostUrl + "/Platform/ReturnGoogleAuth").Build();
            return url.OriginalString;
        }
       
    }
}
