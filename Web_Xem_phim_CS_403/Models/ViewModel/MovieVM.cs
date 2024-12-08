using System.ComponentModel.DataAnnotations;

namespace Web_Xem_phim_CS_403.Models.ViewModel
{
    public class MovieVM
    {
        public int MovieID { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public int? Duration { get; set; }

        public string? PosterURL { get; set; }

        public string? TrailerURL { get; set; }

        public decimal? Rating { get; set; }

        public bool IsSeries { get; set; }
        public int? TotalEpisodes { get; set; }
        public List<int>? SelectedCategories { get; set; } // For multiple categories

        public List<Category> Categories { get; set; } = new();

        public List<EpisodeVM>? Episodes { get; set; }
    }
}
