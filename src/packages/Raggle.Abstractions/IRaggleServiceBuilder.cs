using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;

namespace Raggle.Abstractions;

public interface IRaggleServiceBuilder
{
    IKernelBuilder KernelBuilder { get; set; }
    IKernelMemoryBuilder MemoryBuilder { get; set; }
}
