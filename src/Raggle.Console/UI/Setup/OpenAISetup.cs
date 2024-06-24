using Raggle.Console.Settings;
using Spectre.Console;

namespace Raggle.Console.UI.Setup;

public class OpenAISetup
{
    public OpenAISetting Setup()
    {
        return new OpenAISetting
        {
            ModelName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select the OpenAI model")
                    .PageSize(10)
                    .AddChoices("gpt-4o", "gpt-4", "gpt-3.5-turbo")
            ),
            ApiKey = AnsiConsole.Ask<string>("Enter your OpenAI API key: "),
        };
    }
}