using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using OrderItemsReserver;
using System.Threading.Tasks;

namespace FunctionApp.Functions
{
    public static class BlobReservationFunction
    {
        [FunctionName("BlobReservationFunction")]
        public static async Task Run([ServiceBusTrigger("reservationqueue", Connection = "BusConnection")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");

            var blob = new BlobStorage();

            await blob.Init();

            log.LogInformation("Save to blob");

            await blob.Save(myQueueItem);
            log.LogInformation($"OrderReceiverFunction is finished. save to blob");


        }
    }
}
