using AiConsulting.Web.Models.Panel;

namespace AiConsulting.Web.Services;

public interface IPanelApiService
{
    // Dashboard
    Task<DashboardSummaryModel> GetDashboardSummaryAsync();

    // Clients
    Task<PagedResultModel<ClientModel>> GetClientsAsync(int page = 1, string? search = null);
    Task<ClientModel?> CreateClientAsync(CreateClientModel dto);
    Task<ClientModel?> UpdateClientAsync(Guid id, UpdateClientModel dto);
    Task ArchiveClientAsync(Guid id);

    // Pipeline
    Task<List<OpportunityGroupModel>> GetOpportunitiesByPhaseAsync();
    Task<OpportunityModel?> CreateOpportunityAsync(CreateOpportunityModel dto);
    Task<MoveToPhaseResultModel?> MoveToPhaseAsync(Guid id, string newPhase);

    // Projects
    Task<PagedResultModel<ProjectSummaryModel>> GetProjectsAsync(int page = 1, string? status = null);
    Task<ProjectDetailModel?> GetProjectByIdAsync(Guid id);
    Task<ProjectDetailModel?> CreateProjectAsync(CreateProjectModel dto);
    Task<ProjectDetailModel?> UpdateProjectStatusAsync(Guid id, string newStatus);
    Task<DeliverableModel?> CompleteDeliverableAsync(Guid projectId, Guid deliverableId);
    Task<List<ProjectTemplateModel>> GetTemplatesAsync();

    // Finance
    Task<MonthlySummaryModel> GetMonthlySummaryAsync(int year, int month);
    Task<FinancialProjectionModel> GetProjectionAsync();
    Task CreateInvoiceAsync(CreateInvoiceModel dto);
    Task CreateExpenseAsync(CreateExpenseModel dto);

    // Training
    Task<List<TrainingItemModel>> GetChecklistAsync();
    Task<TrainingItemModel?> CompleteChecklistItemAsync(Guid id);
    Task<TrainingProgressModel> GetTrainingProgressAsync();

    // Catalog
    Task<List<ServiceAdminModel>> GetAllServicesAsync();
    Task<ServiceAdminModel?> CreateServiceAsync(CreateServiceAdminModel dto);
    Task<ServiceAdminModel?> UpdateServiceAsync(Guid id, UpdateServiceAdminModel dto);
    Task<ServiceAdminModel?> ToggleServiceAsync(Guid id);
    Task ReorderServicesAsync(List<ServiceOrderModel> items);
    Task<DeleteServiceResultModel?> DeleteServiceAsync(Guid id, bool confirmed = false);

    // Analytics
    Task<AnalyticsSummaryModel> GetAnalyticsSummaryAsync(DateTime from, DateTime to);
    Task<List<TopServiceModel>> GetTopServicesAsync(DateTime from, DateTime to);

    // Notifications config
    Task<NotificationConfigModel?> GetNotificationConfigAsync();
    Task<NotificationConfigModel?> UpdateNotificationConfigAsync(UpdateNotificationConfigModel dto);

    // Calendar availability
    Task<List<AvailabilityModel>> GetAvailabilityAsync();
    Task UpdateAvailabilityAsync(List<UpdateAvailabilityModel> dtos);
}
