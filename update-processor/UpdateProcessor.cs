using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using Microsoft.Azure.Cosmos;

public class UpdateProcessor
{
    private readonly ILogger _logger;
    private readonly CosmosClient _cosmosClient;
    private readonly Container _container;

    public UpdateProcessor(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<UpdateProcessor>();
        string cosmosDbConnectionString = Environment.GetEnvironmentVariable("CosmosDBConnectionString");
        _cosmosClient = new CosmosClient(cosmosDbConnectionString);
        _container = _cosmosClient.GetContainer("CommandDB", "DataContainer");
    }

    [Function("UpdateProcessor")]
    public async Task Run([EventHubTrigger("cqrsEventHub", Connection = "EventHubConnectionString")] string[] events)
    {
        foreach (var eventData in events)
        {
            var json = eventData;
            dynamic data = JsonConvert.DeserializeObject(json);

            try
            {
                await _container.UpsertItemAsync(data);
                _logger.LogInformation("Item updated in CommandDB.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating item: {ex.Message}");
            }
        }
    }
}
