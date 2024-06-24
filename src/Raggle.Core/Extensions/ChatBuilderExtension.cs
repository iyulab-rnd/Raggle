using Microsoft.KernelMemory.AI.OpenAI;
using Microsoft.KernelMemory.AI;
using Microsoft.KernelMemory;
using Raggle.Abstractions;

namespace Raggle.Core.Extensions;

public static class ChatBuilderExtension
{
    public static IMemoryServiceBuilder WithOpenAI(
        this IKernelMemoryBuilder builder,
        string apiKey,
        string? organization = null,
        ITextTokenizer? textGenerationTokenizer = null,
        ITextTokenizer? textEmbeddingTokenizer = null,
        ILoggerFactory? loggerFactory = null,
        bool onlyForRetrieval = false,
        HttpClient? httpClient = null)
    {
        textGenerationTokenizer ??= new DefaultGPTTokenizer();
        textEmbeddingTokenizer ??= new DefaultGPTTokenizer();

        var openAIConfig = new OpenAIConfig
        {
            TextModel = DefaultTextModel,
            TextModelMaxTokenTotal = DefaultTextModelMaxToken,
            EmbeddingModel = DefaultEmbeddingModel,
            EmbeddingModelMaxTokenTotal = DefaultEmbeddingModelMaxToken,
            APIKey = apiKey,
            OrgId = organization
        };
        openAIConfig.Validate();

        builder.Services.AddOpenAITextEmbeddingGeneration(openAIConfig, textEmbeddingTokenizer, httpClient);
        builder.Services.AddOpenAITextGeneration(openAIConfig, textGenerationTokenizer, httpClient);

        if (!onlyForRetrieval)
        {
            builder.AddIngestionEmbeddingGenerator(new OpenAITextEmbeddingGenerator(
                config: openAIConfig,
                textTokenizer: textEmbeddingTokenizer,
                loggerFactory: loggerFactory,
                httpClient: httpClient));
        }

        return builder;
    }
}
