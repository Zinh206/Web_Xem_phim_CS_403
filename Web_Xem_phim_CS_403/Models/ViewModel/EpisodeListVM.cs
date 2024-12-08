namespace Web_Xem_phim_CS_403.Models.ViewModel
{
    public class EpisodeListVM
    {
        public int MovieID { get; set; }
        public string MovieTitle { get; set; }
        public int TotalEpisodesAllowed { get; set; }
        public List<EpisodeVM> Episodes { get; set; } = new();
    }
}
