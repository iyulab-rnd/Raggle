﻿using System.Text.Json.Serialization;

namespace Raggle.Abstractions.Engines;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "status")]
[JsonDerivedType(typeof(StreamingStopResponse), "stop")]
[JsonDerivedType(typeof(StreamingFilterResponse), "filter")]
[JsonDerivedType(typeof(StreamingLimitResponse), "limit")]
[JsonDerivedType(typeof(StreamingErrorResponse), "error")]
[JsonDerivedType(typeof(StreamingTextResponse), "text_gen")]
[JsonDerivedType(typeof(StreamingToolCallResponse), "tool_call")]
[JsonDerivedType(typeof(StreamingToolUseResponse), "tool_use")]
[JsonDerivedType(typeof(StreamingToolResultResponse), "tool_result")]
[JsonDerivedType(typeof(StreamingMemorySearchResponse), "memory_search")]
[JsonDerivedType(typeof(StreamingMemoryResultResponse), "memory_result")]
public interface IStreamingChatResponse
{
    DateTime TimeStamp { get; set; }
}

public abstract class StreamingChatResponseBase : IStreamingChatResponse
{
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
}

public class StreamingStopResponse : StreamingChatResponseBase { }

public class StreamingFilterResponse : StreamingChatResponseBase { }

public class StreamingLimitResponse : StreamingChatResponseBase { }

public class StreamingErrorResponse : StreamingChatResponseBase
{
    public string? Message { get; set; }
}

public class StreamingTextResponse : StreamingChatResponseBase
{
    public string? Text { get; set; }
}

public class StreamingToolCallResponse : StreamingChatResponseBase
{
    public string? Name { get; set; }
    public string? Argument { get; set; }
}

public class StreamingToolUseResponse : StreamingChatResponseBase
{
    public string? Name { get; set; }
    public string? Argument { get; set; }
}

public class StreamingToolResultResponse : StreamingChatResponseBase
{
    public string? Name { get; set; }
    public object? Result { get; set; }
}

public class StreamingMemorySearchResponse : StreamingChatResponseBase
{
    public string? Query { get; set; }
}

public class StreamingMemoryResultResponse : StreamingChatResponseBase
{
    public string? Query { get; set; }
    public object? Result { get; set; }
}
