using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using FunctionApp.Services;
using ESH.Abstractions.Models;

namespace FunctionApp.Functions
{
    public static class DeliveryOrderFunction
    {
        [FunctionName("DeliveryOrderFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var order = JsonConvert.DeserializeObject<Order>(requestBody);

            log.LogInformation($"{order.Id}");
            foreach (var item in order.Items)
            {
                log.LogInformation(item.Name);
                log.LogInformation(item.Count);
            }

            var cosmosDbHelper = new CosmosDbHelper();

            await cosmosDbHelper.Init();

            await cosmosDbHelper.Additem(order);

            return new OkObjectResult("Function is finished");
        }
    }
}
