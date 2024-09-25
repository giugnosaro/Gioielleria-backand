using Gioielleriabk.Models;
using System.Data.SqlClient;

namespace Gioielleriabk.Service
{
    public class OrderService
    {
        private readonly string _connectionString;

        public OrderService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> SaveOrder(Order order)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Usa una transazione per assicurarti che l'ordine e gli articoli vengano salvati in modo atomico
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Inserisci l'ordine
                        var orderQuery = @"INSERT INTO Orders (CustomerEmail, ShippingAddress, TotalAmount, OrderDate, PaymentMethod)
                                           OUTPUT INSERTED.OrderId
                                           VALUES (@CustomerEmail, @ShippingAddress, @TotalAmount, GETDATE(), @PaymentMethod)";

                        var orderCommand = new SqlCommand(orderQuery, connection, transaction);
                        orderCommand.Parameters.AddWithValue("@CustomerEmail", order.CustomerEmail);
                        orderCommand.Parameters.AddWithValue("@ShippingAddress", order.ShippingAddress);
                        orderCommand.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                        orderCommand.Parameters.AddWithValue("@PaymentMethod", order.PaymentMethod);

                        // Recupera l'OrderId dell'ordine appena inserito
                        int orderId = (int)await orderCommand.ExecuteScalarAsync();

                        // Inserisci gli articoli dell'ordine
                        foreach (var item in order.OrderItems)
                        {
                            var itemQuery = @"INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
                                              VALUES (@OrderId, @ProductId, @Quantity, @Price)";
                            var itemCommand = new SqlCommand(itemQuery, connection, transaction);
                            itemCommand.Parameters.AddWithValue("@OrderId", orderId);
                            itemCommand.Parameters.AddWithValue("@ProductId", item.ProductId);
                            itemCommand.Parameters.AddWithValue("@Quantity", item.Quantity);
                            itemCommand.Parameters.AddWithValue("@Price", item.Price);

                            await itemCommand.ExecuteNonQueryAsync();
                        }

                        // Se tutto va bene, conferma la transazione
                        transaction.Commit();

                        return orderId;
                    }
                    catch (Exception)
                    {
                        // In caso di errore, effettua il rollback della transazione
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
