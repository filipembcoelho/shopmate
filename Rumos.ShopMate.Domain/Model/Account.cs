namespace Rumos.ShopMate.Domain.Model;

public class Account
{
    public string Username { get; set; }
    public string Password { get; set; }

    public Account(string username, string password)
    {
        Username = username;
        if (password.Length != 8)
        {
            throw new ArgumentException("Password must be 8 characters long.");
        }

        Password = password;
        // Exception
    }
}