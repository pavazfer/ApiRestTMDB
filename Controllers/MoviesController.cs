using Microsoft.AspNetCore.Mvc;
using RestSharp;
using Microsoft.Extensions.Configuration;
using PruebaTecnicaApiRest.Models;

[Route("api/movies")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public MoviesController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet("search")]
    public IActionResult SearchMoviesByTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return BadRequest("El título de la película no puede estar vacío.");
        }

        var apiKey = _configuration["TMDApiKey"];
        var client = new RestClient("https://api.themoviedb.org/3");
        var request = new RestRequest($"/search/movie?api_key={apiKey}&query={title}");

        var response = client.Get<SearchMovieResponse>(request);

        if (response != null && response.results.Count > 0)
        {
            var movie = response.results.FirstOrDefault();

            if (movie != null)
            {
                var similarMovies = GetSimilarMovies(movie.id, apiKey);

                var result = new
                {
                    Title = movie.title,
                    OriginalTitle = movie.original_title,
                    AverageRating = movie.vote_average,
                    ReleaseDate = movie.release_date.ToString("yyyy-MM-dd"),
                    Overview = movie.overview,
                    SimilarMovies = similarMovies
                };

                return Ok(result);
            }
        }

        return NotFound("No se encontró la película.");
    }
    private string GetSimilarMovies(int? movieId, string? apiKey)
    {
        var client = new RestClient("https://api.themoviedb.org/3");
        var request = new RestRequest($"/movie/{movieId}/similar?api_key={apiKey}");
        var response = client.Get<SimilarMoviesResponse>(request);

        if (response != null)
        {
            var similarMovies = response.results
                .Take(5)
                .Select(m => $"{m.title} ({m.release_date.ToString("yyyy")})");

            return string.Join(", ", similarMovies);
        }

        return string.Empty;
    }

}
