namespace Raggle.Core.Options.Prompts;

public class DefaultPromptOption
{
    public string SystemPrompt { get; set; } =
"""
You are a helpful assistant replying to user questions using information from your memory.
Reply very briefly and concisely, get to the point immediately. Don't provide long explanations unless necessary.
Sometimes you don't have relevant memories so you reply saying you don't know, don't have the information.
""";
    public string? UserPrompt { get; set; }
}
