using InteractiveChat.Models;
using InteractiveChat.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InteractiveChat.ViewComponents;

public class ConversationListViewComponent(UserManager<ApplicationUser> userManager, IMessageService messageService)
    : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var user = await userManager.GetUserAsync(UserClaimsPrincipal);
        if (user == null)
        {
            return Content("User not found");
        }
        var conversations =  messageService.GetAllConversationsOfUser(user);
        return View(conversations);
    }
}