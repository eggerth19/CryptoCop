using System.Threading.Tasks;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/cryptocurrencies")]
    public class CryptoCurrencyController : ControllerBase
    {
        public readonly ICryptoCurrencyService _cryptoCurrencyService;

        public CryptoCurrencyController(ICryptoCurrencyService cryptoCurrencyService)
        {
            _cryptoCurrencyService = cryptoCurrencyService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAvailableCryptocurrencies()
        {   
            return Ok(await _cryptoCurrencyService.GetAvailableCryptocurrencies());
        }
    }
}
