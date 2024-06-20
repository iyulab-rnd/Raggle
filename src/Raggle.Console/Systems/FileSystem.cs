using Microsoft.KernelMemory.DocumentStorage.DevTools;
using Microsoft.KernelMemory.FileSystem.DevTools;
using Microsoft.KernelMemory.MemoryStorage.DevTools;
using Microsoft.KernelMemory;

namespace Raggle.Console.Systems;

public class FileSystem
{
    public FileSystem(string baseDir)
    {
        var dd = new KernelMemoryBuilder()
            .WithOpenAIDefaults("")
            .WithSimpleVectorDb(new SimpleVectorDbConfig
            {
                Directory = Path.Combine(baseDir, Constants.SETTING_DIRECTORY, Constants.VECTOR_DIRECTORY),
                StorageType = FileSystemTypes.Disk
            })
            .WithSimpleFileStorage(new SimpleFileStorageConfig
            {
                Directory = Path.Combine(baseDir, Constants.SETTING_DIRECTORY, Constants.FILES_DIRECTORY),
                StorageType = FileSystemTypes.Disk
            })
            .Build<MemoryServerless>();
    }

    public List<string> CollectFilePaths(string baseDir)
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
            files.AddRange(CollectFilePaths(subDir));
        }

        return files;
    }
}
