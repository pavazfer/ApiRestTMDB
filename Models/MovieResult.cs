namespace PruebaTecnicaApiRest.Models
{
    public class MovieResult
    {
        public bool adult { get; set; }
        public string backdrop_path { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public string OriginalLanguage { get; set; }
        public string original_title { get; set; }
        public string overview { get; set; }
        public string posterPath { get; set; }
        public string mediaType { get; set; }
        public List<int> genreIds { get; set; }
        public double popularity { get; set; }
        public string release_date { get; set; }
        public bool video { get; set; }
        public double vote_average { get; set; }
        public int vote_count { get; set; }
    }
}
