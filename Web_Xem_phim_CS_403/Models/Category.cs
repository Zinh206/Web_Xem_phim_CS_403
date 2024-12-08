using System.ComponentModel.DataAnnotations;

namespace Web_Xem_phim_CS_403.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public ICollection<MovieCategory> MovieCategories { get; set; } = new List<MovieCategory>();
    }
}
