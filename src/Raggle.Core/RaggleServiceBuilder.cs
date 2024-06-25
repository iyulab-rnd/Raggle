using Microsoft.KernelMemory;
using Microsoft.KernelMemory.DocumentStorage.DevTools;
using Microsoft.KernelMemory.FileSystem.DevTools;
using Microsoft.KernelMemory.MemoryStorage.DevTools;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using Raggle.Abstractions.Services;
using Raggle.Abstractions;
using Microsoft.KernelMemory.AI.OpenAI;
using System.Net.Http;
using Raggle.Core.Options.Chat;
using System.Diagnostics.CodeAnalysis;
using Raggle.Abstractions.Attributes;
using Raggle.Core.Options.Vector;
using static Microsoft.KernelMemory.OpenAIConfig;

namespace Raggle.Core;

[Experimental("KMEXP01")]
public class RaggleServiceBuilder : IRaggleServiceBuilder
{
    public IKernelBuilder KernelBuilder { get; set; } = Kernel.CreateBuilder();
    public IKernelMemoryBuilder MemoryBuilder { get; set; } = new KernelMemoryBuilder();

    public IRaggleService Build()
    {
        var memory = MemoryBuilder.Build();
        var chat = KernelBuilder.Build().GetRequiredService<IChatCompletionService>();
        return new RaggleService(chat, memory);
    }
}

public static class RaggleServiceBuilderExtension
{
    public static IRaggleServiceBuilder WithOpenAI(
        this IRaggleServiceBuilder builder,
        OpenAIOption option)
    {
        var textGenerationTokenizer = new DefaultGPTTokenizer();
        var textEmbeddingTokenizer = new DefaultGPTTokenizer();

        var openAIConfig = new OpenAIConfig
        {
            APIKey = option.ApiKey,
            TextModel = option.ChatModel.GetValue(),
            TextModelMaxTokenTotal = option.ChatModelMaxTokenTotal,
            EmbeddingModel = option.EmbeddingModel.GetValue(),
            EmbeddingModelMaxTokenTotal = option.EmbeddingModelMaxTokenTotal,
            MaxRetries = option.MaxRetries,
            TextGenerationType = TextGenerationTypes.Chat,
        };
        openAIConfig.Validate();

        builder.MemoryBuilder.Services.AddOpenAITextEmbeddingGeneration(openAIConfig, textEmbeddingTokenizer);
        builder.MemoryBuilder.Services.AddOpenAITextGeneration(openAIConfig, textGenerationTokenizer);

#pragma warning disable KMEXP01
        builder.MemoryBuilder.AddIngestionEmbeddingGenerator(new OpenAITextEmbeddingGenerator(
            config: openAIConfig,
            textTokenizer: textEmbeddingTokenizer
        ));
#pragma warning restore KMEXP01

        builder.KernelBuilder.AddOpenAIChatCompletion(
            modelId: option.ChatModel.GetValue(),
            apiKey: option.ApiKey
        );

        return builder;
    }

    public static IRaggleServiceBuilder WithFileVector(
        this IRaggleServiceBuilder builder,
        FileVectorOption option)
    {
        builder.MemoryBuilder.WithSimpleFileStorage(new SimpleFileStorageConfig
        {
            Directory = option.Directory,
            StorageType = FileSystemTypes.Disk
        })
        .WithSimpleVectorDb(new SimpleVectorDbConfig
        {
            Directory = option.Directory,
            StorageType = FileSystemTypes.Disk
        });

        return builder;
    }
}