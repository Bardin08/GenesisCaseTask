using Core.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("rate")]
public class RateController : ControllerBase
{
    private readonly IExchangeRateService _exchangeRateService;

    public RateController(IExchangeRateService exchangeRateService)
    {
        _exchangeRateService = exchangeRateService;
    }

    [HttpGet]
    [Route("")]
    public async Task<decimal> GetBtcExchangeRate()
    {
        return await _exchangeRateService.GetCurrentBtcToUahExchangeRateAsync();
    }
}