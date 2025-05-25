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

        public ReviewApiService(HttpClient http)
            => _http = http;

        public async Task<IEnumerable<ReviewDto>> GetReviewsAsync(int extProductId)
        {
            // Hämta hela produkten, som i JSON innehåller en "reviews"-lista
            var product = await _http.GetFromJsonAsync<ProductDto>($"/product/{extProductId}");
            return product?.Reviews ?? new List<ReviewDto>();
        }

        public async Task<bool> PostReviewAsync(int extProductId, CreateReviewDto dto)
        {
            var response = await _http.PostAsJsonAsync($"/product/{extProductId}/review", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<int?> RegisterAndReturnIdAsync(ProductRequestDto dto)
        {
            var response = await _http.PostAsJsonAsync("/product/save", dto);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<int>();
        }
    }
}
