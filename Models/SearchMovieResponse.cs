﻿namespace PruebaTecnicaApiRest.Models
{
    public class SearchMovieResponse
    {
        public int page { get; set; }
        public List<MovieResult> results { get; set; }
        public int total_pages { get; set; }
        public int total_results { get; set; }
    }
}
