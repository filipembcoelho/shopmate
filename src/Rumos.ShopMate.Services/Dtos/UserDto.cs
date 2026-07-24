namespace Rumos.ShopMate.Services.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
}

public class UserWithAccountDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Username { get; set; }
}

public class AddUserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

public class UpdateUserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
}

public class DeleteUserDto
{
    public int Id { get; set; }
}

// TODO: Add FullName on AddUserDto 
public class AddUserWithFullNameDto
{
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}