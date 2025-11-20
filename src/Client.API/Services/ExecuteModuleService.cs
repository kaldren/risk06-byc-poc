using Azure.Storage.Queues;
using Client.API.Models;
using System.Text.Json;

namespace Client.API.Services;

public class ExecuteModuleService : IExecuteModuleService
{
    private readonly QueueClient _queueClient;

    public ExecuteModuleService()
    {
        // Configure queue
        string queueName = "module-execution-queue";

        // TODO: Switch to Azure and real storage account
        _queueClient = 
            new QueueClient("AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;", queueName);
    }

    public Task EnqueueModuleForExecutionAsync(ExecuteModuleRequest module)
    {
        // Put new message in queue
        _queueClient.CreateIfNotExists();
        _queueClient.SendMessage(JsonSerializer.Serialize(module));

        return Task.CompletedTask;
    }
}
