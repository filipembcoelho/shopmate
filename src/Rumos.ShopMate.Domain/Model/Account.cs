using Rumos.ShopMate.Domain.Exceptions;
using Rumos.ShopMate.Domain.Model.Common;

namespace Rumos.ShopMate.Domain.Model;

public class Account : AuditableEntity
{
    private const int MinimumPasswordLength = 8;
    private const string SpecialCharacters = "!@#$%^&*()-_=+[]{};:,.<>?/";

    public string Username { get; set; }
    public string Password { get; set; }

    public Account(string username, string password)
    {
        ChangeUsername(username);
        ChangePassword(password);
    }

    public void ChangeUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new InvalidAccountException("Username is required.");
        }

        if (username.Trim().Length < 3)
        {
            throw new InvalidAccountException("Username must be at least 3 characters long.");
        }

        if (username.Trim().Length > 20)
        {
            throw new InvalidAccountException("Username must be at most 20 characters long.");
        }

        if (username.Contains(" "))
        {
            throw new InvalidAccountException("Username cannot contain spaces.");
        }

        foreach (var character in username)
        {
            if (!char.IsLetterOrDigit(character))
            {
                throw new InvalidAccountException("Username can only contain letters and numbers.");
            }
        }

        Username = username.Trim().ToLower();
    }

    public void ChangePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidAccountException("Password is required.");
        }

        if (!HasMinimumLength(password))
        {
            throw new InvalidAccountException("Password must have at least 8 characters.");
        }

        if (!HasCapitalLetter(password))
        {
            throw new InvalidAccountException("Password must contain at least one capital letter.");
        }

        if (!HasLowercaseLetter(password))
        {
            throw new InvalidAccountException("Password must contain at least one lowercase letter.");
        }

        if (!HasNumber(password))
        {
            throw new InvalidAccountException("Password must contain at least one number.");
        }

        if (!HasSpecialCharacter(password))
        {
            throw new InvalidAccountException("Password must contain at least one special character.");
        }

        Password = password;
    }

    private bool HasMinimumLength(string password)
    {
        return password.Length >= MinimumPasswordLength;
    }

    private bool HasCapitalLetter(string password)
    {
        foreach (var character in password)
        {
            if (char.IsUpper(character))
            {
                return true;
            }
        }

        return false;
    }

    private bool HasLowercaseLetter(string password)
    {
        foreach (var character in password)
        {
            if (char.IsLower(character))
            {
                return true;
            }
        }

        return false;
    }

    private bool HasNumber(string password)
    {
        foreach (var character in password)
        {
            if (char.IsDigit(character))
            {
                return true;
            }
        }

        return false;
    }

    private bool HasSpecialCharacter(string password)
    {
        foreach (var character in password)
        {
            if (SpecialCharacters.Contains(character))
            {
                return true;
            }
        }

        return false;
    }
}
