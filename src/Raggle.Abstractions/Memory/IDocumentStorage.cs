﻿namespace Raggle.Abstractions.Memory;

public interface IDocumentStorage : IDisposable
{
    Task<IEnumerable<string>> GetCollectionListAsync(
        CancellationToken cancellationToken = default);

    Task<bool> ExistCollectionAsync(
        string collectionName,
        CancellationToken cancellationToken = default);

    Task CreateCollectionAsync(
        string collectionName,
        CancellationToken cancellationToken = default);

    Task DeleteCollectionAsync(
        string collectionName,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<DocumentProfile>> FindDocumentsAsync(
        string collectionName,
        MemoryFilter? filter = null,
        CancellationToken cancellationToken = default);

    Task<bool> ExistDocumentAsync(
        string collectionName,
        string documentId,
        CancellationToken cancellationToken = default);

    Task<DocumentProfile> UpsertDocumentAsync(
        string collectionName,
        DocumentProfile document,
        Stream? content = null,
        CancellationToken cancellationToken = default);

    Task DeleteDocumentAsync(
        string collectionName,
        string documentId,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<string>> GetDocumentFilesAsync(
        string collectionName,
        string documentId,
        CancellationToken cancellationToken = default);

    Task<bool> ExistDocumentFileAsync(
        string collectionName,
        string documentId,
        string filePath,
        CancellationToken cancellationToken = default);

    Task<Stream> ReadDocumentFileAsync(
        string collectionName,
        string documentId,
        string filePath,
        CancellationToken cancellationToken = default);

    Task WriteDocumentFileAsync(
        string collectionName,
        string documentId,
        string filePath,
        Stream content,
        bool overwrite = false,
        CancellationToken cancellationToken = default);

    Task DeleteDocumentFileAsync(
        string collectionName,
        string documentId,
        string filePath,
        CancellationToken cancellationToken = default);
}
