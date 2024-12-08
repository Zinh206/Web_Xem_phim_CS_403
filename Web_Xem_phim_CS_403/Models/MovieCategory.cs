namespace Web_Xem_phim_CS_403.Models
{
    public class MovieCategory
    {
        public int MovieID { get; set; }
        public Movie Movie { get; set; }

        public int CategoryID { get; set; }
        public Category Category { get; set; }
    }
}
