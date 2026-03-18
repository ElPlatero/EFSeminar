using Api.Services;
using Xunit.Abstractions;

namespace Tests;

public class EuroTradingArchiveDataServiceTests(ITestOutputHelper outputHelper)
{
    [Fact]
    public async Task TestGetArchiveAsync()
    {
        var service = getService();
        var archive = await service.GetArchiveAsync("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist-90d.xml");
        Assert.NotNull(archive);

        foreach (var tradingDay in archive.TradingDays)
        {
            outputHelper.WriteLine($"Am {tradingDay.Day:d} gab es die folgenden Wechselkurse:");
            foreach (var foo in tradingDay.ExchangeRates)
            {
                outputHelper.WriteLine($"- {foo.Currency}: {foo.Rate:C5}");
            }
        }
    }
    
    private EuroTradingArchiveDataService getService() => new();
}