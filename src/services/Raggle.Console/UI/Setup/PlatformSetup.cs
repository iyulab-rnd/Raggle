using Raggle.Console.Settings;
using Spectre.Console;

namespace Raggle.Console.UI.Setup;

// 어떤 AI 플랫폼을 사용할지 설정하는 스텝
public class PlatformSetup
{
    public AIPlatforms Setup()
    {
        AnsiConsole.MarkupLine(
"""
[bold]Second Step[/]
"""
            );

        var platform = AnsiConsole.Prompt(
            new SelectionPrompt<AIPlatforms>()
                .Title("Please select the AI platform you want to use:")
                .PageSize(3)
                .AddChoices(AIPlatforms.OpenAI, AIPlatforms.AzureAI)
        );

        return platform;
    }
}
