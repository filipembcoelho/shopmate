using Rumos.ShopMate.Services.Dtos;

namespace Rumos.ShopMate.Services.Interfaces;

public interface ITokenService
{
    TokenDto GenerateToken(UserDto userDto);
}