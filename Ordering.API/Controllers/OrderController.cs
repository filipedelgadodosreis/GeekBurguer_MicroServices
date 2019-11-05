using GeekBurger.UI.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Ordering.API.Sql.Repositories;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

//USE [FIAP15]
//GO

///****** Object: Table [dbo].[Orders] Script Date: 04/11/2019 23:11:30 ******/
//SET ANSI_NULLS ON
//GO

//SET QUOTED_IDENTIFIER ON
//GO

//CREATE TABLE [dbo].[Orders] (
//    [Id]      INT           IDENTITY (1, 1) NOT NULL,
//    [OrderId] VARCHAR (200) NULL,
//    [StoreId] VARCHAR (200) NULL


//USE [FIAP15]
//GO

///****** Object: Table [dbo].[Product] Script Date: 04/11/2019 23:11:01 ******/
//SET ANSI_NULLS ON
//GO

//SET QUOTED_IDENTIFIER ON
//GO

//CREATE TABLE [dbo].[Product] (
//    [Id]        INT           IDENTITY (1, 1) NOT NULL,
//    [OrderId]   INT           NULL,
//    [ProductId] VARCHAR (200) NULL
//);

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private Task _task;
        private readonly OrderSqlRepository _orderSqlRepository;

        public OrderController(IConfiguration configuration, OrderSqlRepository orderSqlRepository)
        {
            _configuration = configuration;
            _orderSqlRepository = orderSqlRepository;
            _task = null;
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
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> ReceiveOrderAsync([FromQuery] NewOrderMessage request)
        {
            // Buscar na fila 
            SendMessagesAsync(request);

            // Grava no mongoDB
            //_orderMongoRepository.Add(order);

            // Grava no Sql
            _orderSqlRepository.Add(request);

            // Retorna mensagem de sucesso.
            return Ok("Pedido cadastrado com sucesso");
        }

        public async void SendMessagesAsync(NewOrderMessage order)
        {
            try
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
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}