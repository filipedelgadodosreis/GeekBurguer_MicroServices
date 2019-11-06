using ConsoleAppTestTopic.Events;
using EventBus.Abstractions;
using GeekBurger.Ordering.Contract;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceBus;
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

        const string INTEGRATION_EVENT_SUFIX = "IntegrationEvent";

        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            const int numberOfMessages = 10;
            topicClient = new TopicClient(ServiceBusConnectionString, TopicName);

            Console.WriteLine("======================================================");
            Console.WriteLine("Press ENTER key to exit after sending all the messages.");
            Console.WriteLine("======================================================");

            //await SendMessagesAsync(numberOfMessages);

            // Send messages.
            //await SendMessagesAsync();

            PublishEvent(Guid.NewGuid());

            await topicClient.CloseAsync();


            Console.ReadKey();
        }

        static void PublishEvent(Guid Id)
        {
            IntegrationEvent orderPaymentIntegrationEvent;

            orderPaymentIntegrationEvent = new OrderPaymentSuccededIntegrationEvent(Id);

            Publish(orderPaymentIntegrationEvent);
        }

        static void Publish(IntegrationEvent @event)
        {

            try
            {
                var eventName = @event.GetType().Name.Replace(INTEGRATION_EVENT_SUFIX, "");
                var jsonMessage = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(jsonMessage);

                var message = new Message
                {
                    MessageId = Guid.NewGuid().ToString(),
                    Body = body,
                    Label = eventName,
                };

                topicClient.SendAsync(message)
                   .GetAwaiter()
                   .GetResult();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }

        static async Task SendMessagesAsync(int numberOfMessagesToSend)
        {
            try
            {
                var order = SetOrder();
                var jsonMessage = JsonConvert.SerializeObject(order);

                byte[] orderByteArray = Encoding.UTF8.GetBytes(jsonMessage);

                for (int i = 0; i < numberOfMessagesToSend; i++)
                {
                    Message message = new Message
                    {
                        Body = orderByteArray,
                        MessageId = Guid.NewGuid().ToString(),
                        Label = order.OrderId.ToString()
                    };

                    Console.WriteLine($"Sending message: {jsonMessage}");

                    await topicClient.SendAsync(message);

                }
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

            order.Products = new List<Product>
            {
                SetProducts()
            };

            order.ProductionIds = new List<Guid>();

            order.ProductionIds.Add(Guid.NewGuid());
            order.ProductionIds.Add(Guid.NewGuid());

            return order;

        }

        static Product SetProducts()
        {
            var product = new Product();

            product.ProductId = Guid.NewGuid();
            product.Price = 10.2;

            return product;
        }
    }

    public class Order
    {
        public Guid OrderId { get; set; }

        public Guid StoreId { get; set; }

        public string Total { get; set; }

        public List<Product> Products { get; set; }

        public List<Guid> ProductionIds { get; set; }

    }
}
