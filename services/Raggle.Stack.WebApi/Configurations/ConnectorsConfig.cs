﻿using Raggle.Connectors.Anthropic.Configurations;
using Raggle.Connectors.Ollama.Configurations;
using Raggle.Connectors.OpenAI.Configurations;

namespace Raggle.Stack.WebApi.Configurations;

public partial class ConnectorsConfig
{
    public OpenAIConfig OpenAI { get; set; } = new OpenAIConfig();

    public AnthropicConfig Anthropic { get; set; } = new AnthropicConfig();

    public OllamaConfig Ollama { get; set; } = new OllamaConfig();
}
