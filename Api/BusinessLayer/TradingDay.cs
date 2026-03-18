namespace Api.BusinessLayer;

public record TradingDay(DateOnly Day, List<ExchangeRate> ExchangeRates);