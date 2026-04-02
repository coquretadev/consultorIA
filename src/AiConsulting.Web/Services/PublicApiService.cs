using System.Net.Http.Json;
using System.Text.Json;
using AiConsulting.Web.Models;

namespace AiConsulting.Web.Services;

public class PublicApiService : IPublicApiService
{
    private readonly HttpClient _http;
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public PublicApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ServiceSummaryModel>> GetServicesAsync()
    {
        try
        {
            var result = await _http.GetFromJsonAsync<List<ServiceSummaryModel>>(
                "/api/public/services", _jsonOptions);
            return result ?? [];
        }
        catch
        {
            return [];
        }
    }

    public async Task<ServiceDetailModel?> GetServiceByIdAsync(Guid id)
    {
        try
        {
            var response = await _http.GetAsync($"/api/public/services/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ServiceDetailModel>(_jsonOptions);
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<CaseStudyModel>> GetCaseStudiesAsync()
    {
        try
        {
            var result = await _http.GetFromJsonAsync<List<CaseStudyModel>>(
                "/api/public/case-studies", _jsonOptions);
            return result ?? [];
        }
        catch
        {
            return [];
        }
    }

    public async Task<List<SectorModel>> GetSectorsAsync()
    {
        try
        {
            var result = await _http.GetFromJsonAsync<List<SectorModel>>(
                "/api/public/sectors", _jsonOptions);
            return result ?? [];
        }
        catch
        {
            return [];
        }
    }

    public async Task<ContactRequestResultModel?> SubmitContactAsync(ContactRequestModel dto)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("/api/public/contact", dto, _jsonOptions);
            if (!response.IsSuccessStatusCode)
                return null;
            return await response.Content.ReadFromJsonAsync<ContactRequestResultModel>(_jsonOptions);
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<AvailableSlotModel>> GetAvailableSlotsAsync(DateOnly date)
    {
        try
        {
            var result = await _http.GetFromJsonAsync<List<AvailableSlotModel>>(
                $"/api/public/availability?date={date:yyyy-MM-dd}", _jsonOptions);
            return result ?? [];
        }
        catch
        {
            return [];
        }
    }

    public async Task<BookingResultModel?> BookSlotAsync(BookSlotModel dto)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("/api/public/book", dto, _jsonOptions);
            if (!response.IsSuccessStatusCode)
                return null;
            return await response.Content.ReadFromJsonAsync<BookingResultModel>(_jsonOptions);
        }
        catch
        {
            return null;
        }
    }
}
