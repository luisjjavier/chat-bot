using AutoMapper;
using ChatBot.API.Models;
using ChatBot.Core.Models;
using ChatBot.Core.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/chat-room")]
    [ApiController]

    public class ChatRoomController : ControllerBase
    {
        private readonly IChatRoomService _chatRoomService;
        private readonly IMapper _mapper;

        public ChatRoomController(IChatRoomService chatRoomService, IMapper mapper)
        {
            _chatRoomService = chatRoomService;
            _mapper = mapper;

        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] CreateChatRoomRequest chatRoomRequest)
        {
            var chatRoom = _mapper.Map<Room>(chatRoomRequest);
            chatRoom.Code = Guid.NewGuid();
            await _chatRoomService.CreateNewRoomAsync(chatRoom);

            return Ok(new
            {
                code= chatRoom.Code
            });
        }
    }
}
