using System;
using System.Linq;
using System.Threading.Tasks;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/cart")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        public readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetCartItems()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "user").ToString();
            return Ok(_shoppingCartService.GetCartItems(email));
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddCartItem([FromBody] ShoppingCartItemInputModel item)
        {
            if (item.Quantity <= 0) { return StatusCode(403, "Quantity cannot be 0 or lower"); }
            var email = User.Claims.FirstOrDefault(c => c.Type == "user").ToString();
            await _shoppingCartService.AddCartItem(email, item);
            return StatusCode(201, "Item added to your shoppingcart");
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult RemoveCartItem([FromRoute] int id)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "user").ToString();
            _shoppingCartService.RemoveCartItem(email, id);
            return NoContent();
        }

        [HttpPatch]
        [Route("{id:int}")]
        public IActionResult UpdateCartItemQuantity([FromRoute] int id, [FromBody] ShoppingCartItemInputModel item)
        {
            
            var email = User.Claims.FirstOrDefault(c => c.Type == "user").ToString();
            float quantity = item.Quantity;
            _shoppingCartService.UpdateCartItemQuantity(email, id, quantity);
            return NoContent();
        }

        [HttpDelete]
        [Route("")]
        public IActionResult ClearCart()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "user").ToString();
            _shoppingCartService.ClearCart(email);
            return NoContent();
        }
    }
}