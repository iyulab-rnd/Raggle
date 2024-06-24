using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using System.Text;
using Spectre.Console;
using Raggle.Console.Settings;
using Raggle.Console.Builders;

namespace Raggle.Console.UI;

public class ChatUI
{
    private readonly ChatHistory _history;
    private readonly IChatCompletionService _chat;
    private readonly StringBuilder reply = new();

    public ChatUI(AppSettings settings)
    {
        _chat = ChatServiceBuilder.Build(settings);
        _history = new ChatHistory(Constants.DEFAULT_SYSTEM_PROMPT);
    }

    public async Task StartAsync()
    {
        System.Console.Clear();
        AnsiConsole.Markup($"[bold {Constants.BOT_COLOR}]{Constants.BOT_NAME} >[/] {Constants.WELCOME_MESSAGE}");

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
                _history.Clear();
                System.Console.Clear();
                continue;
            }
            
            _history.AddUserMessage(prompt);
            //var longTermMemory = await GetLongTermMemory(memory, userMessage);
            //_history[0].Content = $"{systemPrompt}\n\nLong term memory:\n{longTermMemory}";
            AnsiConsole.Markup($"[bold {Constants.BOT_COLOR}]{Constants.BOT_NAME} >[/] ");
            await foreach (StreamingChatMessageContent stream in _chat.GetStreamingChatMessageContentsAsync(_history))
            {
                AnsiConsole.Write(stream.Content ?? "");
                reply.Append(stream.Content);
            }
            AnsiConsole.WriteLine();
            _history.AddAssistantMessage(reply.ToString());
        }
    }
}
