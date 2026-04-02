namespace AiConsulting.Domain.Entities;

public class ConsultorAvailability
{
    public Guid Id { get; set; }
    public int DayOfWeek { get; set; }

    private TimeOnly _startTime;
    public TimeOnly StartTime
    {
        get => _startTime;
        set
        {
            if (value >= EndTime && EndTime != default)
                throw new ArgumentException("StartTime must be less than EndTime.");
            _startTime = value;
        }
    }

    private TimeOnly _endTime;
    public TimeOnly EndTime
    {
        get => _endTime;
        set
        {
            if (value <= StartTime)
                throw new ArgumentException("EndTime must be greater than StartTime.");
            _endTime = value;
        }
    }

    public bool IsActive { get; set; }

    public void SetTimeRange(TimeOnly start, TimeOnly end)
    {
        if (start >= end)
            throw new ArgumentException("StartTime must be less than EndTime.");
        _startTime = start;
        _endTime = end;
    }
}
