using Raggle.Console.Settings;
using Raggle.Console.UI.Setup;
using Spectre.Console;

namespace Raggle.Console.UI;

public class SetupUI
{
    public AppSettings Setup(string baseDir)
    {
        var step1 = new PromptSetup();
        var prompt = step1.Setup();

        var step2 = new PlatformSetup();
        var platformType = step2.Setup();

        var openAI = new OpenAISetting();
        var azureAI = new AzureAISetting();
        var googleAI = new GoogleAISetting();

        if (platformType == AIPlatforms.OpenAI)
        {
            var step3 = new OpenAISetup();
            openAI = step3.Setup();
        }
        else if (platformType == AIPlatforms.AzureAI)
        {
            var step3 = new AzureAISetup();
            azureAI = step3.Setup();
        }
        else if (platformType == AIPlatforms.GoogleAI)
        {
            var step3 = new GoogleAISetup();
            googleAI = step3.Setup();
        }

        return new AppSettings
        {
            WorkingDirectory = baseDir,
            Prompt = prompt,
            PlatformType = platformType,
            OpenAI = openAI,
            AzureAI = azureAI,
            GoogleAI = googleAI,
        };
    }

    public static void Exit()
    {
        AnsiConsole.MarkupLine("[red]Exiting the setup. Goodbye![/]");
        Environment.Exit(0);
    }
}
