﻿using System.Text.Json.Serialization;

namespace Raggle.Driver.Anthropic.ChatCompletion.Models;

internal class CacheControl
{
    /// <summary>
    /// "ephemeral" only
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; } = "ephemeral";
}