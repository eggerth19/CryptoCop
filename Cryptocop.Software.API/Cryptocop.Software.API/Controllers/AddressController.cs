using System.Linq;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/addresses")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        public readonly IAddressService _addressservice;
        
        public AddressController(IAddressService addressservice)
        {
            _addressservice = addressservice;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetCartItems()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "user").ToString();
            return Ok(_addressservice.GetAllAddresses(email));
        }

        [HttpPost]
        [Route("")]
        public IActionResult AddAddress([FromBody] AddressInputModel address)
        {   if (!ModelState.IsValid) { return StatusCode(412, address); }
            var email = User.Claims.FirstOrDefault(c => c.Type == "user").ToString();
            _addressservice.AddAddress(email, address);
            return StatusCode(201, "Address created");
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteAddress([FromRoute] int id)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "user").ToString();
            _addressservice.DeleteAddress(email, id);
            return NoContent();
        }
    }
}