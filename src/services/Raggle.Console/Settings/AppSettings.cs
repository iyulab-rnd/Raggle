using Raggle.Core.Options.Chat;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Raggle.Console.Settings;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AIPlatforms
{
    OpenAI,
    AzureAI,
    GoogleAI,
}

public class AppSettings
{
    public required string WorkingDirectory { get; set; }
    public required AIPlatforms PlatformType { get; set; }
    public required PromptOption Prompt { get; set; }
    public required OpenAIOption OpenAI { get; set; }
    public required AzureAIOption AzureAI { get; set; }
    public required GoogleAIOption GoogleAI { get; set; }

    public static AppSettings? GetSettings(string baseDir)
    {
        try
        {
            var configDir = Path.Combine(baseDir, Constants.SETTING_DIRECTORY);
            var settingsPath = Path.Combine(configDir, Constants.SETTING_FILENAME);

            if (!File.Exists(settingsPath)) return null;

            return JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(settingsPath));
        }
        catch (Exception)
        {
            return null;
        }
    }
}
