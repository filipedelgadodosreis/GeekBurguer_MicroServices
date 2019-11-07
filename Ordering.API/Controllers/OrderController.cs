using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return Ok("Processando pedidos em background");
        }
    }
}