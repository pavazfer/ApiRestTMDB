namespace PruebaTecnicaApiRest.Models
{
    public class TrendingMoviesResponse
    {
        public int page { get; set; }
        public List<MovieResult> results { get; set; }
    }
}
