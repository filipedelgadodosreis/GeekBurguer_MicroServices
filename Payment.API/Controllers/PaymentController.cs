using GeekBurger.Payment.Contract;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Payment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "teste1", "teste2" };
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> ReceivePaymentAsync([FromQuery] NewPayment payment)
        {
            //Tratar HTTP Status
            
            //Envia para fila de processamento

            //salva na base de dados


            return Ok();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> SetPaidOrderStatusAsync([FromQuery] NewPayment payment)
        {
            //Tratar HTTP Status


            //Processa o pagamento


            //salva na base de dados

            return Ok();
        }
    }
}