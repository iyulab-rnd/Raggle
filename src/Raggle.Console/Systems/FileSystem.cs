using Spectre.Console;
using Raggle.Abstractions.Services;

namespace Raggle.Console.Systems;

public class FileSystem
{
    private readonly IMemoryService _memory;
    private readonly FileSystemWatcher _watcher = new();

    public FileSystem(IMemoryService memoryService)
    {
        _memory = memoryService;
    }

    public async Task Initialize(string baseDir)
    {
        var files = CollectFiles(baseDir);
        await _memory.MemorizeDocumentsAsync(files);
    }

    public void Watch(string directory)
    {
        _watcher.Path = directory;
        _watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
        _watcher.Filter = "*.*";
        _watcher.IncludeSubdirectories = true;
        _watcher.EnableRaisingEvents = true;

        _watcher.Created += OnCreated;
        _watcher.Changed += OnChanged;
        _watcher.Renamed += OnRenamed;
        _watcher.Deleted += OnDeleted;
    }

    private void OnCreated(object sender, FileSystemEventArgs e)
    {
        Task.Run(async () =>
        {
            if (IsSetting(e.FullPath)) return;
            await _memory.MemorizeDocumentAsync(e.FullPath);
        });
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        Task.Run(async () =>
        {
            if (IsSetting(e.FullPath)) return;
            await _memory.UnMemorizeAsync(e.FullPath);
            await _memory.MemorizeDocumentAsync(e.FullPath);
        });
    }

    private void OnRenamed(object sender, RenamedEventArgs e)
    {
        Task.Run(async () =>
        {
            if (IsSetting(e.FullPath)) return;
            await _memory.MemorizeDocumentAsync(e.FullPath);
            await _memory.UnMemorizeAsync(e.OldFullPath);
        });
    }

    private void OnDeleted(object sender, FileSystemEventArgs e)
    {
        Task.Run(() =>
        {
            if (IsSetting(e.FullPath)) return;
            _memory.UnMemorizeAsync(e.FullPath);
        });
    }

    private IEnumerable<string> CollectFiles(string baseDir)
    {
        var files = new List<string>();

        string[] baseFiles = Directory.GetFiles(baseDir);
        files.AddRange(baseFiles);

        string[] subDirectories = Directory.GetDirectories(baseDir);
        foreach (string subDir in subDirectories)
        {
            if (Path.GetFileName(subDir) == Constants.SETTING_DIRECTORY)
            {
                continue;
            }
            files.AddRange(CollectFiles(subDir));
        }

        return files;
    }

    private bool IsSetting(string path)
    {
        return path.Contains(Constants.SETTING_DIRECTORY);
    }
}
