using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace QueueApp
{
    class Program
    {   
        private const string ConnectionString = "DefaultEndpointsProtocol=https;EndpointSuffix=core.windows.net;AccountName=storagesmrtest;AccountKey=FxDu25NT6TnLl1FFJElMeci3t7idWbKb/lUSkb5wUkffhpFKe+/gYV3/9yZr8cUbZqrGwDmm7eU/Hatr1qf4lQ==";

        static async Task Main(string[] args)
        {
        if (args.Length > 0)
        {
            string value = String.Join(" ", args);
            await SendArticleAsync(value);
            Console.WriteLine($"Sent: {value}");
        } else{

               string value = await ReceiveArticleAsync();
                Console.WriteLine($"Received {value}");
        }
        }

        static async Task SendArticleAsync(string newsMessage) {

            CloudStorageAccount storage = CloudStorageAccount.Parse(ConnectionString);
            
            CloudQueueClient objetito = storage.CreateCloudQueueClient();

            CloudQueue  queue = objetito.GetQueueReference("newsqueue");

            bool booleate = await queue.CreateIfNotExistsAsync();

            if (booleate) {

                Console.WriteLine("Cola creada");
            } else{
                Console.WriteLine("Pues no :)");
            }

            CloudQueueMessage mensaje = new CloudQueueMessage(newsMessage);

            await queue.AddMessageAsync(mensaje);
        }

        static async Task<string> ReceiveArticleAsync(){

            CloudStorageAccount storage = CloudStorageAccount.Parse(ConnectionString);
            
            CloudQueueClient objetito = storage.CreateCloudQueueClient();

            CloudQueue  queue = objetito.GetQueueReference("newsqueue");

            bool exists = await queue.ExistsAsync();

            if (exists){

                CloudQueueMessage message = await queue.GetMessageAsync();
                if (message != null)
                {
                    string newsMessage = message.AsString;
                    await queue.DeleteMessageAsync(message);
                    return newsMessage;
                }

            }

            return "<queue empty or not created>";

        }
    }
}
