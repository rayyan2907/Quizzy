﻿using Microsoft.AspNetCore.Mvc;

namespace Quizzy.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View("login_page");
        }

        public IActionResult register()
        {
            return View("register_page");
        }
    }
}
