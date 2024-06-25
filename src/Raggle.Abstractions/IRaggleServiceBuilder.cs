using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Raggle.Abstractions;

public interface IRaggleServiceBuilder
{
    IKernelBuilder KernelBuilder { get; set; }
    IKernelMemoryBuilder MemoryBuilder { get; set; }
}
