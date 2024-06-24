using Microsoft.KernelMemory;
using Microsoft.KernelMemory.DocumentStorage.DevTools;
using Microsoft.KernelMemory.FileSystem.DevTools;
using Microsoft.KernelMemory.MemoryStorage.DevTools;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using Raggle.Abstractions.Services;
using Raggle.Console.Settings;
using Raggle.Core.Services;
using MemoryService = Raggle.Core.Services.FileMemoryService;
using Microsoft.KernelMemory.AI.OpenAI;
using Microsoft.KernelMemory.AI;
using Raggle.Abstractions;

namespace Raggle.Core;

public class MemoryServiceBuilder : IMemoryServiceBuilder
{
    public IMemoryService Build(AppSettings settings)
    {
        var kernelMemory = new KernelMemoryBuilder()
            .WithOpenAIDefaults(settings.OpenAI.ApiKey)
            .WithSimpleVectorDb(new SimpleVectorDbConfig
            {
                Directory = Path.Combine(settings.WorkingDirectory, Constants.SETTING_DIRECTORY, Constants.VECTOR_DIRECTORY),
                StorageType = FileSystemTypes.Disk
            })
            .WithSimpleFileStorage(new SimpleFileStorageConfig
            {
                Directory = Path.Combine(settings.WorkingDirectory, Constants.SETTING_DIRECTORY, Constants.FILES_DIRECTORY),
                StorageType = FileSystemTypes.Disk
            })
            .Build<MemoryServerless>();

        var kernel = Kernel.CreateBuilder()
            .AddOpenAIChatCompletion(
            modelId: settings.OpenAI.ModelName,
            apiKey: settings.OpenAI.ApiKey
            )
            .Build();
        return kernel.GetRequiredService<IChatCompletionService>();

        return new MemoryService(kernelMemory);
    }

    
}
