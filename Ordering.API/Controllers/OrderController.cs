using GeekBurger.Ordering.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ordering.API.Mongo.Repositories;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private Task _task;
        private readonly OrderMongoRepository _orderMongoRepository;

        public OrderController(IConfiguration configuration, Task task, Mongo.Repositories.OrderMongoRepository orderMongoRepository)
        {
            _configuration = configuration;
            _task = task;
            _orderMongoRepository = orderMongoRepository;
        }

        // GET api/order
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "teste1", "teste2" };
        }

        /// <summary>
        /// Método responsável por enviar a solicitação de 
        /// processamento de pagamento ao microserviço de pagamentos
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> ReceiveOrderAsync([FromQuery] Order order)
        {
            // Envia a solicitação para a fila
            SendMessagesAsync(order);

            //Grava no mongoDB
            _orderMongoRepository.Add(order);

            //Retorna mensagem de sucesso.
            return Ok(order);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        /// <summary>
        /// Método responsável por enviar mensagem de pagamento negado a API de produção
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> CancelProductionAsync()
        {
            return Ok();
        }

        public async void SendMessagesAsync(Order order)
        {
            if (_task != null && !_task.IsCompleted)
                return;

            string connectionString = _configuration["serviceBus:connectionString"];
            TopicClient topicClient = new TopicClient(connectionString, "orders");
            byte[] orderByteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(order));

            Message message = new Message
            {
                Body = orderByteArray,
                MessageId = Guid.NewGuid().ToString(),
                Label = order.OrderId.ToString()
            };

            _task = topicClient.SendAsync(message);
            await _task;

            var closeTask = topicClient.CloseAsync();
            await closeTask;
        }
    }
}