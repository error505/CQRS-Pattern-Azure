using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;

public class UpdateProcessor
{
    private readonly ILogger _logger;
    private readonly CosmosClient _cosmosClient;
    private readonly Container _commandDbContainer;
    private readonly Container _queryDbContainer;

    public UpdateProcessor(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<UpdateProcessor>();

        // Initialize Cosmos DB clients and containers
        string cosmosDbConnectionString = Environment.GetEnvironmentVariable("CosmosDBConnectionString");
        _cosmosClient = new CosmosClient(cosmosDbConnectionString);

        // Container for Command DB
        _commandDbContainer = _cosmosClient.GetContainer("CommandDB", "DataContainer");
        // Container for Query DB
        _queryDbContainer = _cosmosClient.GetContainer("QueryDB", "DataContainer");
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
                // Update Command DB
                await _commandDbContainer.UpsertItemAsync(data);
                _logger.LogInformation("Item updated in Command DB.");

                // Also update Query DB to ensure consistency
                await _queryDbContainer.UpsertItemAsync(data);
                _logger.LogInformation("Item updated in Query DB.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating databases: {ex.Message}");
            }
        }
    }
}
