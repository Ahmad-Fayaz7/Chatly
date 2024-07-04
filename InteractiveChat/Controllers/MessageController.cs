using InteractiveChat.Data.Repository;
using InteractiveChat.Data.Repository.IRepository;
using InteractiveChat.DTOs;
using InteractiveChat.Models;
using InteractiveChat.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InteractiveChat.Controllers;
[ApiController]
[Route("api/[controller]")]
public class MessageController(UserManager<ApplicationUser> userManager, 
    IApplicationUserRepository applicationUserRepository, IMessageService messageService) : Controller
{
    [HttpGet]
   public async Task<IActionResult> Chat(string recipient)
   {
       var senderUser = await userManager.GetUserAsync(User);
       var recipientUser = applicationUserRepository.GetByUsername(recipient);
        var users = new[] { senderUser, recipientUser };
        return View(users);
    }

    [HttpGet("conversation")]
    public async Task<ActionResult<List<MessageDto>>> LoadMessages([FromQuery] string senderUsername, [FromQuery] string recipientUsername)
    {
        var senderUser = await userManager.GetUserAsync(User);
        var recipientUser = applicationUserRepository.GetByUsername(recipientUsername);
        var conversationDto = messageService.GetConversationByParticipants(senderUser, recipientUser);
        var messages = conversationDto.Messages;
        return Ok(messages);
    }
}

