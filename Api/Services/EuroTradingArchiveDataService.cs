using System.Globalization;
using System.Xml.Linq;
using Api.BusinessLayer;

namespace Api.Services;

public interface IEuroTradingArchiveDataService
{
    Task<Archive> GetArchiveAsync(string uri);
}

public class EuroTradingArchiveDataService : IEuroTradingArchiveDataService
{
    private static readonly XNamespace EcbNs = "http://www.ecb.int/vocabulary/2002-08-01/eurofxref";
    private static readonly CultureInfo CultureInfo = CultureInfo.InvariantCulture;
    
    public async Task<Archive> GetArchiveAsync(string uri)
    {
        using var httpClient = new HttpClient();
        await using var stream = await httpClient.GetStreamAsync(uri);
        var xDocument = await XDocument.LoadAsync(stream, LoadOptions.None, CancellationToken.None);
        var tradingDays = xDocument.Descendants(EcbNs + "Cube")
            .Where(p => p.Attribute("time") != null)
            .Select(p => new TradingDay(
                DateOnly.Parse(p.Attribute("time")!.Value, CultureInfo), 
                p.Elements(EcbNs + "Cube").Select(q => new ExchangeRate(
                    q.Attribute("currency")!.Value, 
                    1 / decimal.Parse(q.Attribute("rate")!.Value, 
                    CultureInfo))
                ).ToList())
            ).ToList();
        return new Archive(tradingDays);
    }
}