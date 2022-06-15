using AutoMapper;
using ChatBot.API.Models;
using ChatBot.Core.Models;
using ChatBot.Core.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ChatBot.API.Controllers
{
    /// <summary>
    /// Account controller which manage all accounts request
    /// </summary>
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

        /// <summary>
        /// Allows a user to register
        /// </summary>
        /// <param name="registrationRequest"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest registrationRequest)
        {
            var user = _mapper.Map<User>(registrationRequest);
            await _userService.RegisterAUser(user, registrationRequest.Password);

            return Created("", registrationRequest);
        }

        /// <summary>
        /// Allow a user to login
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
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
