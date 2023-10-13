using Microsoft.AspNetCore.Mvc;
using RestSharp;
using Microsoft.Extensions.Configuration;
using PruebaTecnicaApiRest.Models;
using System.Linq;

[Route("api/movies")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly RestClient _client;

    public MoviesController(IConfiguration configuration)
    {
        _configuration = configuration;
        _client = new RestClient("https://api.themoviedb.org/3");
    }

    [HttpGet("search")]
    public IActionResult SearchMoviesByTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return BadRequest("El título de la película no puede estar vacío.");
        }

        var apiKey = _configuration["TMDApiKey"];
        var request = new RestRequest($"/search/movie");
        request.AddParameter("api_key", apiKey);
        request.AddParameter("query", title);

        var response = _client.Get<MovieResponse>(request);

        if (response != null && response.results.Any())
        {
            var movie = response.results.FirstOrDefault();

            if (movie != null)
            {
                var similarMovies = GetSimilarMovies(movie.id, apiKey);

                var result = new
                {
                    movie.title,
                    movie.original_title,
                    movie.vote_average,
                    movie.release_date,
                    movie.overview,
                    SimilarMovies = similarMovies
                };

                return Ok(result);
            }
        }

        return NotFound("No se encontró la película.");
    }

    private string GetSimilarMovies(int? movieId, string? apiKey)
    {
        var request = new RestRequest($"/movie/{movieId}/similar");
        request.AddParameter("api_key", apiKey);

        var response = _client.Get<MovieResponse>(request);

        if (response != null)
        {
            var similarMovies = response.results
                .Take(5)
                .Select(m => $"{m.title} ({m.release_date?.Substring(0, 4)})");

            return string.Join(", ", similarMovies);
        }

        return string.Empty;
    }
}
