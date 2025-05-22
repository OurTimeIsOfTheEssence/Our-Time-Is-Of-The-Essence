using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using OurTime.WebUI.Models.Dtos;

namespace OurTime.WebUI.Services
{
    public class ReviewApiService
    {
        private readonly HttpClient _http;

        public ReviewApiService(HttpClient http) => _http = http;

        public void ConfigureKey(string apiKey)
        {
            _http.DefaultRequestHeaders.Remove("X-Api-Key");
            _http.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
        }

        public void ConfigureBearer(string jwt)
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", jwt);
        }

        public async Task<ProductDto> RegisterProductAsync(ProductRequestDto dto)
        {
            var resp = await _http.PostAsJsonAsync("/product/save", dto);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<ProductDto>()!;
        }

        public Task<IEnumerable<ReviewDto>> GetReviewsAsync(int productId) =>
            _http.GetFromJsonAsync<IEnumerable<ReviewDto>>($"/api/product/{productId}/review")
                 ?? Task.FromResult<IEnumerable<ReviewDto>>(new List<ReviewDto>());

        public async Task<ReviewDto?> PostReviewAsync(int productId, CreateReviewDto dto)
        {
            var resp = await _http.PostAsJsonAsync($"/api/product/{productId}/review", dto);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<ReviewDto>();
        }
    }
}
