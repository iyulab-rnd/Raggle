﻿using System.Text.Json.Serialization;

namespace Raggle.Driver.Ollama.Embeddings;

internal class EmbeddingResponse
{
    [JsonPropertyName("model")]
    public required string Model { get; set; }

    [JsonPropertyName("embeddings")]
    public required float[][] Embeddings { get; set; }

    [JsonPropertyName("total_duration")]
    public long TotalDuration { get; set; }

    [JsonPropertyName("load_duration")]
    public long LoadDuration { get; set; }

    [JsonPropertyName("prompt_eval_count")]
    public int PromptEvalCount { get; set; }
}
