using Microsoft.KernelMemory;
using Raggle.Abstractions.Services;
using System.Security.Cryptography;
using System.Text;

namespace Raggle.Core.Services;

public class MemoryService : IMemoryService
{
    private readonly IKernelMemory _memory;

    public MemoryService(IKernelMemory kernelMemory)
    {
        _memory = kernelMemory;
    }

    public async Task<string> MemorizeDocumentAsync(string filePath)
    {
        var documentId = GenerateDocumentId(filePath);
        var isExist = await _memory.IsDocumentReadyAsync(documentId);
        return isExist 
            ? documentId
            : await _memory.ImportDocumentAsync(filePath, documentId);
    }

    public async Task<string[]> MemorizeDocumentsAsync(string[] filePaths)
    {
        return await Task.WhenAll(filePaths.Select(MemorizeDocumentAsync));
    }

    public async Task<string> MemorizeTextAsync(string text, string documentId)
    {
        return await _memory.ImportTextAsync(text, documentId);
    }

    public async Task<string> MemorizeWebPageAsync(string url)
    {
        var documentId = GenerateDocumentId(url);
        var isExist = await _memory.IsDocumentReadyAsync(documentId);
        return isExist
            ? documentId
            : await _memory.ImportWebPageAsync(url, documentId);
    }

    public async Task<string[]> MemorizeWebPagesAsync(string[] urls)
    {
        return await Task.WhenAll(urls.Select(MemorizeWebPageAsync));
    }

    public async Task<string> GetFactsAsync(string query, int limit = 10, double minRelevance = 0.5)
    {
        var memories = await _memory.SearchAsync(query, limit: limit, minRelevance: minRelevance);
        return memories.Results.SelectMany(m => m.Partitions)
            .Aggregate("", (sum, chunk) => sum + chunk.Text + "\n")
            .Trim();
    }

    private static string GenerateDocumentId(string cotent)
    {
        var encryption = SHA256.HashData(Encoding.UTF8.GetBytes(cotent));
        return Convert.ToHexString(encryption).ToUpperInvariant();
    }
}
