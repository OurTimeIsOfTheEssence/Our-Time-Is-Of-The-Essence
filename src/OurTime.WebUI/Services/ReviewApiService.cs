using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using OurTime.WebUI.Models.Dtos;

namespace OurTime.WebUI.Services
{
    public class ReviewApiService
    {
        private readonly HttpClient _http;
        public ReviewApiService(HttpClient http) => _http = http;

        /// Hämtar alla recensioner för en extern produkt via ?productId=…

        public async Task<IEnumerable<ReviewDto>> GetReviewsAsync(int extProductId)
        {
            // OBS! Matcha externa swagger‐definitionen:
            // GET /api/product/reviews?productId={extProductId}
            var uri = $"api/product/reviews?productId={extProductId}";
            var reviews = await _http.GetFromJsonAsync<IEnumerable<ReviewDto>>(uri);
            return reviews ?? new ReviewDto[0];
        }

        /// Skapar en ny recension på /api/product/{id}/review
        public async Task<bool> PostReviewAsync(int extProductId, CreateReviewDto dto)
        {
            var uri = $"api/product/{extProductId}/review";
            var resp = await _http.PostAsJsonAsync(uri, dto);
            return resp.IsSuccessStatusCode;
        }

        /// Registrerar en produkt: POST /api/product/save  (eller enligt extern swagger)
        public async Task<int?> RegisterAndReturnIdAsync(ProductRequestDto dto)
        {
            var resp = await _http.PostAsJsonAsync("api/product/save", dto);
            if (!resp.IsSuccessStatusCode) return null;
            return await resp.Content.ReadFromJsonAsync<int>();
        }
    }
}
