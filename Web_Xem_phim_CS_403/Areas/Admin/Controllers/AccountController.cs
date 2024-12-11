using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Xem_phim_CS_403.Data;
using Web_Xem_phim_CS_403.Models;

namespace Web_Xem_phim_CS_403.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var accounts = _context.Accounts.Include(a => a.Role).ToList();
            return View(accounts);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Roles = _context.Roles.ToList(); // Lấy danh sách Role từ DB
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Account account)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _context.Roles.ToList(); // Gửi lại danh sách Role nếu ModelState không hợp lệ
                return View(account);
            }

            _context.Accounts.Add(account);
            _context.SaveChanges();
            TempData["Success"] = "Account created successfully!";
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var account = _context.Accounts.Find(id);
            if (account == null) return NotFound();

            ViewBag.Roles = _context.Roles.ToList(); // Lấy danh sách Role từ DB
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Account account)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _context.Roles.ToList(); // Gửi lại danh sách Role nếu ModelState không hợp lệ
                return View(account);
            }

            _context.Accounts.Update(account);
            _context.SaveChanges();
            TempData["Success"] = "Account updated successfully!";
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            var account = _context.Accounts.Find(id);
            if (account == null) return NotFound();

            _context.Accounts.Remove(account);
            _context.SaveChanges();
            TempData["Success"] = "Account deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
