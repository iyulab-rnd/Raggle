using Microsoft.KernelMemory;
using Microsoft.SemanticKernel.ChatCompletion;
using Raggle.Abstractions;
using Raggle.Core.Options.Prompts;
using System.Text;

namespace Raggle.Core;

public class RaggleService : IRaggleService
{
    private const string DEFAULT_INDEX = "thisistest";
    private readonly IChatCompletionService _chat;
    private readonly IKernelMemory _memory;
    private readonly DefaultPromptOption _prompt;
    private readonly ChatHistory history = new("you are bot");

    public RaggleService(
        IChatCompletionService chatService, 
        IKernelMemory kernelMemory,
        DefaultPromptOption? prompt = null)
    {
        _memory = kernelMemory;
        _chat = chatService;
        _prompt = prompt ?? new DefaultPromptOption();
    }

    public async Task<string> MemorizeTextAsync(string documentId, string text, string? index = null)
    {
        index ??= DEFAULT_INDEX;
        return await _memory.ImportTextAsync(text: text, documentId: documentId, index: index);
    }

    public async Task<string> MemorizeDocumentAsync(string documentId, string filePath, string? index = null)
    {
        index ??= DEFAULT_INDEX;
        var isExist = await _memory.IsDocumentReadyAsync(documentId: documentId, index: index);
        return isExist
            ? documentId
            : await _memory.ImportDocumentAsync(filePath: filePath, documentId: documentId, index: index);
    }

    public async Task<string> MemorizeWebPageAsync(string documentId, string url, string? index = null)
    {
        index ??= DEFAULT_INDEX;
        var isExist = await _memory.IsDocumentReadyAsync(documentId: documentId, index: index);
        return isExist
            ? documentId
            : await _memory.ImportWebPageAsync(url: url, documentId: documentId, index: index);
    }

    public async Task UnMemorizeAsync(string documentId, string? index = null)
    {
        index ??= DEFAULT_INDEX;
        await _memory.DeleteDocumentAsync(documentId: documentId, index: index);
    }

    public async Task<string> GetInformationAsync(string query, int? limit = null, double? minRelevance = null, string? index = null)
    {
        index ??= DEFAULT_INDEX;
        var memories = await _memory.SearchAsync(
                query: query,
                index: index,
                limit: limit ?? 10, 
                minRelevance: minRelevance ?? 0
            );
        return memories.Results.SelectMany(m => m.Partitions)
            .Aggregate("", (sum, chunk) => sum + chunk.Text + "\n")
            .Trim();
    }

    public async IAsyncEnumerable<string> AskStreamingAsync(string query)
    {
        var information = await GetInformationAsync(query);
        history[0].Content = BuildPrompt(_prompt.SystemPrompt, information);
        history.AddUserMessage(query);
        var reply = new StringBuilder();
        await foreach (var stream in _chat.GetStreamingChatMessageContentsAsync(history))
        {
            var content = stream.Content;
            if (content is not null)
            {
                reply.Append(content);
                yield return content;
            }
        }
        history.AddAssistantMessage(reply.ToString());
    }

    public string BuildPrompt(string system, string information)
    {
        return $"{system}\n\nInformation:\n{information}";
    }
}
