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
        private readonly ITokenHandler _tokenHandler;

        public AccountsController(IUserService userService,ITokenHandler tokenHandler, IMapper mapper)
        {
            _mapper = mapper;
            _userService = userService;
            _tokenHandler = tokenHandler;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest registrationRequest)
        {
            var user = _mapper.Map<User>(registrationRequest);
            await _userService.RegisterAUser(user, registrationRequest.Password);

            return Created("", registrationRequest);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
           var user =  await _userService.LoginAsync(loginRequest);
           string token =  _tokenHandler.GenerateToken(user);

            return Ok(new 
            {
                token
            });
        }
    }
}
