using Microsoft.KernelMemory;
using Microsoft.KernelMemory.DocumentStorage.DevTools;
using Microsoft.KernelMemory.FileSystem.DevTools;
using Microsoft.KernelMemory.MemoryStorage.DevTools;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

var openAIApiKey = "";

// Documentation to memorize
var s_documentation = new List<string>()
{
    "https://raw.githubusercontent.com/microsoft/kernel-memory/main/README.md",
    "https://microsoft.github.io/kernel-memory/quickstart",
    "https://microsoft.github.io/kernel-memory/quickstart/configuration",
    "https://microsoft.github.io/kernel-memory/quickstart/start-service",
    "https://microsoft.github.io/kernel-memory/quickstart/python",
    "https://microsoft.github.io/kernel-memory/quickstart/csharp",
    "https://microsoft.github.io/kernel-memory/quickstart/java",
    "https://microsoft.github.io/kernel-memory/quickstart/javascript",
    "https://microsoft.github.io/kernel-memory/quickstart/bash",
    "https://microsoft.github.io/kernel-memory/service",
    "https://microsoft.github.io/kernel-memory/service/architecture",
    "https://microsoft.github.io/kernel-memory/serverless",
    "https://microsoft.github.io/kernel-memory/security/filters",
    "https://microsoft.github.io/kernel-memory/how-to/custom-partitioning",
    "https://microsoft.github.io/kernel-memory/concepts/indexes",
    "https://microsoft.github.io/kernel-memory/concepts/document",
    "https://microsoft.github.io/kernel-memory/concepts/memory",
    "https://microsoft.github.io/kernel-memory/concepts/tag",
    "https://microsoft.github.io/kernel-memory/concepts/llm",
    "https://microsoft.github.io/kernel-memory/concepts/embedding",
    "https://microsoft.github.io/kernel-memory/concepts/cosine-similarity",
    "https://microsoft.github.io/kernel-memory/faq",
    "https://raw.githubusercontent.com/microsoft/semantic-kernel/main/README.md",
    "https://raw.githubusercontent.com/microsoft/semantic-kernel/main/dotnet/README.md",
    "https://raw.githubusercontent.com/microsoft/semantic-kernel/main/python/README.md",
    "https://raw.githubusercontent.com/microsoft/semantic-kernel/main/java/README.md",
    "https://learn.microsoft.com/en-us/semantic-kernel/overview/",
    "https://learn.microsoft.com/en-us/semantic-kernel/get-started/quick-start-guide",
    "https://learn.microsoft.com/en-us/semantic-kernel/agents/"
};

// Set the base directory for the application
var baseDir =  Debugger.IsAttached
    ? @"C:\data\Raggle\test-dir"
    : Directory.GetCurrentDirectory();
Console.WriteLine($"Working directory: {baseDir}");

// Create the configuration directory and save the settings
var configDir = Path.Combine(baseDir, ".raggle");
var dirInfo = Directory.CreateDirectory(configDir);
dirInfo.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
var settings = new 
{
    OpenAI = new 
    { 
        ApiKey = openAIApiKey
    }
};
File.WriteAllText(Path.Combine(configDir, "setting.json"), JsonSerializer.Serialize(settings, new JsonSerializerOptions
{
    WriteIndented = true,
}));

// Collect all files in the base directory
var filesPath = CollectFiles(baseDir);
Console.WriteLine($"Files in the directory: {filesPath.Count}");

// Create the kernel with OpenAI chat completion
var kernel = Kernel.CreateBuilder()
    .AddOpenAIChatCompletion(modelId: "gpt-4o", apiKey: openAIApiKey)
    .Build();

// Memory instance with persistent storage on disk
var memory = new KernelMemoryBuilder()
    .WithOpenAIDefaults(openAIApiKey)
    .WithSimpleVectorDb(new SimpleVectorDbConfig
    {
        Directory = Path.Combine(baseDir,".raggle", "vectors"),
        StorageType = FileSystemTypes.Disk
    })
    .WithSimpleFileStorage(new SimpleFileStorageConfig
    {
        Directory = Path.Combine(baseDir,".raggle", "files"),
        StorageType = FileSystemTypes.Disk
    })
    .Build<MemoryServerless>();

