namespace Core.Abstractions;

public interface ISubscriptionService
{
    Task<bool> SubscribeAsync(string email);
}