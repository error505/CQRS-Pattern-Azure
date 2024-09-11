using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventHubs.Producer;
using Azure.Messaging.EventHubs;
using Newtonsoft.Json;

public class CommandHandler
{
    private readonly ILogger _logger;
    private readonly EventHubProducerClient _eventHubClient;

    public CommandHandler(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<CommandHandler>();
        string eventHubConnectionString = Environment.GetEnvironmentVariable("EventHubConnectionString");
        string eventHubName = Environment.GetEnvironmentVariable("EventHubName");
        _eventHubClient = new EventHubProducerClient(eventHubConnectionString, eventHubName);
    }

    [Function("CommandHandler")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "command/insert")] HttpRequestData req)
    {
        _logger.LogInformation("CommandHandler function processed a request.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic data = JsonConvert.DeserializeObject(requestBody);

        if (data == null || data.Data == null)
        {
            var badResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
            await badResponse.WriteStringAsync("Invalid data.");
            return badResponse;
        }

        // Publish event to Event Hub
        using EventDataBatch eventBatch = await _eventHubClient.CreateBatchAsync();
        eventBatch.TryAdd(new EventData(JsonConvert.SerializeObject(data)));

        await _eventHubClient.SendAsync(eventBatch);

        _logger.LogInformation("Data inserted and event published.");

        var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
        await response.WriteStringAsync("Data inserted successfully.");
        return response;
    }
}
