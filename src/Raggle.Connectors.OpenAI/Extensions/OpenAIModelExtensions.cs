﻿using Raggle.Connectors.OpenAI.Base;

namespace Raggle.Connectors.OpenAI.Extensions;

internal static class OpenAIModelExtensions
{
    internal static bool IsChatCompletion(this OpenAIModel model)
    {
        // 최신 모델 및 Alias 모델만 추가

        return model.ID.Equals("o3-mini")
            || model.ID.Equals("o1")
            || model.ID.Equals("o1-mini")
            || model.ID.Equals("gpt-4o-mini")
            || model.ID.Equals("gpt-4o");
            //|| model.ID.Equals("gpt-4-turbo")
            //|| model.ID.Equals("gpt-4");
    }

    internal static bool IsEmbedding(this OpenAIModel model)
    {
        return model.ID.Contains("embedding");
    }

    internal static bool IsTextToImage(this OpenAIModel model)
    {
        return model.ID.Contains("dall-e");
    }

    internal static bool IsTextToSpeech(this OpenAIModel model)
    {
        return model.ID.Contains("tts");
    }

    internal static bool IsAudioToText(this OpenAIModel model)
    {
        return model.ID.Contains("whisper");
    }
}
