﻿using Microsoft.Extensions.Azure;
using IronHive.Abstractions;
using IronHive.Abstractions.ChatCompletion;
using IronHive.Abstractions.Embedding;
using IronHive.Connectors.Anthropic;
using IronHive.Connectors.Anthropic.Configurations;
using IronHive.Connectors.OpenAI;
using IronHive.Connectors.OpenAI.Configurations;
using IronHive.Core;

namespace IronHive.Stack.WebApi;

public static class Extensions
{
    /// <summary>
    /// 임시 메서드
    /// </summary>
    public static void AddHiveMind(this WebApplicationBuilder builder)
    {
        builder.Services.AddHiveServices();

        var container = new HiveServiceContainer();
        var o_config = new OpenAIConfig
        {
            ApiKey = "",
        };
        container.RegisterKeyedService<IChatCompletionConnector>("openai", new OpenAIChatCompletionConnector(o_config));
        container.RegisterKeyedService<IEmbeddingConnector>("openai", new OpenAIEmbeddingConnector(o_config));

        var a_config = new AnthropicConfig
        {
            ApiKey = "",
        };
        container.RegisterKeyedService<IChatCompletionConnector>("anthropic", new AnthropicChatCompletionConnector(a_config));

        var g_config = new OpenAIConfig
        {
            BaseUrl = "https://generativelanguage.googleapis.com/v1beta/openai/",
            ApiKey = ""
        };
        container.RegisterKeyedService<IChatCompletionConnector>("gemini", new OpenAIChatCompletionConnector(g_config));

        var l_config = new OpenAIConfig
        {
            BaseUrl = "http://172.30.1.53:8080/v1-openai/",
            ApiKey = ""
        };
        container.RegisterKeyedService<IChatCompletionConnector>("iyulab", new OpenAIChatCompletionConnector(l_config));

        builder.Services.AddSingleton<IHiveServiceContainer>(container);
    }
}
