using Core.Abstractions;
using Data.Providers;

namespace Core.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly IJsonEmailsStorage _emailsStorage;

    public SubscriptionService(IJsonEmailsStorage emailsStorage)
    {
        _emailsStorage = emailsStorage;
    }

    public async Task<bool> SubscribeAsync(string email)
    {
        // We can't use ReadAsync here, because a model that we store doesn't contains an ID.
        var isAlreadyExists = (await _emailsStorage.ReadAllAsync(0, 0))
            .FirstOrDefault(x => x!.Equals(email), null) != null ? true : false;

        if (isAlreadyExists)
        {
            return false;
        }

        await _emailsStorage.CreateAsync(email);
        return true;
    }
}