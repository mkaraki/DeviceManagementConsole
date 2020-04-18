using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;

namespace DeviceManagementConsole.Shared
{
    public class Messenger
    {
        internal HttpClient HttpClient { get; private set; }

        public string Server { get; private set; }

        public int Port { get; private set; }

        public string AccessBaseUrl { get; private set; }

        public string Unique { get; private set; }

        public Messenger(string Server, int Port, string Unique)
        {
            this.Server = Server;
            this.Port = Port;

            this.Unique = Unique;

            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders
                .Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders
                .Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", EncodeBase64(Unique));

            AccessBaseUrl = $"http://{Server}:{Port}/";
        }

        internal string GetAccessUrl(string service)
            => AccessBaseUrl + service + ".php";

        internal string EncodeBase64(string original, Encoding encode = null)
        {
            if (encode == null) encode = Encoding.UTF8;

            return Convert.ToBase64String(encode.GetBytes(original));
        }

        internal string DecodeBase64(string base64, Encoding encode = null)
        {
            if (encode == null) encode = Encoding.UTF8;

            return encode.GetString(Convert.FromBase64String(base64));
        }

        public async Task<bool> CheckConnectionAsync()
        {
            try
            {
                HttpResponseMessage res = await HttpClient.GetAsync(GetAccessUrl("test"));
                string status = await res.Content.ReadAsStringAsync();
                if (status == "OK") return true;
                else return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SendKeepaliveAsync()
        {
            var content = new StringContent("keepalive");

            try
            {
                await HttpClient.PutAsync(GetAccessUrl("keepalive"), content);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<string> GetRemoteTaskJsonAsync()
        {
            try
            {
                var content = await HttpClient.GetAsync(GetAccessUrl("task"));
                await RemoveRemoteTaskAsync();
                string ret = await content.Content.ReadAsStringAsync();
                if (ret == string.Empty)
                    return null;
                return ret;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> RemoveRemoteTaskAsync()
        {
            try
            {
                await HttpClient.DeleteAsync(GetAccessUrl("task"));
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<bool> SendReportJsonAsync(string reportjson)
        {
            var content = new StringContent(reportjson, Encoding.UTF8, "application/json");

            try
            {
                await HttpClient.PostAsync(GetAccessUrl("report"), content);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
