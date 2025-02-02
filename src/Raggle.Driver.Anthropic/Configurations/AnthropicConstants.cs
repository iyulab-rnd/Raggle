﻿namespace Raggle.Driver.Anthropic.Configurations;

/// <summary>
/// <see href="https://docs.anthropic.com/en/api/getting-started">Anthropic API Documentation</see>
/// </summary>
internal class AnthropicConstants
{
    internal const string DefaultEndPoint = "https://api.anthropic.com/v1/";
    internal const string ApiKeyHeaderName = "x-api-key";
    internal const string VersionHeaderName = "anthropic-version";
    internal const string VersionHeaderValue = "2023-06-01";

    internal const string PostMessagesPath = "messages";

    /// <summary>
    /// <see href="https://docs.anthropic.com/en/docs/about-claude/models#model-names"/>
    /// </summary>
    internal static readonly IEnumerable<string> PredefinedModelIds =
    [
        // chat completion models
        "claude-3-5-sonnet-latest",
        "claude-3-5-haiku-latest",
        "claude-3-opus-latest",
    ];
}
