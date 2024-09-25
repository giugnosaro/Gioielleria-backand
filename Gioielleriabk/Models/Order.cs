namespace Gioielleriabk.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerEmail { get; set; }
        public string ShippingAddress { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }

        // Relazione con gli articoli dell'ordine
        public List<OrderItem> OrderItems { get; set; }


    }
}
