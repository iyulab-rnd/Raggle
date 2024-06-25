using Raggle.Abstractions.Attributes;

namespace Raggle.Core.Options.Chat;

public enum OpenAIChatModel
{
    [EnumString("gpt-3.5-turbo")]
    GPT3_5_Turbo,
    [EnumString("gpt-4")]
    GPT4,
    [EnumString("gpt-4o")]
    GPT4o,
}

public enum OpenAIEmbeddingModel
{
    [EnumString("text-davinci-003")]
    TextDavinci003,
    [EnumString("text-davinci-002")]
    TextDavinci002,
    [EnumString("text-davinci-001")]
    TextDavinci001,
}

public class OpenAIOption
{
    public required string ApiKey { get; set; } = string.Empty;
    public OpenAIChatModel ChatModel { get; set; } = OpenAIChatModel.GPT4o;
    public OpenAIEmbeddingModel EmbeddingModel { get; set; } = OpenAIEmbeddingModel.TextDavinci003;
    public int ChatModelMaxTokenTotal { get; set; } = 2048;
    public int EmbeddingModelMaxTokenTotal { get; set; } = 8191;
    public int MaxRetries { get; set; } = 10;
}
