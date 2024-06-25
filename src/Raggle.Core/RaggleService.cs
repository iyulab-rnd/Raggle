using Microsoft.KernelMemory;
using Microsoft.SemanticKernel.ChatCompletion;
using Raggle.Abstractions.Services;
using Raggle.Console.Settings;
using System.Security.Cryptography;
using System.Text;

namespace Raggle.Core;

public class RaggleService : IRaggleService
{
    private readonly IChatCompletionService _chat;
    private readonly IKernelMemory _memory;
    private readonly PromptOption _prompt;

    public RaggleService(
        IChatCompletionService chatService, 
        IKernelMemory kernelMemory,
        PromptOption? prompt = null)
    {
        _memory = kernelMemory;
        _chat = chatService;
        _prompt = prompt ?? new PromptOption();
    }

    public async Task<string> MemorizeTextAsync(string text, string documentId)
    {
        return await _memory.ImportTextAsync(text, documentId);
    }

    public async Task<string> MemorizeDocumentAsync(string filePath)
    {
        var documentId = GenerateDocumentId(filePath);
        var isExist = await _memory.IsDocumentReadyAsync(documentId);
        return isExist
            ? documentId
            : await _memory.ImportDocumentAsync(filePath, documentId);
    }

    public async Task<string[]> MemorizeDocumentsAsync(IEnumerable<string> filePaths)
    {
        return await Task.WhenAll(filePaths.Select(MemorizeDocumentAsync));
    }

    public async Task<string> MemorizeWebPageAsync(string url)
    {
        var documentId = GenerateDocumentId(url);
        var isExist = await _memory.IsDocumentReadyAsync(documentId);
        return isExist
            ? documentId
            : await _memory.ImportWebPageAsync(url, documentId);
    }

    public async Task<string[]> MemorizeWebPagesAsync(IEnumerable<string> urls)
    {
        return await Task.WhenAll(urls.Select(MemorizeWebPageAsync));
    }

    public async Task UnMemorizeAsync(string content)
    {
        var documentId = GenerateDocumentId(content);
        await _memory.DeleteDocumentAsync(documentId);
    }

    public async Task<string> GetInformationAsync(string query, int limit = 10, double minRelevance = 0.5)
    {
        var memories = await _memory.SearchAsync(query, limit: limit, minRelevance: minRelevance);
        return memories.Results.SelectMany(m => m.Partitions)
            .Aggregate("", (sum, chunk) => sum + chunk.Text + "\n")
            .Trim();
    }

    public string GenerateDocumentId(string cotent)
    {
        var encryption = SHA256.HashData(Encoding.UTF8.GetBytes(cotent));
        return Convert.ToHexString(encryption).ToUpperInvariant();
    }
}
