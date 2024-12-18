using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Xem_phim_CS_403.Data;

namespace Web_Xem_phim_CS_403.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? categoryId, int? year, int? duration)
        {
            // Dropdown dữ liệu
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Years = _context.Movies
                .Where(m => m.ReleaseDate.HasValue)
                .Select(m => m.ReleaseDate.Value.Year)
                .Distinct()
                .OrderByDescending(y => y)
                .ToList();
            ViewBag.Durations = new List<string> { "Dưới 60 phút", "60-120 phút", "Trên 120 phút" };

            // Phim đề cử
            ViewBag.TopRatedMovies = _context.Movies
                .OrderByDescending(m => m.Rating)
                .Take(4)
                .ToList();

            // Lọc phim
            var movies = _context.Movies.AsQueryable();

            if (categoryId.HasValue)
                movies = movies.Where(m => m.MovieCategories.Any(mc => mc.CategoryID == categoryId.Value));

            if (year.HasValue)
                movies = movies.Where(m => m.ReleaseDate.HasValue && m.ReleaseDate.Value.Year == year.Value);

            if (duration.HasValue)
            {
                switch (duration.Value)
                {
                    case 1: // Dưới 60 phút
                        movies = movies.Where(m => m.Duration.HasValue && m.Duration.Value < 60);
                        break;
                    case 2: // 60-120 phút
                        movies = movies.Where(m => m.Duration.HasValue && m.Duration.Value >= 60 && m.Duration.Value <= 120);
                        break;
                    case 3: // Trên 120 phút
                        movies = movies.Where(m => m.Duration.HasValue && m.Duration.Value > 120);
                        break;
                }
            }

            ViewBag.Movies = movies.ToList();
            return View();

        }
    }
}
