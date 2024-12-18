using Microsoft.AspNetCore.Mvc;
using Web_Xem_phim_CS_403.Models;
using System.Linq;
using System.Threading.Tasks;
using Web_Xem_phim_CS_403.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Web_Xem_phim_CS_403.Areas.User.Controllers
{
    [Area("User")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            var account = _context.Accounts
                .Include(a => a.Role)
                .FirstOrDefault(a => a.Email == email && a.Password == password);

            if (account == null)
            {
                TempData["Error"] = "Invalid email or password!";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.Name),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Role, account.Role.RoleName)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home", new { area = "User" });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Account account)
        {
            if (!ModelState.IsValid)
            {
                return View(account);
            }

            _context.Accounts.Add(account);
            _context.SaveChanges();
            TempData["Success"] = "Registration successful!";
            return RedirectToAction("Login");
        }
    }
}
