using Data.Abstractions;

namespace Data.Providers;

public interface IJsonEmailsStorage : IJsonFileProvider<string, string>
{
} 

public class JsonEmailsStorage : JsonFileProvider<string, string>, IJsonEmailsStorage
{
    private const string FileName = "emails.json";

    public JsonEmailsStorage() : base(FileName)
    {
    }
}