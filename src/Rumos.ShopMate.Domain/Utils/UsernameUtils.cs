using Rumos.ShopMate.Domain.Model;

namespace Rumos.ShopMate.Domain.Utils;

public static class UsernameUtils
{
    public static string SuggestUsername(string fullName, IReadOnlyList<User> existingUsers)
    {
        var baseUsername = CreateBaseUsername(fullName);
        var suggestedUsername = baseUsername;
        var number = 2;

        while (UsernameExists(suggestedUsername, existingUsers))
        {
            suggestedUsername = baseUsername + number;
            number++;
        }

        return suggestedUsername;
    }

    private static string CreateBaseUsername(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            return "user";
        }

        var names = fullName.Trim().ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries);

        if (names.Length == 1)
        {
            return CleanUsername(names[0]);
        }

        var firstName = names[0];
        var lastName = names[names.Length - 1];
        var username = firstName[0] + lastName;

        return CleanUsername(username);
    }

    private static string CleanUsername(string username)
    {
        var cleanUsername = "";

        foreach (var character in username)
        {
            if (char.IsLetterOrDigit(character))
            {
                cleanUsername = cleanUsername + character;
            }
        }

        if (cleanUsername.Length == 0)
        {
            return "user";
        }

        return cleanUsername;
    }

    private static bool UsernameExists(string username, IReadOnlyList<User> existingUsers)
    {
        foreach (var user in existingUsers)
        {
            if (user.Account.Username == username)
            {
                return true;
            }
        }

        return false;
    }
}
