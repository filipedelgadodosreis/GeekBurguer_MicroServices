using GeekBurger.UI.Contract;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTestTopic
{
    static class Program
    {
        const string ServiceBusConnectionString = "Endpoint=sb://geekburgerpayment.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=A6/Nc3xMfiPRf9Owj0mJ9YwS7ThtqhfwSmz9osUyOPM=";
        const string TopicName = "processpayment";
        static ITopicClient topicClient;

        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            topicClient = new TopicClient(ServiceBusConnectionString, TopicName);

            Console.WriteLine("======================================================");
            Console.WriteLine("Press ENTER key to exit after sending all the messages.");
            Console.WriteLine("======================================================");

            // Send messages.
            await SendMessagesAsync();
            await topicClient.CloseAsync();

            Console.ReadKey();

        
        }

        static async Task SendMessagesAsync()
        {
            try
            {
                var order = SetOrder();
                var jsonMessage = JsonConvert.SerializeObject(order);

                byte[] orderByteArray = Encoding.UTF8.GetBytes(jsonMessage);

                Message message = new Message
                {
                    Body = orderByteArray,
                    MessageId = Guid.NewGuid().ToString(),
                    Label = order.OrderId.ToString()
                };

                Console.WriteLine($"Sending message: {jsonMessage}");

                await topicClient.SendAsync(message);

            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }

        static Order SetOrder()
        {
            var order = new Order();

            order.OrderId = Guid.NewGuid();
            order.StoreId = Guid.NewGuid();

            order.Products = new List<ProductMessage>
            {
                SetProducts()
            };

            order.ProductionIds = new List<Guid>();

            order.ProductionIds.Add(Guid.NewGuid());
            order.ProductionIds.Add(Guid.NewGuid());

            return order;

        }

        static ProductMessage SetProducts()
        {
            var product = new ProductMessage();

            product.ProductId = Guid.NewGuid();
            //product.Price = 10.2;

            return product;
        }
    }

    public class Order
    {
        public Guid OrderId { get; set; }

        public Guid StoreId { get; set; }

        public string Total { get; set; }

        public List<ProductMessage> Products { get; set; }

        public List<Guid> ProductionIds { get; set; }

    }
}
