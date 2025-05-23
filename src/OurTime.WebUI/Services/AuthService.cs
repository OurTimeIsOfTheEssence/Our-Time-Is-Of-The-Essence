// Services/AuthService.cs
using System.Net.Http;
using System.Threading.Tasks;

namespace OurTime.WebUI.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        public AuthService(HttpClient http) => _http = http;

        public async Task<string?> LoginAsync(string user, string pass)
        {
            // skicka login-request
            var resp = await _http.PostAsJsonAsync(
                "/auth/login",
                new { userName = user, password = pass });
            if (!resp.IsSuccessStatusCode)
                return null;

            // läs hela texten
            var body = await resp.Content.ReadAsStringAsync();

            // extrahera allt efter "JWT:"
            const string marker = "JWT:";
            var idx = body.IndexOf(marker, System.StringComparison.OrdinalIgnoreCase);
            if (idx < 0)
                return null;

            return body.Substring(idx + marker.Length).Trim();
        }

        public async Task<string?> GenerateApiKeyAsync(string jwt)
        {
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);

            var resp = await _http.PostAsync("/api-key/generate", null);
            if (!resp.IsSuccessStatusCode)
                return null;

            return (await resp.Content.ReadAsStringAsync()).Trim();
        }
    }
}
