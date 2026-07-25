using Microsoft.EntityFrameworkCore;
using Rumos.ShopMate.Data;
using Rumos.ShopMate.Domain.Model;
using Rumos.ShopMate.Services.Dtos;
using Rumos.ShopMate.Services.Exceptions;
using Rumos.ShopMate.Services.Interfaces;
using Rumos.ShopMate.Services.Mappers;

namespace Rumos.ShopMate.Services.Implementations;

public class AuthenticationService(ApplicationContext context) : IAuthenticationService
{
    public UserDto? Login(string username, string password)
    {
        ValidateRequired(username, "Username is required.");
        ValidateRequired(password, "Password is required.");

        var normalizedUsername = username.Trim().ToLower();
        var user = context.Users
            .Include(existingUser => existingUser.Account)
            .AsNoTracking()
            .SingleOrDefault(existingUser =>
                existingUser.Account.Username == normalizedUsername &&
                existingUser.Account.Password == password);

        return user == null ? null : DtoMapper.ToDto(user);
    }

    public UserDto Register(RegisterUserDto userDto)
    {
        if (userDto == null)
        {
            throw new ServiceException("User data is required.");
        }

        ValidateRequired(userDto.FullName, "Full name is required.");
        ValidateRequired(userDto.Username, "Username is required.");
        ValidateRequired(userDto.Password, "Password is required.");

        var normalizedUsername = userDto.Username.Trim().ToLower();
        var usernameExists = context.Users
            .Any(user => user.Account.Username == normalizedUsername);

        if (usernameExists)
        {
            throw new ServiceException("Username already exists.");
        }

        var user = new User(
            userDto.FullName,
            normalizedUsername,
            userDto.Password);

        context.Users.Add(user);
        context.SaveChanges();

        return DtoMapper.ToDto(user);
    }

    private static void ValidateRequired(string value, string message)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ServiceException(message);
        }
    }
}
