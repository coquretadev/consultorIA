namespace AiConsulting.Web.Models.Panel;

public class ServiceAdminModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Sector { get; set; }
    public decimal? PriceRangeMin { get; set; }
    public decimal? PriceRangeMax { get; set; }
    public bool IsActive { get; set; }
    public int SortOrder { get; set; }
    public string? Description { get; set; }
    public string? Benefits { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? Slug { get; set; }
    public int ProjectCount { get; set; }
}

public class CreateServiceAdminModel
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Benefits { get; set; }
    public string? Sector { get; set; }
    public decimal? PriceRangeMin { get; set; }
    public decimal? PriceRangeMax { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? Slug { get; set; }
}

public class UpdateServiceAdminModel
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Benefits { get; set; }
    public string? Sector { get; set; }
    public decimal? PriceRangeMin { get; set; }
    public decimal? PriceRangeMax { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? Slug { get; set; }
}

public class ServiceOrderModel
{
    public Guid Id { get; set; }
    public int SortOrder { get; set; }
}

public class DeleteServiceResultModel
{
    public bool Success { get; set; }
    public bool RequiresConfirmation { get; set; }
    public string? Message { get; set; }
}
