
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NexumTech.Infra.WEB;

namespace NexumTech.Infra.API
{
    public class BaseHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly AppSettingsWEB _appSettingsUI;
        
        public BaseHttpService(HttpClient httpClient, IOptions<AppSettingsWEB> appSettingsUI)
        {
            _httpClient = httpClient;
            _appSettingsUI = appSettingsUI.Value;
        }

        public async Task<T> CallMethod<T>(string url, HttpMethod method, object data = null)
        {
            try
            {
                string completeURL = String.Concat(_appSettingsUI.ApiBaseURL, url.Trim());

                HttpRequestMessage request = new HttpRequestMessage(method, completeURL);

                if (data != null)
                {
                    string jsonData = JsonConvert.SerializeObject(data);
                    request.Content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                }

                HttpResponseMessage response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(responseBody);
                }
                else
                {
                    throw new Exception($"API Error: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"API Error: {ex.Message}");
            }
        }
    }
}
