namespace AiConsulting.Application.DTOs.Clients;

public class ClientFilterDto
{
    public string? Search { get; set; }
    public bool IncludeArchived { get; set; } = false;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
