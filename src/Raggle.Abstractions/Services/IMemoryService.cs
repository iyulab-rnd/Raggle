
namespace Raggle.Abstractions.Services;

public interface IMemoryService
{
    string GenerateDocumentId(string cotent);
    Task<string> GetInformationAsync(string query, int limit = 10, double minRelevance = 0.5);
    Task<string> MemorizeDocumentAsync(string filePath);
    Task<string[]> MemorizeDocumentsAsync(IEnumerable<string> filePaths);
    Task<string> MemorizeTextAsync(string text, string documentId);
    Task<string> MemorizeWebPageAsync(string url);
    Task<string[]> MemorizeWebPagesAsync(IEnumerable<string> urls);
    Task UnMemorizeAsync(string documentId);
}
