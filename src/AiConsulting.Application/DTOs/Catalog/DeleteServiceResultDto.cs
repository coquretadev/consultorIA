namespace AiConsulting.Application.DTOs.Catalog;

public class DeleteServiceResultDto
{
    public bool Deleted { get; set; }
    public bool RequiresConfirmation { get; set; }
    public string Message { get; set; } = string.Empty;
}
