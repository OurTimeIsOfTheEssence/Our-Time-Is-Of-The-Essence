using System.Net.Http;
using System.Net.Http.Json;
using OurTime.WebUI.Models.Dtos;

namespace OurTime.WebUI.Services;

public class ReviewApiService
{
    private readonly HttpClient _http;
    public ReviewApiService(HttpClient http) => _http = http;

    public Task<IEnumerable<ReviewDto>> GetReviewsAsync(int productId) =>
        _http.GetFromJsonAsync<IEnumerable<ReviewDto>>($"/api/product/review/{productId}")
             ?? Task.FromResult(Enumerable.Empty<ReviewDto>());

    public async Task<ReviewDto?> PostReviewAsync(int productId, CreateReviewDto dto)
    {
        var resp = await _http.PostAsJsonAsync($"/api/product/{productId}/review", dto);
        resp.EnsureSuccessStatusCode();
        return await resp.Content.ReadFromJsonAsync<ReviewDto>();
    }
}
