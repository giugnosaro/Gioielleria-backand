using System.Text.Json.Serialization;

namespace Gioielleriabk.Models
{
    public class OrderItem
    {

        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        // Rendi questo campo opzionale
        public Order? Order { get; set; }
    }
}
