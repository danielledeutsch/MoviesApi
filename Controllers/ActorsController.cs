using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using MoviesApi.Services;
using MoviesApi.ApiModels;
using System.Collections.Generic;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly DatabaseService _databaseService;

        public ActorsController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        // GET: api/Actors/{actorId}/movies
        [HttpGet("{actorId}/movies")]
        public IActionResult GetMoviesByActor(int actorId)
        {
            var movies = new List<MovieApi>();

            using (MySqlConnection connection = _databaseService.GetConnection())
            {
                connection.Open();
                string query = @"
                    SELECT m.MovieId, m.Title, m.Description, m.ReleaseDate, m.PosterURL, m.Director
                    FROM movies m
                    INNER JOIN actorsmovies am ON m.MovieId = am.MovieId
                    WHERE am.ActorId = @ActorId";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@ActorId", actorId);

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

        // GET: api/Actors
        [HttpGet]
        public IActionResult GetActors()
        {
            var actors = new List<Actor>();

            using (MySqlConnection connection = _databaseService.GetConnection())
            {
                connection.Open();
                string query = "SELECT ActorId, ActorName, PictureURL FROM actors";
                MySqlCommand command = new MySqlCommand(query, connection);
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
        [HttpGet("movie/{movieId}/actors")]
        public IActionResult GetActorsByMovie(int movieId)
        {
            var actors = new List<Actor>();

            using (var connection = _databaseService.GetConnection())
            {
                connection.Open();
                string query = @"
            SELECT a.ActorId, a.ActorName, a.PictureURL 
            FROM actors a
            JOIN actorsmovies am ON a.ActorId = am.ActorId
            WHERE am.MovieId = @movieId";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@movieId", movieId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            actors.Add(new Actor
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                PictureURL = reader.GetString(2)
                            });
                        }
                    }
                }
            }

            return Ok(actors);
        }

    }
}
