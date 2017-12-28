using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Security.Authentication;
using System.Net.Http.Headers;
using System.Net;

namespace android_photo_syncr.Tools
{
    class RESTClient
    {
        public bool TestConnection(string url)
        {
            bool isLive = false;
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response?.StatusCode == HttpStatusCode.OK)
                {
                    isLive = true;
                }
            }
            finally { }
            return isLive;
        }

        public async Task<string> AsyncMultipartPost(string authHeader, string url, MultipartFormDataContent package)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.MaxResponseContentBufferSize = 256000;
                    var uri = new Uri(string.Format(url.TrimEnd('/'), string.Empty));
                    var response = await client.PostAsync(uri, package);
                    string content = "no good";
                    if (response.IsSuccessStatusCode)
                    {
                        content = await response.Content.ReadAsStringAsync();
                    }
                    client.DefaultRequestHeaders.Accept.Clear();
                    return content;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}