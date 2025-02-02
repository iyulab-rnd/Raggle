﻿using Raggle.Abstractions.Messages;

namespace Raggle.Abstractions.AI;

public class ChatCompletionResponse
{
    public ChatCompletionEndReason? EndReason { get; set; }

    public string? Model { get; set; }

    public Message? Message { get; set; }

    public TokenUsage? TokenUsage { get; set; }
}
