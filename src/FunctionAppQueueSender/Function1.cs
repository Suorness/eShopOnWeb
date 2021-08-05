using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text;
using Microsoft.Azure.ServiceBus;

namespace FunctionAppQueueSender
{
    public static class Function1
    {
        static string ServiceBusConnectionString = Environment.GetEnvironmentVariable("BusConnection");
        const string QueueName = "reservationqueue";
        static IQueueClient queueClient;

        [FunctionName("OrderSenderFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

            try
            {
                var message = new Message(Encoding.UTF8.GetBytes(requestBody));
                Console.WriteLine($"Sending message: {requestBody}");
                await queueClient.SendAsync(message);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }

            await queueClient.CloseAsync();

            return new OkObjectResult("ready");
        }
    }
}
