﻿namespace Raggle.Driver.Ollama.Configurations;

internal class OllamaConstants
{
    internal const string DefaultEndPoint = "http://localhost:11434/";

    internal const string GetModelListPath = "/api/tags";
    internal const string PostChatCompletionPath = "/api/chat";
    internal const string PostEmbeddingPath = "/api/embed";
}
