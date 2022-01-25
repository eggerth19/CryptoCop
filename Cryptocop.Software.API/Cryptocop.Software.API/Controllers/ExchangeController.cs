using System.Threading.Tasks;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/exchanges")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        public readonly IExchangeService _exchangeservice;

        public ExchangeController(IExchangeService exchangeservice)
        {
            _exchangeservice = exchangeservice;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetExchanges([FromQuery] int pageNumber = 1)
        {   
            return Ok(await _exchangeservice.GetExchanges(pageNumber));
        }
    }
}