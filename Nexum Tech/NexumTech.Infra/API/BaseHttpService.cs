
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NexumTech.Infra.API.Interfaces;
using NexumTech.Infra.WEB;

namespace NexumTech.Infra.API
{
    public class BaseHttpService : IBaseHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly AppSettingsWEB _appSettingsUI;
        
        public BaseHttpService(HttpClient httpClient, IOptions<AppSettingsWEB> appSettingsUI)
        {
            _httpClient = httpClient;
            _appSettingsUI = appSettingsUI.Value;
        }

        public async Task<T> CallMethod<T>(string url, HttpMethod method, string token = null, object data = null, Dictionary<string, string> headers = null)
        {
            try
            {
                string completeURL = String.Concat(_appSettingsUI.ApiBaseURL, url.Trim());

                HttpRequestMessage request = new HttpRequestMessage(method, completeURL);

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        request.Headers.Add(header.Key, header.Value);
                    }
                }

                if (token != null) 
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                if (data != null)
                {
                    string jsonData = JsonConvert.SerializeObject(data);
                    request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                }

                HttpResponseMessage response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    if (typeof(T) == typeof(string))
                    {
                        return (T)(object)responseBody;
                    }

                    return JsonConvert.DeserializeObject<T>(responseBody);
                }
                else
                {
                    throw new Exception(response.Content.ReadAsStringAsync().Result);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"API Error: {ex.Message}");
            }
        }
    }
}
