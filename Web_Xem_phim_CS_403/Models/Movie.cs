using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Xem_phim_CS_403.Models
{
    public class Movie
    {
        [Key]
        public int MovieID { get; set; }

        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }

        public int? Duration { get; set; } // in minutes

        public string? PosterURL { get; set; }

        public string? TrailerURL { get; set; }

        [Range(0, 10)]
        [Column(TypeName = "decimal(5, 2)")]
        public decimal? Rating { get; set; }

        public int Views { get; set; } = 0;

        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<MovieCategory> MovieCategories { get; set; } = new List<MovieCategory>();

       
        // Phân biệt phim lẻ và phim bộ
        public bool IsSeries { get; set; } = false;

        [Range(1, int.MaxValue, ErrorMessage = "Total episodes must be greater than 0.")]
        public int? TotalEpisodes { get; set; }

        // Nếu là phim bộ, sẽ có danh sách các tập
        public ICollection<Episode>? Episodes { get; set; }
    }
}
