using System;
using System.Threading.Tasks;
using ESH.Abstractions.Models;
using Microsoft.Azure.Cosmos;

namespace FunctionApp.Services
{
    class CosmosDbHelper
    {
        private readonly string EndpointUri = Environment.GetEnvironmentVariable("EndpointUriCosmosdb");
        private readonly string PrimaryKey = Environment.GetEnvironmentVariable("PrimaryKeyCosmosDb");
        private readonly string databaseId = "OrderDatabase";
        private readonly string containerId = "OrderContainer";

        private CosmosClient _cosmosClient;
        private Database _database;
        private Container _container;

        public async Task Init()
        {
            _cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);

            await CreateDatabaseAsync();
            await CreateContainerAsync();
        }

        public async Task Additem(Order order)
        {
            ItemResponse<Order> response = await _container.CreateItemAsync(order, new PartitionKey(order.Id2));
            Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n",
                response.Resource.Id, response.RequestCharge);
        }

        private async Task CreateDatabaseAsync()
        {
            _database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            Console.WriteLine("Created Database: {0}\n", _database.Id);
        }

        private async Task CreateContainerAsync()
        {
            _container = await _database.CreateContainerIfNotExistsAsync(containerId, "/Id2");
            Console.WriteLine("Created Container: {0}\n", _container.Id);
        }
    }
}
