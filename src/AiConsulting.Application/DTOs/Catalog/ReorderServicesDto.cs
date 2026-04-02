namespace AiConsulting.Application.DTOs.Catalog;

public class ServiceOrderItem
{
    public Guid Id { get; set; }
    public int SortOrder { get; set; }
}

public class ReorderServicesDto
{
    public List<ServiceOrderItem> Items { get; set; } = new();
}
