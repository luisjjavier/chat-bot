using AutoMapper;
using ChatBot.API.Models;
using ChatBot.Core.Models;
using ChatBot.Core.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.API.Controllers
{
    /// <summary>
    /// Manage all chat room request
    /// </summary>
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/chat-room")]
    [ApiController]
    public class ChatRoomController : ControllerBase
    {
        private readonly IChatRoomService _chatRoomService;
        private readonly IMapper _mapper;

        public ChatRoomController(IChatRoomService chatRoomService, 
            IMapper mapper)
        {
            _chatRoomService = chatRoomService;
            _mapper = mapper;

        }

        /// <summary>
        /// Creates a new room with given parameters
        /// </summary>
        /// <param name="chatRoomRequest"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Retrieve the last 50 messages from a room with a room code
        /// </summary>
        /// <param name="roomCode"></param>
        /// <returns></returns>

        [HttpGet("{roomCode}/Messages")]
        public async Task<IActionResult> Messages(Guid roomCode)
        {
            var messages = await _chatRoomService.GetChatRoomMessages(roomCode);
            return Ok(messages);
        }
    }
}
