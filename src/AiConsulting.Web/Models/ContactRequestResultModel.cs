namespace AiConsulting.Web.Models;

public class ContactRequestResultModel
{
    public Guid Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid OpportunityId { get; set; }
}
