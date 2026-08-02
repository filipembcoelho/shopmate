using Microsoft.EntityFrameworkCore;
using Rumos.ShopMate.Data;
using Rumos.ShopMate.Domain.Utils;
using Rumos.ShopMate.Services.Dtos;
using Rumos.ShopMate.Services.Exceptions;
using Rumos.ShopMate.Services.Interfaces;
using Rumos.ShopMate.Services.Mappers;

namespace Rumos.ShopMate.Services.Implementations;

public class UserService(ApplicationContext context) : IUserService // new ApplciationContext();
{
    public string SuggestUsername(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ServiceException("Full name is required.");
        }

        var users = context.Users
            .Include(user => user.Account)
            .AsNoTracking()
            .ToList();

        return UsernameUtils.SuggestUsername(fullName, users);
    }

    public UserDto GetByUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ServiceException("Username is required.");
        }

        var normalizedUsername = username.Trim().ToLower();
        var user = context.Users
            .Include(existingUser => existingUser.Account)
            .AsNoTracking()
            .SingleOrDefault(existingUser =>
                existingUser.Account.Username == normalizedUsername);

        if (user is null)
        {
            throw new ServiceNotFoundException("User not found.");
        }

        return DtoMapper.ToDto(user);
    }
}
