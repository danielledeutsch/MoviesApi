namespace MoviesApi.ApiModels
{
    public class MovieApi
    {
        public int MovieId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string? PosterURL { get; set; }
        public string? Director { get; set; }
    }
}

