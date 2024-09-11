using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;

public class QueryHandler
{
    private readonly ILogger _logger;
    private readonly CosmosClient _cosmosClient;
    private readonly Container _container;

    public QueryHandler(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<QueryHandler>();
        string cosmosDbConnectionString = Environment.GetEnvironmentVariable("CosmosDBConnectionString");
        _cosmosClient = new CosmosClient(cosmosDbConnectionString);
        _container = _cosmosClient.GetContainer("QueryDB", "DataContainer");
    }

    [Function("QueryHandler")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "query/get")] HttpRequestData req)
    {
        _logger.LogInformation("QueryHandler function processed a request.");

        string id = req.Query["id"];
        if (string.IsNullOrEmpty(id))
        {
            var badResponse = req.CreateResponse(System.Net.HttpStatusCode.BadRequest);
            await badResponse.WriteStringAsync("Invalid ID.");
            return badResponse;
        }

        try
        {
            var sqlQueryText = $"SELECT * FROM c WHERE c.id = '{id}'";
            var queryDefinition = new QueryDefinition(sqlQueryText);
            var queryResultSetIterator = _container.GetItemQueryIterator<dynamic>(queryDefinition);

            while (queryResultSetIterator.HasMoreResults)
            {
                var response = await queryResultSetIterator.ReadNextAsync();
                if (response.Resource.Count > 0)
                {
                    var okResponse = req.CreateResponse(System.Net.HttpStatusCode.OK);
                    await okResponse.WriteStringAsync(JsonConvert.SerializeObject(response.Resource.First()));
                    return okResponse;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error querying data: {ex.Message}");
            var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync("Error querying data.");
            return errorResponse;
        }

        var notFoundResponse = req.CreateResponse(System.Net.HttpStatusCode.NotFound);
        await notFoundResponse.WriteStringAsync("Data not found.");
        return notFoundResponse;
    }
}
