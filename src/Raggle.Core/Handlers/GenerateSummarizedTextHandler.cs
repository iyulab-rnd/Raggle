﻿using Raggle.Abstractions.AI;
using Raggle.Abstractions.Memory;
using Raggle.Abstractions.Memory.Document;
using Raggle.Abstractions.Messages;
using Raggle.Core.Document;
using Raggle.Core.Utils;
using System.Text;

namespace Raggle.Core.Handlers;

public class GenerateSummarizedTextHandler : IPipelineHandler
{
    private readonly IDocumentStorage _documentStorage;
    private readonly IChatCompletionService _chatService;
    private readonly ChatCompletionOptions _chatOptions;

    public GenerateSummarizedTextHandler(
        IDocumentStorage documentStorage,
        IChatCompletionService chatService,
        ChatCompletionOptions chatOptions)
    {
        _documentStorage = documentStorage;
        _chatService = chatService;
        _chatOptions = chatOptions;
    }

    public async Task<DataPipeline> ProcessAsync(DataPipeline pipeline, CancellationToken cancellationToken)
    {
        var chunkFiles = await GetDocumentChunkFilesAsync(pipeline, cancellationToken);
        foreach (var chunkFile in chunkFiles)
        {
            var chunk = await GetDocumentChunkAsync(pipeline, chunkFile, cancellationToken);
            if (string.IsNullOrWhiteSpace(chunk.RawText))
                throw new InvalidOperationException("No text content found in the document chunk.");

            var answer = await GenerateSummarizedTextAsync(chunk.RawText, cancellationToken);
            chunk.SummarizedText = answer;
            await UpsertDocumentChunkAsync(pipeline, chunk, cancellationToken);
        }

        return pipeline;
    }

    #region Private Methods

    private async Task<IEnumerable<string>> GetDocumentChunkFilesAsync(DataPipeline pipeline, CancellationToken cancellationToken)
    {
        var filePaths = await _documentStorage.GetDocumentFilesAsync(
            collectionName: pipeline.Document.CollectionName,
            documentId: pipeline.Document.DocumentId,
            cancellationToken: cancellationToken);
        return filePaths.Where(x => x.EndsWith(DocumentFileHelper.ChunkedFileExtension));
    }

    private async Task<DocumentChunk> GetDocumentChunkAsync(DataPipeline pipeline, string chunkFilePath, CancellationToken cancellationToken)
    {
        var chunkStream = await _documentStorage.ReadDocumentFileAsync(
            pipeline.Document.CollectionName,
            pipeline.Document.DocumentId,
            chunkFilePath,
            cancellationToken);
        return JsonDocumentSerializer.Deserialize<DocumentChunk>(chunkStream);
    }

    private async Task UpsertDocumentChunkAsync(DataPipeline pipeline, DocumentChunk chunk, CancellationToken cancellationToken)
    {
        var filename = DocumentFileHelper.GetChunkedFileName(pipeline.Document.FileName, chunk.Index);
        var chunkStream = JsonDocumentSerializer.SerializeToStream(chunk);
        await _documentStorage.WriteDocumentFileAsync(
            pipeline.Document.CollectionName,
            pipeline.Document.DocumentId,
            filename,
            chunkStream,
            overwrite: true,
            cancellationToken);
    }

    private async Task<string> GenerateSummarizedTextAsync(string text, CancellationToken cancellationToken)
    {
        var history = new ChatHistory();
        history.AddUserMessage(new TextContentBlock
        {
            Text = $"Summarize This:\n\n{text}",
        });
        _chatOptions.System = GetSystemInstructionPrompt();
        var response = await _chatService.ChatCompletionAsync(history, _chatOptions);
        if (response.State == ChatResponseState.Stop)
        {
            var textAnswer = new StringBuilder();
            foreach (var content in response.Contents)
            {
                if (content is TextContentBlock textContent)
                {
                    textAnswer.AppendLine(textContent.Text);
                }
            }
            var answer = textAnswer.ToString();
            if (string.IsNullOrWhiteSpace(answer))
            {
                throw new InvalidOperationException("Failed to generate questions.");
            }
            return answer;
        }
        else
        {
            throw new InvalidOperationException("Failed to generate questions.");
        }
    }

    private static string? GetSystemInstructionPrompt()
    {
        return """
        You are an AI designed to accurately summarize text without adding or inferring information.

        [SUMMARIZATION RULES]
        - Summarize only the provided text without adding or inferring information.
        - Use short, clear, complete sentences.
        - Eliminate redundancy and repetition.
        - Do not include these phrases:
            - This article
            - This document
            - This page

        [EXAMPLES]
        Original: "Hello, how are you?"
        Summary: "Hello."

        Original: "The quick brown fox jumps over the lazy dog."
        Summary: "A fox jumps over a dog."
        [END EXAMPLES]
        """;
    }

    #endregion
}