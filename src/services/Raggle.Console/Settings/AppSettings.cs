using Raggle.Core.Options.Platforms;
using Raggle.Core.Options.Prompts;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Raggle.Console.Settings;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AIPlatforms
{
    OpenAI,
    AzureAI,
}

public class AppSettings
{
    public required string WorkingDirectory { get; set; }
    public required DefaultPromptOption Prompt { get; set; }
    public required PlatformOptions Platforms { get; set; }
}

public class PlatformOptions
{
    public required AIPlatforms PlatformType { get; set; }
    public OpenAIOption OpenAI { get; set; }
    public AzureAIOption AzureAI { get; set; }
}