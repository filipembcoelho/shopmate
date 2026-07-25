using Rumos.ShopMate.Services.Dtos;

namespace Rumos.ShopMate.Services.Interfaces;

public interface IUserService
{
    string SuggestUsername(string fullName);
    UserDto? GetByUsername(string username);
}
