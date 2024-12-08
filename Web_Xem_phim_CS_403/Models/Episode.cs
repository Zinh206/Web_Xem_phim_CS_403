using System.ComponentModel.DataAnnotations;

namespace Web_Xem_phim_CS_403.Models
{
    public class Episode
    {
        [Key]
        public int EpisodeID { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters.")]
        public string Title { get; set; } // Tên tập phim

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Episode number must be greater than 0.")]
        public int EpisodeNumber { get; set; } // Episode number for a series

        [Range(1, int.MaxValue, ErrorMessage = "Duration must be greater than 0.")]
        public int? Duration { get; set; } // Thời lượng tập phim (phút)

        [Required]
        [Url(ErrorMessage = "Please provide a valid URL.")]
        public string VideoURL { get; set; } // URL tập phim

        // Liên kết với Movie
        [Required]
        public int MovieID { get; set; }
        public Movie Movie { get; set; }
    }
}
