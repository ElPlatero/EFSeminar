namespace Api.BusinessLayer;

public record Archive
{
    public int Id { get; init; }
    public List<TradingDay> TradingDays { get; init; } = [];
}