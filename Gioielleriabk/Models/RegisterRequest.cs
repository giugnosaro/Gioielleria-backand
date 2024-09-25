
using System.ComponentModel.DataAnnotations;

namespace Gioielleriabk.Models
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Il campo Nome è obbligatorio.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Il campo Email è obbligatorio.")]
        [EmailAddress(ErrorMessage = "Inserisci un indirizzo email valido.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Il campo Password è obbligatorio.")]
        [MinLength(6, ErrorMessage = "La password deve essere lunga almeno 6 caratteri.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Il campo Ruolo è obbligatorio.")]
        public string Role { get; set; }
    }
}
