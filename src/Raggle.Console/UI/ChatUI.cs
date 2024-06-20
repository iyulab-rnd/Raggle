using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using System.Text;
using Spectre.Console;

namespace Raggle.Console.UI;

public class ChatUI
{
    private readonly ChatHistory _history;
    private readonly IChatCompletionService _chat;
    private readonly CancellationTokenSource _cts = new();

    public ChatUI(string openAIApiKey)
    {
        var kernel = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion(modelId: "gpt-4o", apiKey: openAIApiKey)
            .Build();
        _chat = kernel.GetRequiredService<IChatCompletionService>();
        _history = new ChatHistory(Constants.DEFAULT_SYSTEM_PROMPT);
    }

    public void Initialize()
    {

    }

    public async Task StartAsync()
    {
        System.Console.Clear();
        WelcomeMessage();

        while (_cts.IsCancellationRequested == false)
        {
            var prompt = UserAsk();
            if (string.IsNullOrWhiteSpace(prompt)) continue;
            await BotAnswerAsync(prompt);
        }
    }

    public void Stop()
    {
        _cts.Cancel();
    }

    private void WelcomeMessage()
    {
        AnsiConsole.Markup($"[bold {Constants.BOT_COLOR}]{Constants.BOT_NAME} >[/] {Constants.WELCOME_MESSAGE}");
    }

    private async Task BotAnswerAsync(string prompt)
    {
        //var longTermMemory = await GetLongTermMemory(memory, userMessage);
        //chatHistory[0].Content = $"{systemPrompt}\n\nLong term memory:\n{longTermMemory}";
        var reply = new StringBuilder();
        AnsiConsole.Markup($"[bold {Constants.BOT_COLOR}]{Constants.BOT_NAME} >[/] ");
        await foreach (StreamingChatMessageContent stream in _chat.GetStreamingChatMessageContentsAsync(_history))
        {
            AnsiConsole.Markup(stream.Content ?? "");
            reply.Append(stream.Content);
        }
        AnsiConsole.WriteLine();
        _history.AddAssistantMessage(reply.ToString());
    }

    private string UserAsk()
    {
        var prompt = AnsiConsole.Ask<string>($"[bold {Constants.USER_COLOR}]{Constants.USER_NAME} >[/] ");
        if (!string.IsNullOrWhiteSpace(prompt)) _history.AddUserMessage(prompt);
        return prompt;
    }
}
