using Rumos.ShopMate.Services.Dtos;

namespace Rumos.ShopMate.Services.Interfaces;

public interface IAuthenticationService
{
    UserDto? Login(string username, string password);
    UserDto Register(RegisterUserDto userDto);
}
