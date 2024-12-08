using Microsoft.AspNetCore.Mvc;

namespace Web_Xem_phim_CS_403.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
