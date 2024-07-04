using InteractiveChat.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InteractiveChat.ViewComponents;

public class ProfileViewComponent(UserManager<ApplicationUser> userManager) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var user = await userManager.GetUserAsync(HttpContext.User);
        return View(user);
    }
}