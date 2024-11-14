using System.Net.Http.Headers;

namespace EcoBalance.Services
{
    public class ThingerService
    {
        private readonly HttpClient _httpClient;

        public ThingerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetDataFromThinger(string token)
        {
            string endpoint = "https://backend.thinger.io/v3/users/{id_user}/devices/{device_name}/callback/data";

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
