using GeekBurger.Ordering.Contract;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
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
        public async Task<ActionResult> ReceiveOrderAsync([FromQuery] NewOrder order)
        {
            // Envia a solicitação para o microserviço de pagamento para processar o mesmo.
            // Requisição Service Bus
            
            //Grava na base de dados

            //Retorna mensagem de sucesso.

            return Ok();
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
       

    }
}