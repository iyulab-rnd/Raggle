using Raggle.Core.Options.Platforms;
using Spectre.Console;

namespace Raggle.Console.UI.Setup;

public class OpenAISetup
{
    public OpenAIOption Setup()
    {
        var apiKey = AnsiConsole.Ask<string>("Enter your OpenAI API key: ");
        var textModel = AnsiConsole.Prompt(
            new SelectionPrompt<OpenAITextModel>()
                .Title("Select the OpenAI model")
                .PageSize(10)
                .AddChoices(OpenAITextModel.GPT_4o, OpenAITextModel.GPT_4_Turbo, OpenAITextModel.GPT_4, OpenAITextModel.GPT_3_5_Turbo)
        );
        var embeddingModel = AnsiConsole.Prompt(
            new SelectionPrompt<OpenAIEmbeddingModel>()
                .Title("Select the OpenAI model")
                .PageSize(10)
                .AddChoices(OpenAIEmbeddingModel.Text_Embedding_3_Large, OpenAIEmbeddingModel.Text_Embedding_3_Small, OpenAIEmbeddingModel.Text_Embedding_Ada_002)
        );
        
        return new OpenAIOption
        {
            ApiKey = apiKey,
            TextModel = textModel,
            EmbeddingModel = embeddingModel
        };
    }
}