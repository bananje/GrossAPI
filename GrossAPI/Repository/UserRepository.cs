using GrossAPI.DataAccess;
using GrossAPI.Models;
using GrossAPI.Models.DTOModel;
using GrossAPI.Repository.IRepository;
using GrossAPI.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GrossAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _db;
        private string secretKey;
        public UserRepository(ApplicationDBContext db, IConfiguration configuration)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.Users.Where(name => name.UserName == username).FirstOrDefault();
            if(user == null)
                return true;

            return false;
        }      

        public async Task<LoginResponseDTO> Login(LoginRequestDTO obj)
        {
            var user = await _db.Users.Where(u => u.UserName == obj.UserName).FirstOrDefaultAsync();
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO();

            if (user == null)
                return loginResponseDTO;

            var savedPasswordHash = user.PasswordHash;

            bool verification = Crypter.VerifyPassword(obj.Password, savedPasswordHash);
            if (verification)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(secretKey);
                var userRole = await _db.UserRoles.FirstOrDefaultAsync(u => u.UserId.Contains(user.UserId));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.UserId.ToString()),
                        new Claim(ClaimTypes.Role, userRole.RoleId)
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                loginResponseDTO = new LoginResponseDTO
                {
                    Token = tokenHandler.WriteToken(token),
                    User = user
                };
            }
            return loginResponseDTO;
        }

        public async Task<Users> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            Users user = new Users()
            {
                UserId = Guid.NewGuid().ToString(),
                UserName = registrationRequestDTO.UserName,
                PasswordHash = Crypter.HashPassword(registrationRequestDTO.Password),
                Name = registrationRequestDTO.Name,
                Surname = registrationRequestDTO.Surname,
                Patronymic = registrationRequestDTO.Patronymic,
                TelNumber= registrationRequestDTO.PhoneNumber,
                Email = registrationRequestDTO.Email,
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            UserRoles userRoles = new UserRoles()
            {
                UserId = user.UserId,
                RoleId = WC.CustomerRoleId
            };
            _db.UserRoles.Add(userRoles);
            await _db.SaveChangesAsync();
            
            return user;           
        }       
    }
}
