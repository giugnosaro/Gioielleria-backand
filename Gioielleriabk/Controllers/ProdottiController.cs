using Microsoft.AspNetCore.Mvc;
using Gioielleriabk.Models;
using Gioielleriabk.Service;
using System.Threading.Tasks;

namespace Gioielleriabk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdottiController : ControllerBase
    {
        private readonly ProdottiService _prodottoService;

        // Inietta il servizio tramite il costruttore
        public ProdottiController(ProdottiService prodottiService)
        {
            _prodottoService = prodottiService;
        }

        [HttpGet("categorie")]
        public async Task<IActionResult> GetCategorie()
        {
            var categorie = await _prodottoService.GetCategorie(); // Usa il servizio per ottenere le categorie
            return Ok(categorie);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProdottoById(int id)
        {
            var prodotto = await _prodottoService.GetProdottoById(id);

            if (prodotto == null)
            {
                return NotFound(); // Restituisci un 404 se il prodotto non esiste
            }

            return Ok(prodotto);  // Restituisci il prodotto trovato
        }


        [HttpPost]
        public async Task<IActionResult> AddProdotto([FromForm] Prodotto prodotto)
        {
            if (string.IsNullOrEmpty(prodotto.Immagine))
            {
                return BadRequest(new { Message = "L'immagine è richiesta." });
            }

            try
            {
                var nuovoProdotto = await _prodottoService.AddProdotto(prodotto);

                if (nuovoProdotto == null)
                {
                    return BadRequest("Errore durante l'inserimento del prodotto.");
                }

                return Ok(nuovoProdotto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Errore del server: {ex.Message}");
            }
        }

        [HttpGet("byCategoria")]
        public async Task<IActionResult> GetProdottiByCategoria([FromQuery] int idCategoria)
        {
            try
            {
                var prodotti = await _prodottoService.GetProdottiByCategoria(idCategoria);
                return Ok(prodotti);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Errore del server: {ex.Message}");
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetProdotti()
        {
            try
            {
                var prodotti = await _prodottoService.GetProdotti();
                return Ok(prodotti);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Errore del server: {ex.Message}");
            }
        }
    }
}
