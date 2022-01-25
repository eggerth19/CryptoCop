using System.Linq;
using System.Threading.Tasks;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        public readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetOrders()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "user").ToString();
            return Ok(await _orderService.GetOrders(email));
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateNewOrder([FromBody] OrderInputModel order)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "user").ToString();
            await _orderService.CreateNewOrder(email, order);
            return StatusCode(201, "Order created");
        }
    }
}