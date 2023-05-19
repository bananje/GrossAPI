using GrossAPI.Models;
using GrossAPI.Models.DTOModel;

namespace GrossAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<Users> Register(RegistrationRequestDTO registrationRequestDTO);
    }
}
