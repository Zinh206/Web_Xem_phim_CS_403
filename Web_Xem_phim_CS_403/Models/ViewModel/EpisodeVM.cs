using System.ComponentModel.DataAnnotations;

namespace Web_Xem_phim_CS_403.Models.ViewModel
{
    public class EpisodeVM
    {
        [Key]
        public int EpisodeID { get; set; }

        
        public string Title { get; set; }

      
        public int EpisodeNumber { get; set; }

       
        public string VideoURL { get; set; }

        
        public int? Duration { get; set; } // Thời lượng tập phim (phút)

        [Required]
        public int MovieID { get; set; } // Liên kết tới Movie

        public string MovieTitle { get; set; } // Tên phim liên kết
    }
}
