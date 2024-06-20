using System.CommandLine;

var pathOption = new Option<string>("--path", "directory path to use as the base");

var rootCommand = new RootCommand("A simple console app that says hello");
rootCommand.AddOption(pathOption);

rootCommand.SetHandler((handle) =>
{
    // Set the base directory for the application

    // Check setting directory

    // SetupUI setup = new SetupUI();
    // setup.Start();

    // FileSystem fs = new FileSystem(baseDir);
    // fs.Start();

    // ChatUI chat = new ChatUI();
    // chat.Start();
    
}, pathOption);

return rootCommand.Invoke(args);
