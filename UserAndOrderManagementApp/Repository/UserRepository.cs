    using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserAndOrderManagement.Data;
using UserAndOrderManagement.Models.Dto;
using UserAndOrderManagement.Models;
using UserAndOrderManagementApp.Models.Dto;
using UserAndOrderManagementApp.Repository.IRepository;
using Azure;

namespace UserAndOrderManagementApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private string secretKey;
        private readonly IMapper _mapper;

        public UserRepository(ApplicationDbContext db, IConfiguration configuration,
            UserManager<ApplicationUser> userManager, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _mapper = mapper;
            _userManager = userManager;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _roleManager = roleManager;
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.UserName == username);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers
                .FirstOrDefault(u => u.UserName == loginRequestDto.UserName);

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);


            if (user == null || isValid == false)
            {
                return new LoginResponseDto()
                {
                    Token = "",
                    User = null
                };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                Token = tokenHandler.WriteToken(token),
                User = _mapper.Map<UserDto>(user),

            };
            return loginResponseDto;
        }

        public async Task<UserDto> Register(RegisterationRequestDto registerationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registerationRequestDto.UserName,
                Email = registerationRequestDto.UserName,
                NormalizedEmail = registerationRequestDto.UserName.ToUpper(),
                Name = registerationRequestDto.Name
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registerationRequestDto.Password);
                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new IdentityRole("admin"));
                        await _roleManager.CreateAsync(new IdentityRole("customer"));
                    }
                    await _userManager.AddToRoleAsync(user, "admin");
                    var userToReturn = _db.ApplicationUsers
                        .FirstOrDefault(u => u.UserName == registerationRequestDto.UserName);
                    return _mapper.Map<UserDto>(userToReturn);
                }

                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {

            }

            return new UserDto();
        }
    }
}
