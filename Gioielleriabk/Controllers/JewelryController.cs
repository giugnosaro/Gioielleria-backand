using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gioielleriabk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JewelryController : ControllerBase
    {
        [HttpGet("buy")]
        public IActionResult BuyJewelry()
        {
            // Logica per permettere all'utente di comprare gioielli
            return Ok("Jewelry purchase is successful!");
        }

        [HttpPost("manage")]
        [Authorize(Roles = "Admin")] // Solo gli admin possono gestire i gioielli
        public IActionResult ManageJewelry()
        {
            // Logica per gestire i gioielli (aggiungi, modifica, rimuovi)
            return Ok("Jewelry management successful!");
        }
    }
}
