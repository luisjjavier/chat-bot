using AutoMapper;
using ChatBot.API.Models;
using ChatBot.Core.Models;

namespace ChatBot.API.MapperProfiles
{
    public sealed class ChatBotProfiles : Profile
    {
        public ChatBotProfiles()
        {
            CreateMap<RegistrationRequest, User>();
        }

    }
}
