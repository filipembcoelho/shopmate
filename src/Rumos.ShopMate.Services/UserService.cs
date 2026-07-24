using Microsoft.EntityFrameworkCore;
using Rumos.ShopMate.Data;
using Rumos.ShopMate.Domain.Model;
using Rumos.ShopMate.Services.Dtos;

namespace Rumos.ShopMate.Services;

public class UserService(ApplicationContext ctx)
{
    public UserDto GetById(int id) // 1
    {
        var user = ctx.Users.FirstOrDefault(u => u.Id == id);

        if (user is null)
            throw new ArgumentException("Id doesn't exist.");

        return MapToDto(user);
    }

    public List<UserDto> GetAll()
    {
        var users = ctx.Users.ToList();

        var dtos = new List<UserDto>();

        foreach (var user in users)
        {
            dtos.Add(MapToDto(user));
        }

        return dtos;
    }

    public UserWithAccountDto GetUserWithAccountById(int id)
    {
        var user = ctx.Users
            .Include(x => x.Account) // JOIN
            .FirstOrDefault(x => x.Id == id);

        if (user is null)
            throw new ArgumentException("Id doesn't exist.");

        return new UserWithAccountDto()
        {
            Id = user.Id,
            FullName = user.Name.ToString(),
            Username = user.Account.Username
        };
    }

    public void Add(AddUserDto userDto)
    {
        // validações // Exceptions ou Results Pattern
        if (userDto == null)
        {
            throw new ArgumentException("User data cannot be null.");
        }

        if (string.IsNullOrEmpty(userDto.FirstName) || string.IsNullOrEmpty(userDto.LastName))
        {
            throw new ArgumentException("First name and last name cannot be null or empty.");
        }

        ctx.Users.Add(MapToUser(userDto));
        ctx.SaveChanges();
    }

    public void Add(AddUserWithFullNameDto userDto)
    {
        // validações // Exceptions ou Results Pattern
        if (userDto == null)
        {
            throw new ArgumentException("User data cannot be null.");
        }

        if (string.IsNullOrEmpty(userDto.FullName))
        {
            throw new ArgumentException("Full name cannot be null or empty.");
        }

        var user = new User(userDto.FullName, userDto.Username, userDto.Password);

        ctx.Users.Add(user);
        ctx.SaveChanges();
    }

    public void Update(UpdateUserDto userDto)
    {
        // TODO: reuse the same code from above (lot's of repetition) => DRY
        var user = ctx.Users
            .Include(x => x.Account)
            .FirstOrDefault(x => x.Id == userDto.Id);

        if (user is null)
            throw new ArgumentException("Id doesn't exist.");

        user.Name.FirstName = userDto.FirstName;
        user.Name.LastName = userDto.LastName;
        user.Account.Username = userDto.Username;

        ctx.SaveChanges();
    }

    public void Delete(DeleteUserDto userDto)
    {
        var user = ctx.Users.FirstOrDefault(x => x.Id == userDto.Id);

        if (user is null)
            throw new ArgumentException("Id doesn't exist.");

        ctx.Users.Remove(user);
        ctx.SaveChanges();
    }

    private UserDto MapToDto(User user)
    {
        return new UserDto()
        {
            Id = user.Id,
            FullName = user.Name.ToString()
        };
    }

    private User MapToUser(AddUserDto userDto)
    {
        return new User(new Name(userDto.FirstName, userDto.LastName),
            new Account(userDto.Username, userDto.Password));
    }
}