using GrossAPI.Models;
using GrossAPI.Models.DTOModel;
using GrossAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace GrossAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepo;

        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var loginResponse = await _userRepo.Login(model);
            if (loginResponse.User == null || string.IsNullOrEmpty(loginResponse.Token))
                return BadRequest();

            return Ok(loginResponse);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO model)
        {
            bool isUserNameUnique = _userRepo.IsUniqueUser(model.UserName);
            if (!isUserNameUnique)
                return BadRequest();


            var user = await _userRepo.Register(model);
            if (user == null)
                return BadRequest();

            return Ok();
        }
    }
}
