using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace OrderItemsReserver
{
    public class BlobStorage
    {
        private static string StorageAccountConnectionString = Environment.GetEnvironmentVariable("BlobConnection");

        private const string ContainerName = "container123";

        public async Task<bool> Init()
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageAccountConnectionString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(ContainerName);
                return await container.CreateIfNotExistsAsync();
            }
            catch (Exception e)
            {
            }

            return false;
        }

        public Task Save(string order)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageAccountConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(ContainerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(Guid.NewGuid().ToString("N"));

            return blockBlob.UploadTextAsync(order);
        }
    }
}
