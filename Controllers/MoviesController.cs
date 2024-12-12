

using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MoviesApi.Services;
using System.Collections.Generic;
using MoviesApi.ApiModels;
namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly DatabaseService _databaseService;

        public MoviesController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        // GET: api/Movies
        [HttpGet]
        public IActionResult GetMovies()
        {
            var movies = new List<MovieApi>();

            using (MySqlConnection connection = _databaseService.GetConnection())
            {
                connection.Open();
                string query = "SELECT MovieId, Title, Description, ReleaseDate, PosterURL, Director FROM movies";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var movie = new MovieApi
                    {
                        MovieId = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Description = reader.GetString(2),
                        ReleaseDate = reader.GetDateTime(3),
                        PosterURL = reader.GetString(4),
                        Director = reader.GetString(5)
                    };
                    movies.Add(movie);
                }

                reader.Close();
            }

            return Ok(movies);
        }
        [HttpGet("{movieId}/actors")]
        public IActionResult GetActorsByMovie(int movieId)
        {
            var actors = new List<Actor>();

            using (MySqlConnection connection = _databaseService.GetConnection())
            {
                connection.Open();
                string query = @"
            SELECT a.ActorId, a.ActorName, a.PictureURL
            FROM actors a
            INNER JOIN actorsmovies am ON a.ActorId = am.ActorId
            WHERE am.MovieId = @MovieId";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@MovieId", movieId);

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var actor = new Actor
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        PictureURL = reader.GetString(2)
                    };
                    actors.Add(actor);
                }

                reader.Close();
            }

            return Ok(actors);
        }

        // Get movies by director
        [HttpGet("director/{directorName}/movies")]
        public IActionResult GetMoviesByDirector(string directorName)
        {
            var movies = new List<MovieApi>();

            using (MySqlConnection connection = _databaseService.GetConnection())
            {
                connection.Open();
                string query = @"
                SELECT MovieId, Title, Description, ReleaseDate, PosterURL, Director
                FROM movies
                WHERE Director = @DirectorName";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@DirectorName", directorName);

                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var movie = new MovieApi
                    {
                        MovieId = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Description = reader.GetString(2),
                        ReleaseDate = reader.GetDateTime(3),
                        PosterURL = reader.GetString(4),
                        Director = reader.GetString(5)
                    };
                    movies.Add(movie);
                }

                reader.Close();
            }

            if (!movies.Any())
            {
                return NotFound(new { message = "No movies found for this director." });
            }

            return Ok(movies);
        }
    }
}


   
