using Microsoft.AspNetCore.Mvc;
using Web_Xem_phim_CS_403.Data;
using Web_Xem_phim_CS_403.Models;

namespace Web_Xem_phim_CS_403.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
       
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        // Form thêm mới Category
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Xử lý thêm mới Category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // Form chỉnh sửa Category
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // Xử lý cập nhật Category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // Xóa Category
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
