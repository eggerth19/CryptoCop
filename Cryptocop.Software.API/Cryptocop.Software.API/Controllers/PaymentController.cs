using System.Linq;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cryptocop.Software.API.Controllers
{
    [Authorize]
    [Route("api/payments")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        public readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetStoredPaymentCards()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == "user").ToString();
            return Ok(_paymentService.GetStoredPaymentCards(email));
        }

        [HttpPost]
        [Route("")]
        public IActionResult AddPaymentCard([FromBody] PaymentCardInputModel paymentCard)
        {
            if (!ModelState.IsValid) { return StatusCode(412, "Invalid paymentcard information"); }
            var email = User.Claims.FirstOrDefault(c => c.Type == "user").ToString();
            _paymentService.AddPaymentCard(email, paymentCard);
            return StatusCode(201, "Paymentcard added");
        }
    }
}