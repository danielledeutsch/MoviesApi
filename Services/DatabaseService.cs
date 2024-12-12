using MySql.Data.MySqlClient;
using MoviesApi.ApiModels;
using MoviesApi.Controllers;

namespace MoviesApi.Services
{
    public class DatabaseService
    {
        private readonly string? _connectionString;

        public DatabaseService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}

