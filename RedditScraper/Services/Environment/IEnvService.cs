using RedditScraper.Helper.Environment.Enums;

namespace RedditScraper.Services.Environment;

public interface IEnvService
{
    string GetEnvVariable(EnvVariableKeys key);
}
