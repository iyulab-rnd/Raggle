using Raggle.Abstractions.Attributes;

namespace Raggle.Core.Options.Platforms;

public enum AzureAIModel
{
    [EnumString("gpt-3.5-turbo")]
    GPT3_5_Turbo,
    [EnumString("gpt-4")]
    GPT4,
    [EnumString("gpt-4o")]
    GPT4o,
}

public enum AzureAIEmbeddingModel
{
    [EnumString("text-davinci-003")]
    TextDavinci003,
    [EnumString("text-davinci-002")]
    TextDavinci002,
    [EnumString("text-davinci-001")]
    TextDavinci001,
}

public class AzureAIOption
{
    public AzureAIModel ChatModel { get; set; } = AzureAIModel.GPT4o;
    public AzureAIEmbeddingModel EmbeddingModel { get; set; } = AzureAIEmbeddingModel.TextDavinci003;
    public string ApiKey { get; set; } = string.Empty;
}
