using System.Net.Http.Json;
using System.Text.Json;
using AiConsulting.Web.Models.Panel;
using Microsoft.AspNetCore.Components;

namespace AiConsulting.Web.Services;

public class PanelApiService : IPanelApiService
{
    private readonly HttpClient _http;
    private readonly IAuthApiService _auth;
    private readonly NavigationManager _navigation;
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public PanelApiService(HttpClient http, IAuthApiService auth, NavigationManager navigation)
    {
        _http = http;
        _auth = auth;
        _navigation = navigation;
    }

    private async Task<bool> AddAuthHeaderAsync()
    {
        var token = await _auth.GetTokenAsync();
        if (string.IsNullOrEmpty(token))
        {
            _navigation.NavigateTo("/panel/login");
            return false;
        }
        _http.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        return true;
    }

    private async Task<T?> GetAsync<T>(string url)
    {
        if (!await AddAuthHeaderAsync()) return default;
        try
        {
            var response = await _http.GetAsync(url);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _navigation.NavigateTo("/panel/login");
                return default;
            }
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
        }
        catch { return default; }
    }

    private async Task<T?> PostAsync<T>(string url, object? body = null)
    {
        if (!await AddAuthHeaderAsync()) return default;
        try
        {
            var response = body is null
                ? await _http.PostAsync(url, null)
                : await _http.PostAsJsonAsync(url, body, _jsonOptions);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _navigation.NavigateTo("/panel/login");
                return default;
            }
            if (!response.IsSuccessStatusCode) return default;
            return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
        }
        catch { return default; }
    }

    private async Task PostVoidAsync(string url, object? body = null)
    {
        if (!await AddAuthHeaderAsync()) return;
        try
        {
            var response = body is null
                ? await _http.PostAsync(url, null)
                : await _http.PostAsJsonAsync(url, body, _jsonOptions);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                _navigation.NavigateTo("/panel/login");
        }
        catch { }
    }

    private async Task<T?> PutAsync<T>(string url, object body)
    {
        if (!await AddAuthHeaderAsync()) return default;
        try
        {
            var response = await _http.PutAsJsonAsync(url, body, _jsonOptions);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _navigation.NavigateTo("/panel/login");
                return default;
            }
            if (!response.IsSuccessStatusCode) return default;
            return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
        }
        catch { return default; }
    }

    private async Task PutVoidAsync(string url, object body)
    {
        if (!await AddAuthHeaderAsync()) return;
        try
        {
            var response = await _http.PutAsJsonAsync(url, body, _jsonOptions);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                _navigation.NavigateTo("/panel/login");
        }
        catch { }
    }

    private async Task<T?> PatchAsync<T>(string url, object? body = null)
    {
        if (!await AddAuthHeaderAsync()) return default;
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, url);
            if (body is not null)
                request.Content = JsonContent.Create(body, options: _jsonOptions);
            var response = await _http.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _navigation.NavigateTo("/panel/login");
                return default;
            }
            if (!response.IsSuccessStatusCode) return default;
            return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
        }
        catch { return default; }
    }

    private async Task DeleteAsync(string url)
    {
        if (!await AddAuthHeaderAsync()) return;
        try
        {
            var response = await _http.DeleteAsync(url);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                _navigation.NavigateTo("/panel/login");
        }
        catch { }
    }

    private async Task<T?> DeleteWithResultAsync<T>(string url)
    {
        if (!await AddAuthHeaderAsync()) return default;
        try
        {
            var response = await _http.DeleteAsync(url);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _navigation.NavigateTo("/panel/login");
                return default;
            }
            if (!response.IsSuccessStatusCode) return default;
            return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
        }
        catch { return default; }
    }

    // Dashboard
    public async Task<DashboardSummaryModel> GetDashboardSummaryAsync()
    {
        var now = DateTime.Now;
        var projectsTask = GetProjectsAsync(1, "InProgress");
        var financeTask = GetMonthlySummaryAsync(now.Year, now.Month);
        var trainingTask = GetTrainingProgressAsync();

        await Task.WhenAll(projectsTask, financeTask, trainingTask);

        var projects = await projectsTask;
        var finance = await financeTask;
        var training = await trainingTask;

        return new DashboardSummaryModel
        {
            ActiveProjectsCount = projects.TotalCount,
            MonthlyIncome = finance.TotalIncome,
            TrainingProgressPercent = training.ProgressPercentage
        };
    }

    // Clients
    public async Task<PagedResultModel<ClientModel>> GetClientsAsync(int page = 1, string? search = null)
    {
        var url = $"/api/clients?page={page}";
        if (!string.IsNullOrEmpty(search)) url += $"&search={Uri.EscapeDataString(search)}";
        return await GetAsync<PagedResultModel<ClientModel>>(url) ?? new PagedResultModel<ClientModel>();
    }

    public async Task<ClientModel?> CreateClientAsync(CreateClientModel dto) =>
        await PostAsync<ClientModel>("/api/clients", dto);

    public async Task<ClientModel?> UpdateClientAsync(Guid id, UpdateClientModel dto) =>
        await PutAsync<ClientModel>($"/api/clients/{id}", dto);

    public async Task ArchiveClientAsync(Guid id) =>
        await PatchAsync<object>($"/api/clients/{id}/archive");

    // Pipeline
    public async Task<List<OpportunityGroupModel>> GetOpportunitiesByPhaseAsync() =>
        await GetAsync<List<OpportunityGroupModel>>("/api/opportunities/by-phase") ?? [];

    public async Task<OpportunityModel?> CreateOpportunityAsync(CreateOpportunityModel dto) =>
        await PostAsync<OpportunityModel>("/api/opportunities", dto);

    public async Task<MoveToPhaseResultModel?> MoveToPhaseAsync(Guid id, string newPhase) =>
        await PatchAsync<MoveToPhaseResultModel>($"/api/opportunities/{id}/move", new { NewPhase = newPhase });

    // Projects
    public async Task<PagedResultModel<ProjectSummaryModel>> GetProjectsAsync(int page = 1, string? status = null)
    {
        var url = $"/api/projects?page={page}";
        if (!string.IsNullOrEmpty(status)) url += $"&status={Uri.EscapeDataString(status)}";
        return await GetAsync<PagedResultModel<ProjectSummaryModel>>(url) ?? new PagedResultModel<ProjectSummaryModel>();
    }

    public async Task<ProjectDetailModel?> GetProjectByIdAsync(Guid id) =>
        await GetAsync<ProjectDetailModel>($"/api/projects/{id}");

    public async Task<ProjectDetailModel?> CreateProjectAsync(CreateProjectModel dto) =>
        await PostAsync<ProjectDetailModel>("/api/projects", dto);

    public async Task<ProjectDetailModel?> UpdateProjectStatusAsync(Guid id, string newStatus) =>
        await PatchAsync<ProjectDetailModel>($"/api/projects/{id}/status", new { NewStatus = newStatus });

    public async Task<DeliverableModel?> CompleteDeliverableAsync(Guid projectId, Guid deliverableId) =>
        await PatchAsync<DeliverableModel>($"/api/projects/{projectId}/deliverables/{deliverableId}/complete");

    public async Task<List<ProjectTemplateModel>> GetTemplatesAsync() =>
        await GetAsync<List<ProjectTemplateModel>>("/api/project-templates") ?? [];

    // Finance
    public async Task<MonthlySummaryModel> GetMonthlySummaryAsync(int year, int month) =>
        await GetAsync<MonthlySummaryModel>($"/api/finance/summary?year={year}&month={month}")
        ?? new MonthlySummaryModel { Year = year, Month = month };

    public async Task<FinancialProjectionModel> GetProjectionAsync() =>
        await GetAsync<FinancialProjectionModel>("/api/finance/projection")
        ?? new FinancialProjectionModel();

    public async Task CreateInvoiceAsync(CreateInvoiceModel dto) =>
        await PostVoidAsync("/api/finance/invoices", dto);

    public async Task CreateExpenseAsync(CreateExpenseModel dto) =>
        await PostVoidAsync("/api/finance/expenses", dto);

    // Training
    public async Task<List<TrainingItemModel>> GetChecklistAsync() =>
        await GetAsync<List<TrainingItemModel>>("/api/training/roadmap") ?? [];

    public async Task<TrainingItemModel?> CompleteChecklistItemAsync(Guid id) =>
        await PatchAsync<TrainingItemModel>($"/api/training/items/{id}/complete");

    public async Task<TrainingProgressModel> GetTrainingProgressAsync() =>
        await GetAsync<TrainingProgressModel>("/api/training/progress")
        ?? new TrainingProgressModel();

    // Catalog
    public async Task<List<ServiceAdminModel>> GetAllServicesAsync() =>
        await GetAsync<List<ServiceAdminModel>>("/api/services") ?? [];

    public async Task<ServiceAdminModel?> CreateServiceAsync(CreateServiceAdminModel dto) =>
        await PostAsync<ServiceAdminModel>("/api/services", dto);

    public async Task<ServiceAdminModel?> UpdateServiceAsync(Guid id, UpdateServiceAdminModel dto) =>
        await PutAsync<ServiceAdminModel>($"/api/services/{id}", dto);

    public async Task<ServiceAdminModel?> ToggleServiceAsync(Guid id) =>
        await PatchAsync<ServiceAdminModel>($"/api/services/{id}/toggle");

    public async Task ReorderServicesAsync(List<ServiceOrderModel> items) =>
        await PostVoidAsync("/api/services/reorder", items);

    public async Task<DeleteServiceResultModel?> DeleteServiceAsync(Guid id, bool confirmed = false) =>
        await DeleteWithResultAsync<DeleteServiceResultModel>($"/api/services/{id}?confirmed={confirmed}");

    // Analytics
    public async Task<AnalyticsSummaryModel> GetAnalyticsSummaryAsync(DateTime from, DateTime to) =>
        await GetAsync<AnalyticsSummaryModel>(
            $"/api/analytics/summary?from={from:yyyy-MM-dd}&to={to:yyyy-MM-dd}")
        ?? new AnalyticsSummaryModel();

    public async Task<List<TopServiceModel>> GetTopServicesAsync(DateTime from, DateTime to) =>
        await GetAsync<List<TopServiceModel>>(
            $"/api/analytics/top-services?from={from:yyyy-MM-dd}&to={to:yyyy-MM-dd}") ?? [];

    // Notifications config
    public async Task<NotificationConfigModel?> GetNotificationConfigAsync() =>
        await GetAsync<NotificationConfigModel>("/api/notifications/config");

    public async Task<NotificationConfigModel?> UpdateNotificationConfigAsync(UpdateNotificationConfigModel dto) =>
        await PutAsync<NotificationConfigModel>("/api/notifications/config", dto);

    // Calendar availability
    public async Task<List<AvailabilityModel>> GetAvailabilityAsync() =>
        await GetAsync<List<AvailabilityModel>>("/api/calendar/availability") ?? [];

    public async Task UpdateAvailabilityAsync(List<UpdateAvailabilityModel> dtos) =>
        await PutVoidAsync("/api/calendar/availability", dtos);
}
