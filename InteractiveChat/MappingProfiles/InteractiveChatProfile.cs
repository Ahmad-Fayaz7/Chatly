using AutoMapper;
using InteractiveChat.DTOs;
using InteractiveChat.Models;

namespace InteractiveChat.MappingProfiles
{
    public class InteractiveChatProfile : Profile
    {
        public InteractiveChatProfile()
        {
            CreateMap<ApplicationUser, ApplicationUserDTO>();
            CreateMap<ApplicationUserDTO, ApplicationUser>();
        }
    }
}
