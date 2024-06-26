using Raggle.Abstractions;
using Raggle.Console.Settings;
using Raggle.Core;
using Raggle.Core.Options.Vectors;
using System.Text.Json;

namespace Raggle.Console;

public class AppBuilder : IDisposable
{
    private readonly string _baseDir;
    private readonly string _configDir;
    private readonly string _settingsPath;

    public AppBuilder(string baseDir)
    {
        _baseDir = baseDir;
        _configDir = Path.Combine(_baseDir, Constants.SETTING_DIRECTORY);
        _settingsPath = Path.Combine(_configDir, Constants.SETTING_FILENAME);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public IRaggleService BuildRaggleService(AppSettings settings)
    {
        var builder = new RaggleServiceBuilder();
        var platform = settings.Platforms.PlatformType;
        if (platform == AIPlatforms.OpenAI)
        {
            builder.WithOpenAI(settings.Platforms.OpenAI);
        }
        else if (platform == AIPlatforms.AzureAI)
        {
            builder.WithAzureAI(settings.Platforms.AzureAI);
        }

        builder.WithFileVector(new FileVectorOption
        {
            ChunkDirectory = Path.Combine(_configDir, Constants.FILES_DIRECTORY),
            VectorDirectory = Path.Combine(_configDir, Constants.VECTOR_DIRECTORY)
        });

        return builder.Build();
    }

    public void SaveSettings(AppSettings settings)
    {
        if (!Directory.Exists(_configDir))
            Directory.CreateDirectory(_configDir);

        File.WriteAllText(_settingsPath, JsonSerializer.Serialize(settings, new JsonSerializerOptions
        {
            WriteIndented = true
        }));
    }

    public AppSettings? GetSettings()
    {
        try
        {
            if (!File.Exists(_settingsPath)) return null;
            return JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(_settingsPath));
        }
        catch (Exception)
        {
            return null;
        }
    }
}
