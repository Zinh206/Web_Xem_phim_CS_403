using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Xem_phim_CS_403.Data;
using Web_Xem_phim_CS_403.Models;
using Web_Xem_phim_CS_403.Models.ViewModel;

namespace Web_Xem_phim_CS_403.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EpisodeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EpisodeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var seriesMovies = _context.Movies
                .Where(m => m.IsSeries)
                .Include(m => m.Episodes)
                .ToList();

            return View(seriesMovies);
        }

        [HttpGet]
        public IActionResult Manage(int movieId)
        {
            var movie = _context.Movies
                .Include(m => m.Episodes)
                .FirstOrDefault(m => m.MovieID == movieId);

            if (movie == null)
            {
                return NotFound();
            }

            var episodeListVM = new EpisodeListVM
            {
                MovieID = movie.MovieID,
                MovieTitle = movie.Title,
                TotalEpisodesAllowed = movie.TotalEpisodes ?? 0,
                Episodes = movie.Episodes?.Select(e => new EpisodeVM
                {
                    EpisodeID = e.EpisodeID, // Thêm ID
                    Title = e.Title,
                    VideoURL = e.VideoURL,
                    Duration = e.Duration,
                    EpisodeNumber = e.EpisodeNumber,
                    MovieID = e.MovieID
                }).ToList() ?? new List<EpisodeVM>()
            };

            return View(episodeListVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEditEpisode(EpisodeVM episodeVM)
        {
            // Loại bỏ MovieTitle khỏi kiểm tra ModelState
            ModelState.Remove(nameof(EpisodeVM.MovieTitle));

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid data.";
                return RedirectToAction("Manage", new { movieId = episodeVM.MovieID });
            }

            // Lấy thông tin phim để kiểm tra
            var movie = _context.Movies.Include(m => m.Episodes)
                                       .FirstOrDefault(m => m.MovieID == episodeVM.MovieID);

            if (movie == null)
            {
                TempData["Error"] = "Movie not found.";
                return RedirectToAction("Manage", new { movieId = episodeVM.MovieID });
            }

            // Kiểm tra số tập trùng
            var duplicateEpisode = movie.Episodes.FirstOrDefault(e =>
                e.EpisodeNumber == episodeVM.EpisodeNumber &&
                e.EpisodeID != episodeVM.EpisodeID);

            if (duplicateEpisode != null)
            {
                TempData["Error"] = $"Episode number {episodeVM.EpisodeNumber} already exists.";
                return RedirectToAction("Manage", new { movieId = episodeVM.MovieID });
            }

            // Thêm mới hoặc chỉnh sửa
            if (episodeVM.EpisodeID == 0)
            {
                // Thêm mới
                var newEpisode = new Episode
                {
                    Title = episodeVM.Title,
                    VideoURL = episodeVM.VideoURL,
                    Duration = episodeVM.Duration,
                    EpisodeNumber = episodeVM.EpisodeNumber,
                    MovieID = episodeVM.MovieID
                };
                _context.Episodes.Add(newEpisode);
                TempData["Success"] = "Episode added successfully.";
            }
            else
            {
                // Sửa tập hiện có
                var existingEpisode = movie.Episodes.FirstOrDefault(e => e.EpisodeID == episodeVM.EpisodeID);
                if (existingEpisode != null)
                {
                    existingEpisode.Title = episodeVM.Title;
                    existingEpisode.VideoURL = episodeVM.VideoURL;
                    existingEpisode.Duration = episodeVM.Duration;
                    existingEpisode.EpisodeNumber = episodeVM.EpisodeNumber;
                    TempData["Success"] = "Episode updated successfully.";
                }
            }

            _context.SaveChanges();
            return RedirectToAction("Manage", new { movieId = episodeVM.MovieID });
        }



        [HttpPost]
        public IActionResult DeleteEpisode(int episodeId, int movieId)
        {
            var episode = _context.Episodes.FirstOrDefault(e => e.EpisodeID == episodeId);
            if (episode != null)
            {
                _context.Episodes.Remove(episode);
                _context.SaveChanges();
            }

            return RedirectToAction("Manage", new { movieId });
        }
    }
}
