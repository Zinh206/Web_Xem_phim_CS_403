using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Xem_phim_CS_403.Data;
using Web_Xem_phim_CS_403.Models;
using Web_Xem_phim_CS_403.Models.ViewModel;

namespace Web_Xem_phim_CS_403.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MovieController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string search, int page = 1, int pageSize = 10)
        {
            var moviesQuery = _context.Movies
                .Include(m => m.MovieCategories).ThenInclude(mc => mc.Category)
                .Include(m => m.Episodes)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                moviesQuery = moviesQuery.Where(m => m.Title.Contains(search));
            }

            var totalMovies = moviesQuery.Count();
            var movies = moviesQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalMovies / pageSize);
            ViewBag.Search = search;

            return View(movies);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MovieVM movieVM)
        {
            if (ModelState.IsValid)
            {
                var movie = new Movie
                {
                    Title = movieVM.Title,
                    Description = movieVM.Description,
                    ReleaseDate = movieVM.ReleaseDate,
                    Duration = movieVM.Duration,
                    PosterURL = movieVM.PosterURL,
                    TrailerURL = movieVM.TrailerURL,
                    Rating = movieVM.Rating,
                    IsSeries = movieVM.IsSeries,
                    TotalEpisodes = movieVM.IsSeries ? movieVM.TotalEpisodes : null // Đặt null nếu không phải series
                };

                // Kiểm tra và thêm danh mục
                if (movieVM.SelectedCategories != null && movieVM.SelectedCategories.Any())
                {
                    foreach (var categoryId in movieVM.SelectedCategories)
                    {
                        movie.MovieCategories.Add(new MovieCategory { CategoryID = categoryId });
                    }
                }

                _context.Movies.Add(movie);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // Đổ lại dữ liệu nếu ModelState không hợp lệ
            ViewBag.Categories = _context.Categories.ToList();
            return View(movieVM);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var movie = _context.Movies
                .Include(m => m.MovieCategories).ThenInclude(mc => mc.Category)
                .Include(m => m.Episodes)
                .FirstOrDefault(m => m.MovieID == id);

            if (movie == null)
            {
                return NotFound();
            }

            var movieVM = new MovieVM
            {
                MovieID = movie.MovieID,
                Title = movie.Title,
                Description = movie.Description,
                ReleaseDate = movie.ReleaseDate,
                Duration = movie.Duration,
                PosterURL = movie.PosterURL,
                TrailerURL = movie.TrailerURL,
                Rating = movie.Rating,
                IsSeries = movie.IsSeries,
                TotalEpisodes = movie.TotalEpisodes,
                SelectedCategories = movie.MovieCategories.Select(mc => mc.CategoryID).ToList(),
                Episodes = movie.Episodes?.Select(e => new EpisodeVM
                {
                    EpisodeID = e.EpisodeID,
                    Title = e.Title,
                    VideoURL = e.VideoURL,
                    Duration = e.Duration,
                    EpisodeNumber = e.EpisodeNumber
                }).ToList()
            };

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.SelectedCategories = movieVM.SelectedCategories ?? new List<int>(); // Đảm bảo không null
            return View(movieVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MovieVM movieVM)
        {
            if (ModelState.IsValid)
            {
                var movie = _context.Movies
                    .Include(m => m.MovieCategories)
                    .FirstOrDefault(m => m.MovieID == movieVM.MovieID);

                if (movie == null)
                {
                    return NotFound();
                }

                movie.Title = movieVM.Title;
                movie.Description = movieVM.Description;
                movie.ReleaseDate = movieVM.ReleaseDate;
                movie.Duration = movieVM.Duration;
                movie.PosterURL = movieVM.PosterURL;
                movie.TrailerURL = movieVM.TrailerURL;
                movie.Rating = movieVM.Rating;
                movie.IsSeries = movieVM.IsSeries;
                movie.TotalEpisodes = movieVM.IsSeries ? movieVM.TotalEpisodes : null; // Đặt null nếu không phải series

                movie.MovieCategories.Clear();
                foreach (var categoryId in movieVM.SelectedCategories)
                {
                    movie.MovieCategories.Add(new MovieCategory { CategoryID = categoryId });
                }

                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(movieVM);
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            var movie = _context.Movies
                .Include(m => m.MovieCategories)
                .Include(m => m.Episodes)
                .FirstOrDefault(m => m.MovieID == id);

            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
