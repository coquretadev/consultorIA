namespace AiConsulting.Application.DTOs.Catalog;

public class CreateServiceDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Benefits { get; set; } = string.Empty;
    public decimal PriceRangeMin { get; set; }
    public decimal PriceRangeMax { get; set; }
    public int EstimatedDeliveryDays { get; set; }
    public string TargetSector { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}
