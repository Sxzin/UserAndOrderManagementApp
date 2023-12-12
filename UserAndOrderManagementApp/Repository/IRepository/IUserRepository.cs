using UserAndOrderManagement.Models.Dto;
using UserAndOrderManagementApp.Models.Dto;

namespace UserAndOrderManagementApp.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDTO);
        Task<UserDto> Register(RegisterationRequestDto registerationRequestDTO);

    }
}
