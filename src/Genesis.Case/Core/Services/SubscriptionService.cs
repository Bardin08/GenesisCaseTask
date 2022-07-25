using Core.Abstractions;
using Data.Providers;

namespace Core.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly IEmailService _emailService;
    private readonly IExchangeRateService _exchangeRateService;
    private readonly IJsonEmailsStorage _emailsStorage;

    public SubscriptionService(IEmailService emailService, IExchangeRateService exchangeRateService, IJsonEmailsStorage emailsStorage)
    {
        _emailService = emailService;
        _exchangeRateService = exchangeRateService;
        _emailsStorage = emailsStorage;
    }

    public async Task<bool> SubscribeAsync(string email)
    {
        // We can't use ReadAsync here, because a model that we store doesn't contains an ID.
        var isAlreadyExists = (await _emailsStorage.ReadAllAsync(0, 0))
            .FirstOrDefault(x => x!.Equals(email), null) != null;

        if (isAlreadyExists)
        {
            return false;
        }

        await _emailsStorage.CreateAsync(email);
        return true;
    }

    public async Task NotifyAsync()
    {
        var emailAddresses = await _emailsStorage.ReadAllAsync(0, 0);
        var currentExchangeRate = await _exchangeRateService.GetCurrentBtcToUahExchangeRateAsync();

        var tasks = emailAddresses
            .Select(address => _emailService.SendEmailAsync(address, "Current Exchange Rate",
                $"Hello, {address}!\n\nWe have a new BTC to UAH exchange rate for you! It is {currentExchangeRate} now!"))
            .Cast<Task>()
            .ToList();

        await Task.WhenAll(tasks);
    }
}