using InteractiveChat.Data;
using InteractiveChat.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InteractiveChat.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserController(
            ILogger<HomeController> logger,
            ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> EditAsync()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (user == null) { 
                    return NotFound();
                }
                user.FirstName = applicationUser.FirstName;
                user.LastName = applicationUser.LastName;
                user.ProfilePicUrl = SaveProfilePicAsync(applicationUser);
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
               
            }
            return View(applicationUser);
        }

        private string SaveProfilePicAsync(ApplicationUser user)
        {
            var profilePicsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, @"images\users");
            var profilePicFile = Request.Form.Files["ProfilePicUrl"];
            string uniqueFileName = null;
            if (profilePicFile != null && profilePicFile.Length > 0)
            {
                // Ensure profilePicsDicrectory exists
                if (!Directory.Exists(profilePicsDirectory))
                {
                    Directory.CreateDirectory(profilePicsDirectory);
                }

                // Delete old profile picture if exists
                string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, user.ProfilePicUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                // Generate unique file name
                uniqueFileName = $"{Guid.NewGuid().ToString()}_{profilePicFile.FileName}";

                // Save file to profile pictures directory
                var filePath = Path.Combine(profilePicsDirectory, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    profilePicFile.CopyTo(fileStream);
                }

            }
            // Save file path to database/users table
            return @"\images\users\" + uniqueFileName;
        }

    }
}
