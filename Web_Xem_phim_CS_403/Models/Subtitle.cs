using System.ComponentModel.DataAnnotations;

namespace Web_Xem_phim_CS_403.Models
{
    public class Subtitle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MovieID { get; set; }

        public Movie Movie { get; set; }

        [Required]
        public string Language { get; set; }

        public string SubtitleURL { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
