namespace AiConsulting.Application.DTOs.Public;

public class ContactRequestResultDto
{
    public Guid Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid OpportunityId { get; set; }
}
