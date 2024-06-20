namespace Raggle.Core.Options;

public enum OpenAIChatModel
{

}

public class OpenAIOption
{
    public const string PROPERTY_NAME = "OpenAI";
    public required  OpenAIChatModel ModelName { get; set; }
    public required string ApiKey { get; set; }
}
