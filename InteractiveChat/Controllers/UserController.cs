﻿using InteractiveChat.Models;
using Microsoft.AspNetCore.Mvc;

namespace InteractiveChat.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public UserController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Edit()
        {
            return View();
        }
        public IActionResult Profile()
        {
            var username = HttpContext.User.Identity.Name;
            Console.WriteLine(username);

            ViewData["Username"] = username;
            return View();
        }
    }
}
