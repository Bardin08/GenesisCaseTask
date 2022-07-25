using Newtonsoft.Json.Linq;

namespace Core.Models;

public class CoinbaseRatesResponse
{
    public Data? Data { get; set; }
}

public class Data
{
    public string? Currency { get; set; }
    public JToken? Rates { get; set; }
}