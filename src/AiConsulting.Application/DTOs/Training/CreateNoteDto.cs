namespace AiConsulting.Application.DTOs.Training;

public class CreateNoteDto
{
    public string Content { get; set; } = string.Empty;
    public string? ResourceUrl { get; set; }
}
