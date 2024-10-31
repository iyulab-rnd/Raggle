﻿using System.Text.Json;
using System.Text;
using System.Net.Http.Json;
using Raggle.Connector.Anthropic.ChatCompletion.Models;
using Raggle.Connector.Anthropic.Configurations;
using Raggle.Connector.Anthropic.Base;

namespace Raggle.Connector.Anthropic.ChatCompletion;

internal class AnthropicChatCompletionClient : AnthropicClientBase
{
    internal AnthropicChatCompletionClient(AnthropicConfig config) : base(config) { }

    internal AnthropicChatCompletionClient(string apiKey) : base(apiKey) { }

    /// <summary>
    /// Anthropic does not provide a way to list available models.
    /// so this list is hardcoded models, writed at 2024-09-02.
    /// </summary>
    internal IEnumerable<AnthropicChatCompletionModel> GetChatCompletionModels()
    {
        var models = GetModels();
        return models.Select(m => new AnthropicChatCompletionModel
        {
            ModelId = m.ModelId,
        });
    }

    internal async Task<MessagesResponse> PostMessagesAsync(MessagesRequest request)
    {
        request.Stream = false;
        var json = JsonSerializer.Serialize(request, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(AnthropicConstants.PostMessagesPath, content);
        response.EnsureSuccessStatusCode();
        var message = await response.Content.ReadFromJsonAsync<MessagesResponse>(_jsonOptions)
            ?? throw new InvalidOperationException("Failed to deserialize response.");
        return message;
    }

    internal async IAsyncEnumerable<StreamingMessagesResponse> PostStreamingMessagesAsync(MessagesRequest request)
    {
        request.Stream = true;
        var json = JsonSerializer.Serialize(request, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(AnthropicConstants.PostMessagesPath, content);
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(stream);

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(line) || !line.StartsWith("data"))
                continue;

            var data = line.Substring("data:".Length).Trim();
            if (!data.StartsWith('{') || !data.EndsWith('}'))
                continue;

            var message = JsonSerializer.Deserialize<StreamingMessagesResponse>(data, _jsonOptions);
            if (message != null)
            {
                yield return message;
            }
        }
    }

}