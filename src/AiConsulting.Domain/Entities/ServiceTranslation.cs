namespace AiConsulting.Domain.Entities;

public class ServiceTranslation
{
    public Guid Id { get; set; }
    public Guid ServiceId { get; set; }
    public string LanguageCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Benefits { get; set; } = string.Empty;
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }

    public Service Service { get; set; } = null!;
}
