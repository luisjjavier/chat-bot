using AutoMapper;
using ChatBot.API.Models;
using ChatBot.Core.Models;
using ChatBot.Core.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public AccountsController(IUserService userService, IMapper mapper)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest registrationRequest)
        {
            var user = _mapper.Map<User>(registrationRequest);
            await _userService.RegisterAUser(user, registrationRequest.Password);

            return Created("", registrationRequest);
        }
    }
}
