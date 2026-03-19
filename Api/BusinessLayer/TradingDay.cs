namespace Api.BusinessLayer;

public record TradingDay
{
    public int Id { get; init; }
    public DateOnly Date { get; init; }
    public List<ExchangeRate> ExchangeRates { get; init; } = new();
}