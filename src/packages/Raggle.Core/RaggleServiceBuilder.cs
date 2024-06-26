using Microsoft.KernelMemory;
using Microsoft.KernelMemory.DocumentStorage.DevTools;
using Microsoft.KernelMemory.FileSystem.DevTools;
using Microsoft.KernelMemory.MemoryStorage.DevTools;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using Raggle.Abstractions;
using Microsoft.KernelMemory.AI.OpenAI;
using Raggle.Abstractions.Attributes;
using static Microsoft.KernelMemory.OpenAIConfig;
using Raggle.Core.Options.Platforms;
using Raggle.Core.Options.Vectors;

namespace Raggle.Core;

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

public static partial class RaggleServiceBuilderExtension
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
            TextGenerationType = TextGenerationTypes.Chat,
            TextModel = option.TextModel.GetValue(),
            TextModelMaxTokenTotal = option.TextModelMaxToken,
            EmbeddingModel = option.EmbeddingModel.GetValue(),
            EmbeddingModelMaxTokenTotal = option.EmbeddingModelMaxToken,
            MaxRetries = option.MaxRetries,
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
            modelId: option.TextModel.GetValue(),
            apiKey: option.ApiKey
        );

        return builder;
    }

    public static IRaggleServiceBuilder WithAzureAI(
        this IRaggleServiceBuilder builder,
        AzureAIOption option)
    {
        var textGenerationTokenizer = new DefaultGPTTokenizer();
        var textEmbeddingTokenizer = new DefaultGPTTokenizer();

        var openAIConfig = new OpenAIConfig
        {
            APIKey = option.ApiKey,
            TextModel = option.ChatModel.GetValue(),
            //TextModelMaxTokenTotal = option.ChatModelMaxTokenTotal,
            EmbeddingModel = option.EmbeddingModel.GetValue(),
            //EmbeddingModelMaxTokenTotal = option.EmbeddingModelMaxTokenTotal,
            //MaxRetries = option.MaxRetries,
            TextGenerationType = TextGenerationTypes.Chat,
        };
        openAIConfig.Validate();

        builder.MemoryBuilder.Services.AddOpenAITextEmbeddingGeneration(openAIConfig, textEmbeddingTokenizer);
        builder.MemoryBuilder.Services.AddOpenAITextGeneration(openAIConfig, textGenerationTokenizer);

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
            Directory = option.ChunkDirectory,
            StorageType = FileSystemTypes.Disk
        })
        .WithSimpleVectorDb(new SimpleVectorDbConfig
        {
            Directory = option.VectorDirectory,
            StorageType = FileSystemTypes.Disk
        });

        return builder;
    }
}