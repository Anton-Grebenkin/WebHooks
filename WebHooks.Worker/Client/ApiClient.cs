using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BonusPlus.WebHook.Client
{

    public class ApiClient
    {
        private HttpClient _HttpClient;
        public ApiClient()
        {
            _HttpClient = new HttpClient();
        }

        public async Task<HttpResponseMessage> SendRequest(string url, string httpMethod, string secretKey = null, string contentType = null, string body = null)
        {
            using (var request = new HttpRequestMessage(new HttpMethod(httpMethod), url))
            {
                if (body != null)
                {
                    //string json = JsonConvert.SerializeObject(param);
                    var stringContent = new StringContent(body, Encoding.UTF8, contentType);
                    request.Content = stringContent;
                }
                if (!string.IsNullOrEmpty(secretKey))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer ", secretKey);
                }
               
                var response = await _HttpClient.SendAsync(request);
                return response;
            }
        }

        public string GetResponseContent(HttpResponseMessage response)
        {
            string body = response.Content.ReadAsStringAsync().Result;
            string header = response.Headers.ToString();

            return String.Format("Header: {0}\nBody: {1}", header, body);
        }

    }
}
