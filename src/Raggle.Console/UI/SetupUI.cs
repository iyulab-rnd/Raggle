using System.Diagnostics;
using System.Text.Json;

namespace Raggle.Console.UI;

public class SetupUI
{
    public SetupUI()
    {
        // Set the base directory for the application
        var baseDir = Debugger.IsAttached
            ? Constants.DEBUG_DIRECTORY
            : Directory.GetCurrentDirectory();
        System.Console.WriteLine($"Working directory: {baseDir}");

        // Create the configuration directory and save the settings
        var configDir = Path.Combine(baseDir, Constants.SETTING_DIRECTORY);
        var dirInfo = Directory.CreateDirectory(configDir);
        dirInfo.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
        var settings = new
        {
            OpenAI = new
            {
                ApiKey = ""
            }
        };
        File.WriteAllText(Path.Combine(configDir, Constants.SETTING_FILENAME), JsonSerializer.Serialize(settings, new JsonSerializerOptions
        {
            WriteIndented = true,
        }));
    }

    public void Start()
    {
        
    }

    public void Exit()
    {
        System.Console.WriteLine("Press any key to exit...");
        System.Console.ReadKey();
    }
}
