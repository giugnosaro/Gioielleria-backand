using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Gioielleriabk.Models;

namespace Gioielleriabk.Services
{
    public class UserService
    {
        private readonly string _connectionString;

        public UserService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Metodo per registrare un nuovo utente con nome, email e password
        public async Task<bool> RegisterUserAsync(string nome, string email, string password, Role role)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("INSERT INTO Users (Nome, Email, PasswordHash, Role) VALUES (@Nome, @Email, @PasswordHash, @Role)", connection);
                command.Parameters.AddWithValue("@Nome", nome);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                command.Parameters.AddWithValue("@Role", role.ToString());

                try
                {
                    await command.ExecuteNonQueryAsync();
                    return true;
                }
                catch (SqlException ex) when (ex.Number == 2627) // Violazione del vincolo UNIQUE
                {
                    // Gestisce l'errore quando l'email è già registrata
                    return false;
                }
            }
        }

        // Metodo per autenticare l'utente e restituire i dettagli dell'utente
        public async Task<User> AuthenticateUserAsync(string email, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT Nome, Email, PasswordHash, Role FROM Users WHERE Email = @Email", connection);
                command.Parameters.AddWithValue("@Email", email);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        var storedHash = reader["PasswordHash"].ToString();

                        if (BCrypt.Net.BCrypt.Verify(password, storedHash))
                        {
                            // Se la password corrisponde, restituisci i dettagli dell'utente
                            return new User
                            {
                                Nome = reader["Nome"].ToString(),
                                Email = reader["Email"].ToString(),
                                Role = reader["Role"].ToString()
                            };
                        }
                    }
                }
            }

            return null; // Restituisce null se l'autenticazione fallisce
        }
    }

    // Definizione della classe User
    public class User
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
