using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace OurTime.WebUI.Services
{
    public class ReviewApiAuthHandler : DelegatingHandler
    {
        private readonly AuthService _auth;
        private string? _jwt;
        private string? _apiKey;

        public ReviewApiAuthHandler(AuthService auth) => _auth = auth;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
        {
            if (_jwt == null)
            {
                _jwt = await _auth.LoginAsync(
                              Environment.GetEnvironmentVariable("REVIEW_ENGINE_USER")!,
                              Environment.GetEnvironmentVariable("REVIEW_ENGINE_PASSWORD")!);
                _apiKey = _jwt != null
                          ? await _auth.GenerateApiKeyAsync(_jwt)
                          : null;
            }

            if (!string.IsNullOrWhiteSpace(_jwt))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _jwt);

            if (!string.IsNullOrWhiteSpace(_apiKey))
                request.Headers.Add("X-API-KEY", _apiKey);

            return await base.SendAsync(request, ct);
        }
    }
}
