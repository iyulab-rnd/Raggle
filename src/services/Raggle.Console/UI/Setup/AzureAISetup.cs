using Raggle.Core.Options.Platforms;
using Spectre.Console;

namespace Raggle.Console.UI.Setup;
public class AzureAISetup
{
    public AzureAIOption Setup()
    {
        var apiKey = AnsiConsole.Ask<string>("Enter your Azure AI API key: ");

        return new AzureAIOption
        {
            ApiKey = apiKey,
        };
    }
}
