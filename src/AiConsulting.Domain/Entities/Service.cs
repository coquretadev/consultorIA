namespace AiConsulting.Domain.Entities;

public class Service
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Benefits { get; set; } = string.Empty;

    private decimal _priceRangeMin;
    public decimal PriceRangeMin
    {
        get => _priceRangeMin;
        set
        {
            if (value > PriceRangeMax && PriceRangeMax != 0)
                throw new ArgumentException("PriceRangeMin must be less than or equal to PriceRangeMax.");
            _priceRangeMin = value;
        }
    }

    private decimal _priceRangeMax;
    public decimal PriceRangeMax
    {
        get => _priceRangeMax;
        set
        {
            if (value < PriceRangeMin)
                throw new ArgumentException("PriceRangeMax must be greater than or equal to PriceRangeMin.");
            _priceRangeMax = value;
        }
    }

    public int EstimatedDeliveryDays { get; set; }
    public string TargetSector { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string Slug { get; set; } = string.Empty;
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }

    public ICollection<Project> Projects { get; set; } = new List<Project>();
    public ICollection<ServiceTranslation> Translations { get; set; } = new List<ServiceTranslation>();

    public void SetPriceRange(decimal min, decimal max)
    {
        if (min > max)
            throw new ArgumentException("PriceRangeMin must be less than or equal to PriceRangeMax.");
        _priceRangeMin = min;
        _priceRangeMax = max;
    }
}
