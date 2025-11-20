using Client.API.Models;

namespace Client.API.Services;

public interface IExecuteModuleService
{
    Task EnqueueModuleForExecutionAsync(ExecuteModuleRequest module);
}
