using AutoMapper;
using InteractiveChat.Data;
using InteractiveChat.DTOs;
using InteractiveChat.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InteractiveChat.Controllers;

public class UserController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<HomeController> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public UserController(
        ILogger<HomeController> logger,
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IWebHostEnvironment webHostEnvironment,
        IMapper mapper
    )

    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _webHostEnvironment = webHostEnvironment;
        _mapper = mapper;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> EditAsync()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var userDto = _mapper.Map<ApplicationUserDTO>(user);
        return View(userDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ApplicationUserDTO applicationUserDto)
    {
        if (!ModelState.IsValid) return View(applicationUserDto);
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user == null) return NotFound();

        // Update only the necessary properties
        user.FirstName = applicationUserDto.FirstName;
        user.LastName = applicationUserDto.LastName;

        // Handle the profile picture
        if (applicationUserDto.ProfilePicUrl != null)
            user.ProfilePicUrl = SaveProfilePicAsync(applicationUserDto.ProfilePicUrl);
        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded) return RedirectToAction("Index", "Home");
        foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);

        return View(applicationUserDto);
    }

    private string SaveProfilePicAsync(string userProfilePic)
    {
        var profilePicsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, @"images\users");
        var profilePicFile = Request.Form.Files["ProfilePicUrl"];
        string uniqueFileName = null;
        if (profilePicFile != null && profilePicFile.Length > 0)
        {
            // Ensure profilePicsDicrectory exists
            if (!Directory.Exists(profilePicsDirectory)) Directory.CreateDirectory(profilePicsDirectory);

            // Delete old profile picture if exists
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, userProfilePic.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath) && !oldImagePath.Contains("profile-placeholder.png"))
                System.IO.File.Delete(oldImagePath);

            // Generate unique file name
            uniqueFileName = $"{Guid.NewGuid().ToString()}_{profilePicFile.FileName}";

            // Save file to profile pictures directory
            var filePath = Path.Combine(profilePicsDirectory, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                profilePicFile.CopyTo(fileStream);
            }
        }
        else
        {
            uniqueFileName = "profile-placeholder.png";
        }

        // Save file path to database/users table
        return @"\images\users\" + uniqueFileName;
    }
}