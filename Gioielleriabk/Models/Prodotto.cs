using Microsoft.AspNetCore.Http; // Aggiungi questa direttiva

namespace Gioielleriabk.Models
{
    public class Prodotto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descrizione { get; set; }
        public decimal Prezzo { get; set; }
        public string Immagine { get; set; } // Utilizza string per contenere la rappresentazione base64 dell'immagine
        public int idCategoria { get; set; }
    }

}
