﻿using Microsoft.AspNetCore.Mvc;

namespace Web_Xem_phim_CS_403.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
