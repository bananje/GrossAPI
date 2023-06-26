using GrossAPI.Models;
using GrossAPI.Models.DTOModel;
using GrossAPI.Repository.IRepository;
using GrossAPI.Utils;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace GrossAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;

        public UserController(IUserRepository userRepo, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
        {
            _userRepo = userRepo;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
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
            if (model.UserName == "" || model.PhoneNumber == "" || model.Email == ""
               || model.Password == "")
                return NotFound();

            bool isUserNameUnique = _userRepo.IsUniqueUser(model.UserName);
            if (!isUserNameUnique)
                return BadRequest();


            var user = await _userRepo.Register(model, WC.CustomerRoleId);
            if (user == null)
                return NotFound();

            var pathToForm = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString() + "Forms"
                                                             + Path.DirectorySeparatorChar.ToString() + "RegisterForm.html";
            var subject = "Регистрация на сайте Бухгалтерии Гросс";
            string htmlBody;
            using (StreamReader sr = System.IO.File.OpenText(pathToForm))
            {
                htmlBody = sr.ReadToEnd();
            }

            string encodedVariable = HttpUtility.HtmlEncode((user.Name + " " + user.Patronymic).ToString());
            string formattedHtml = string.Format(htmlBody, encodedVariable);

            await _emailSender.SendEmailAsync(user.Email, subject, formattedHtml);

            return Ok();
        }

        
        [HttpPost("registeradmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegistrationRequestDTO model)
        {
            bool isUserNameUnique = _userRepo.IsUniqueUser(model.UserName);
            if (!isUserNameUnique)
                return BadRequest();

            var user = await _userRepo.Register(model, WC.AdminRoleId);
            if (user == null)
                return BadRequest();
         
            return Ok();
        }
    }
}
