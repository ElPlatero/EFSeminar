using Api.Services;
using EntityFrameworkCoreSeminar.Database.Models.Chinook;
using Xunit.Abstractions;

namespace Tests;

public class EuroTradingArchiveDataServiceTests(ITestOutputHelper outputHelper) : ChinookTestBase
{
    [Fact]
    public async Task TestGetArchiveAsync()
    {
        await using var context = getContext(outputHelper);
        
        var service = getService(context);
        var archive = await service.GetArchiveAsync("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist-90d.xml");
        Assert.NotNull(archive);
    
        foreach (var tradingDay in archive.TradingDays)
        {
            outputHelper.WriteLine($"Am {tradingDay.Date:d} gab es die folgenden Wechselkurse:");
            foreach (var foo in tradingDay.ExchangeRates)
            {
                outputHelper.WriteLine($"- {foo.Currency}: {foo.Rate:C5}");
            }
        }
        
        await service.SaveArchiveAsync(archive);
    }
    
    private EuroTradingArchiveDataService getService(ChinookContext context) => new(context);
}