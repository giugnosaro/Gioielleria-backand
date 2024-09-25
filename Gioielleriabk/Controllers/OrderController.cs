using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gioielleriabk.Service;
using Gioielleriabk.Models;

namespace Gioielleriabk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromForm] Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Questo ti darà maggiori informazioni sui campi che non superano la validazione
            }

            try
            {
                // Valida l'ordine
                if (order == null || !order.OrderItems.Any())
                {
                    return BadRequest(new { message = "Order or OrderItems cannot be null" });
                }

                var orderId = await _orderService.SaveOrder(order);
                return Ok(new { OrderId = orderId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }


    }
}
