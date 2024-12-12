using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MoviesApi.Services;
using MoviesApi.ApiModels; 
using Org.BouncyCastle.Crypto.Generators;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DatabaseService _databaseService;

        public UserController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        // POST: api/User/Login
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest("Email and password are required.");
            }

            using (MySqlConnection connection = _databaseService.GetConnection())
            {
                connection.Open();

                // Query to fetch the user with the given email
                string query = "SELECT * FROM users WHERE email = @Email LIMIT 1";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", model.Email);

                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    // Creating the user object from database response
                    var user = new User()
                    {
                        Id = reader.GetInt32(0),
                        Email = reader.GetString(1),
                        Password = reader.GetString(2), // Hashed password in DB
                        Name = reader.GetString(3),
                        Role = (UserRole)reader.GetInt32(4)
                    };

                    // Compare hashed password
                    if (model.Password== user.Password)
                    {
                        // Return user details excluding the password
                        var userToReturn = new User()
                        {
                            Id = user.Id,
                            Email = user.Email,
                            Name = user.Name,
                            Role = user.Role
                        };
                        return Ok(userToReturn); // Return user data (exclude password)
                    }
                    else
                    {
                        return Unauthorized("Invalid password.");
                    }
                }
            }

            return Unauthorized("Invalid email.");
        }
    }
}
