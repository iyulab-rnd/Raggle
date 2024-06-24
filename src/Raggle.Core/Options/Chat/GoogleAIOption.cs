namespace Raggle.Core.Options.Chat;

public enum GoogleChatModel
{
    
}

public enum GoogleEmbeddingModel
{
    
}

public class GoogleAIOption
{
    public required GoogleChatModel ChatModel { get; set; } = GoogleChatModel.Davinci;
    public required GoogleEmbeddingModel EmbeddingModel { get; set; } = GoogleEmbeddingModel.Davinci;
    public required string Apikey { get; set; }
}
