using RedditScraper.Helper.Environment.Enums;

namespace RedditScraper.Services.Environment;

public class EnvService(IConfiguration configuration) : IEnvService
{
    private readonly IConfiguration _config = configuration;

    public string GetEnvVariable(EnvVariableKeys key)
    {
        var val =
            _config[key.ToString()]
            ?? throw new InvalidOperationException(
                "Environment variable requested does not exist."
            );
        return val;
    }
}
