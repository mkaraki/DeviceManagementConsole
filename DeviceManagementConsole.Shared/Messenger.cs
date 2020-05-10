using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DeviceManagementConsole.Shared
{
    public class Messenger
    {
        internal HttpClient HttpClient { get; private set; }

        public string Server { get; private set; }

        public int Port { get; private set; }

        public string AccessBaseUrl { get; private set; }

        public string Unique { get; private set; }

        public static JsonSerializerOptions JsonSerializerOptions { get; private set; } = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
        };

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
                status = status.Trim(' ', '\r', '\n');
                if (res.IsSuccessStatusCode && status == "OK") return true;
                else return false;
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
                var ret = await HttpClient.PutAsync(GetAccessUrl("keepalive"), content);
                return ret.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> GetRemoteTaskJsonAsync()
        {
            try
            {
                var content = await HttpClient.GetAsync(GetAccessUrl("task"));
                await RemoveRemoteTaskAsync();
                string ret = await content.Content.ReadAsStringAsync();
                if (!content.IsSuccessStatusCode || ret == string.Empty)
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
                var ret = await HttpClient.DeleteAsync(GetAccessUrl("task"));
                return ret.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SendReportJsonAsync(string reportjson)
        {
            var content = new StringContent(reportjson, Encoding.UTF8, "application/json");

            try
            {
                var ret = await HttpClient.PostAsync(GetAccessUrl("report"), content);
                return ret.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public bool SendReportJson(string json)
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var ret = HttpClient.PostAsync(GetAccessUrl("report"), content).GetAwaiter().GetResult();
                return ret.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}