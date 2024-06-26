using Spectre.Console;
using Raggle.Abstractions;

namespace Raggle.Console.UI;

public class ChatUI
{
    private readonly IRaggleService _raggle;

    public ChatUI(IRaggleService raggleService)
    {
        _raggle = raggleService;
    }

    public async Task StartAsync()
    {
        System.Console.Clear();
        AnsiConsole.MarkupLine($"[bold {Constants.BOT_COLOR}]{Constants.BOT_NAME} >[/] {Constants.WELCOME_MESSAGE}");
        while (true)
        {
            var prompt = AnsiConsole.Ask<string>($"[bold {Constants.USER_COLOR}]{Constants.USER_NAME} >[/] ").Trim();
            if (prompt == "exit()")
            {
                Environment.Exit(0);
                break;
            }
            if (prompt == "clear()")
            {
                System.Console.Clear();
                continue;
            }

            AnsiConsole.Markup($"[bold {Constants.BOT_COLOR}]{Constants.BOT_NAME} >[/] ");
            await foreach (var stream in _raggle.AskStreamingAsync(prompt))
            {
                AnsiConsole.Write(stream ?? "");
            }
            AnsiConsole.WriteLine();
        }
    }
}
