﻿using Raggle.Driver.Ollama.Base;
using System.Text.Json.Serialization;

namespace Raggle.Driver.Ollama.Embeddings;

internal class EmbeddingRequest
{
    [JsonPropertyName("model")]
    public required string Model { get; set; }

    [JsonPropertyName("input")]
    public required string[] Input { get; set; }

    [JsonPropertyName("options")]
    public ModelOptions? Options { get; set; }

    [JsonPropertyName("truncate")]
    public bool Truncate { get; set; } = true;

    [JsonPropertyName("keep_alive")]
    public string KeepAlive { get; set; } = "5m";
}
