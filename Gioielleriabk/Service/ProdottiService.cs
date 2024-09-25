using Gioielleriabk.Models;
using System.Data.SqlClient;

namespace Gioielleriabk.Service
{
    public class ProdottiService
    {
        private readonly string _connectionString;

        public ProdottiService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Categoria>> GetCategorie()
        {
            var categorie = new List<Categoria>();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var command = new SqlCommand("SELECT id, nome FROM Categorie", connection);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var categoria = new Categoria
                            {
                                Id = (int)reader["id"],
                                Nome = reader["nome"].ToString()
                            };
                            categorie.Add(categoria);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Logga l'errore
                Console.WriteLine($"Errore durante il recupero delle categorie: {ex.Message}");
                throw;
            }
            return categorie;
        }





        public async Task<List<Prodotto>> GetProdotti()
        {
            var prodotti = new List<Prodotto>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT id, nome, descrizione, prezzo, Immagine, idCategoria FROM Prodotti", connection);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var prodotto = new Prodotto
                        {
                            Id = (int)reader["id"],
                            Nome = reader["nome"].ToString(),
                            Descrizione = reader["descrizione"].ToString(),
                            Prezzo = (decimal)reader["prezzo"],
                            Immagine = reader["Immagine"] != DBNull.Value ? Convert.ToBase64String((byte[])reader["Immagine"]) : null, // Solo base64, senza prefisso
                            idCategoria = (int)reader["idCategoria"]
                        };
                        prodotti.Add(prodotto);
                    }
                }
            }
            return prodotti;
        }


        public async Task<Prodotto> GetProdottoById(int id)
        {
            Prodotto prodotto = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT id, nome, descrizione, prezzo, Immagine FROM Prodotti WHERE id = @id", connection);
                command.Parameters.AddWithValue("@id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        prodotto = new Prodotto
                        {
                            Id = (int)reader["id"],
                            Nome = reader["nome"].ToString(),
                            Descrizione = reader["descrizione"].ToString(),
                            Prezzo = (decimal)reader["prezzo"],
                            Immagine = reader["Immagine"] != DBNull.Value ? "data:image/jpeg;base64," + Convert.ToBase64String((byte[])reader["Immagine"]) : null
                        };
                    }
                }
            }
            return prodotto;
        }


        public async Task<Prodotto> AddProdotto(Prodotto prodotto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("INSERT INTO Prodotti (Nome, Descrizione, Prezzo, Immagine, idCategoria) VALUES (@Nome, @Descrizione, @Prezzo, @Immagine, @idCategoria); SELECT SCOPE_IDENTITY()", connection);

                command.Parameters.AddWithValue("@Nome", prodotto.Nome);
                command.Parameters.AddWithValue("@Descrizione", prodotto.Descrizione);
                command.Parameters.AddWithValue("@Prezzo", prodotto.Prezzo);

                // Converti la stringa base64 in un array di byte per salvarla nel database
                if (!string.IsNullOrEmpty(prodotto.Immagine))
                {
                    byte[] imageBytes = Convert.FromBase64String(prodotto.Immagine); // Converte la stringa base64 in byte[]
                    command.Parameters.AddWithValue("@Immagine", imageBytes);
                }
                else
                {
                    command.Parameters.AddWithValue("@Immagine", DBNull.Value); // Inserisci un valore null se l'immagine non è presente
                }

                command.Parameters.AddWithValue("@idCategoria", prodotto.idCategoria);

                prodotto.Id = Convert.ToInt32(await command.ExecuteScalarAsync());
            }
            return prodotto;
        }


         public async Task<List<Prodotto>> GetProdottiByCategoria(int idCategoria)
        {
            var prodotti = new List<Prodotto>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT id, nome, descrizione, prezzo, Immagine, idCategoria FROM Prodotti WHERE idCategoria = @idCategoria", connection);
                command.Parameters.AddWithValue("@idCategoria", idCategoria);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        var prodotto = new Prodotto
                        {
                            Id = (int)reader["id"],
                            Nome = reader["nome"].ToString(),
                            Descrizione = reader["descrizione"].ToString(),
                            Prezzo = (decimal)reader["prezzo"],
                            Immagine = reader["Immagine"] != DBNull.Value ? Convert.ToBase64String((byte[])reader["Immagine"]) : null,
                            idCategoria = (int)reader["idCategoria"]
                        };
                        prodotti.Add(prodotto);
                    }
                }
            }
            return prodotti;
        }

    }
}