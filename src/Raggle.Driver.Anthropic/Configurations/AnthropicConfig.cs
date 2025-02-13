﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace Raggle.Driver.Anthropic.Configurations;

/// <summary>
/// Represents the configuration settings required to connect to the Anthropic API.
/// </summary>
public class AnthropicConfig
{
    /// <summary>
    /// Gets or sets the endpoint URL for the OpenAI API.
    /// Default value is "https://api.anthropic.com/".
    /// </summary>
    public string EndPoint { get; set; } = AnthropicConstants.DefaultEndPoint;

    /// <summary>
    /// Gets or sets the API key used for authenticating requests to the Anthropic API.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the version of the Anthropic API being used.
    /// This value is typically set to a <see cref="AnthropicConstants.VersionHeaderValue"/>
    /// </summary>
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the JSON serialization options used for Anthropic API
    /// </summary>
    [JsonIgnore]
    public JsonSerializerOptions JsonOptions { get; set; } = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower) }
    };
}
