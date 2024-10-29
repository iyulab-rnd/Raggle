﻿using Raggle.Abstractions.Engines;
using Tiktoken;

namespace Raggle.Core.Tokenizers;

// 임시
public class TiktokenTokenizer : ITextTokenizer
{
    private readonly Encoder _tokenizer = ModelToEncoder.For("gpt-4o");

    /// <inheritdoc />
    public int CountTokens(string text)
    {
        return _tokenizer.CountTokens(text);
    }

    /// <inheritdoc />
    public IReadOnlyList<int> Encode(string text)
    {
        return (IReadOnlyList<int>)_tokenizer.Encode(text);
    }

    /// <inheritdoc />
    public string Decode(IReadOnlyList<int> values)
    {
        return _tokenizer.Decode(values);
    }
}
