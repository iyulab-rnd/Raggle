﻿using Raggle.Driver.AzureBlob;
using Raggle.Driver.LiteDB;
using Raggle.Driver.LocalDisk;
using Raggle.Driver.Qdrant;

namespace Raggle.Server.Configurations;

public enum VectorStorageTypes
{
    LiteDB,
    Qdrant
}

public enum DocumentStorageTypes
{
    LocalDisk,
    AzureBlob
}

public partial class RaggleStorageConfig
{
    public VectorStorageConfig Vectors { get; set; } = new VectorStorageConfig();

    public DocumentStorageConfig Documents { get; set; } = new DocumentStorageConfig();

    public class VectorStorageConfig
    {
        public VectorStorageTypes Type { get; set; }
        public LiteDBConfig LiteDB { get; set; } = new LiteDBConfig();
        public QdrantConfig Qdrant { get; set; } = new QdrantConfig();
    }

    public class DocumentStorageConfig
    {
        public DocumentStorageTypes Type { get; set; }
        public LocalDiskConfig LocalDisk { get; set; } = new LocalDiskConfig();
        public AzureBlobConfig AzureBlob { get; set; } = new AzureBlobConfig();
    }
}