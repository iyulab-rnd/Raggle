using Raggle.Core.Options.Prompts;
using Spectre.Console;

namespace Raggle.Console.UI.Setup;


// 현재 스텝은 Rag를 사용한 유저에게 BOT에게 어떤 파일들인지에 대해
// 제일 앞에서 시스템 프로프트 뒤에 붙일 유저 프로프트를 설정하는 스텝이다.
public class PromptSetup
{
    public DefaultPromptOption Setup()
    {
        AnsiConsole.MarkupLine(
"""
[bold]First Step[/]
"""
            );

        var userPrompt = AnsiConsole.Ask<string>(
            "Please enter the prompt describing the files you want to add to the system: "
        );

        return new DefaultPromptOption
        {
            UserPrompt = userPrompt
        };
    }
}