// Memorize some data
Console.WriteLine("# Saving documentation into kernel memory...");
await MemorizeDocuments(memory, s_documentation);

// Infinite chat loop
Console.WriteLine("# Starting chat...");
var chatService = kernel.GetRequiredService<IChatCompletionService>();

// Chat setup
var systemPrompt = """
                        You are a helpful assistant replying to user questions using information from your memory.
                        Reply very briefly and concisely, get to the point immediately. Don't provide long explanations unless necessary.
                        Sometimes you don't have relevant memories so you reply saying you don't know, don't have the information.
                        The topic of the conversation is Kernel Memory (KM) and Semantic Kernel (SK).
                        """;

var chatHistory = new ChatHistory(systemPrompt);

// Start the chat
var assistantMessage = "Hello, how can I help?";
Console.WriteLine($"Copilot> {assistantMessage}\n");
chatHistory.AddAssistantMessage(assistantMessage);

// Infinite chat loop
var reply = new StringBuilder();

while (true)
{
    // Get user message (retry if the user enters an empty string)
    Console.Write("You> ");
    var userMessage = Console.ReadLine()?.Trim();
    if (string.IsNullOrWhiteSpace(userMessage)) { continue; }
    else { chatHistory.AddUserMessage(userMessage); }

    // Recall relevant information from memory
    var longTermMemory = await GetLongTermMemory(memory, userMessage);
    // Console.WriteLine("-------------------------- recall from memory\n{longTermMemory}\n--------------------------");

    // Inject the memory recall in the initial system message
    chatHistory[0].Content = $"{systemPrompt}\n\nLong term memory:\n{longTermMemory}";

    // Generate the next chat message, stream the response
    Console.Write("\nCopilot> ");
    reply.Clear();
    await foreach (StreamingChatMessageContent stream in chatService.GetStreamingChatMessageContentsAsync(chatHistory))
    {
        Console.Write(stream.Content);
        reply.Append(stream.Content);
    }

    chatHistory.AddAssistantMessage(reply.ToString());
    Console.WriteLine("\n");
}


static async Task<string> GetLongTermMemory(IKernelMemory memory, string query, bool asChunks = true)
{
    if (asChunks)
    {
        // Fetch raw chunks, using KM indexes. More tokens to process with the chat history, but only one LLM request.
        SearchResult memories = await memory.SearchAsync(query, limit: 10);
        return memories.Results.SelectMany(m => m.Partitions).Aggregate("", (sum, chunk) => sum + chunk.Text + "\n").Trim();
    }

    // Use KM to generate an answer. Fewer tokens, but one extra LLM request.
    MemoryAnswer answer = await memory.AskAsync(query);
    return answer.Result.Trim();
}

static async Task MemorizeDocuments(IKernelMemory memory, List<string> pages)
{
    await memory.ImportTextAsync("We can talk about Semantic Kernel and Kernel Memory, you can ask any questions, I will try to reply using information from public documentation in Github", documentId: "help");
    foreach (var url in pages)
    {
        var id = GetUrlId(url);
        // Check if the page is already in memory, to avoid importing twice
        if (!await memory.IsDocumentReadyAsync(id))
        {
            await memory.ImportWebPageAsync(url, documentId: id);
        }
    }
}

static string GetUrlId(string url)
{
    return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(url))).ToUpperInvariant();
}

static List<string> CollectFiles(string baseDir)
{
    List<string> files = new List<string>();

    // Get all files in the base directory
    string[] baseFiles = Directory.GetFiles(baseDir);
    files.AddRange(baseFiles);

    // Get all subdirectories in the base directory
    string[] subDirectories = Directory.GetDirectories(baseDir);

    // Recursively collect files from subdirectories
    foreach (string subDir in subDirectories)
    {
        // Exclude the .raggle directory
        if (Path.GetFileName(subDir) == ".raggle")
        {
            continue;
        }
        files.AddRange(CollectFiles(subDir));
    }

    return files;
}
