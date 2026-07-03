using Rumos.ShopMate.Domain.Exceptions;

namespace Rumos.ShopMate.Domain.Model;

public class Name
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<string> MiddleNames { get; set; }

    private Name()
    {
        MiddleNames = new List<string>();
    }

    public Name(string firstName, string lastName) : this()
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new InvalidNameException("First name is required.");
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new InvalidNameException("Last name is required.");
        }

        if (firstName.Trim().Length < 2)
        {
            throw new InvalidNameException("First name must be at least 2 characters long.");
        }

        if (lastName.Trim().Length < 2)
        {
            throw new InvalidNameException("Last name must be at least 2 characters long.");
        }

        if (firstName.Trim().Length > 40)
        {
            throw new InvalidNameException("First name must be at most 40 characters long.");
        }

        if (lastName.Trim().Length > 40)
        {
            throw new InvalidNameException("Last name must be at most 40 characters long.");
        }

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
    }

    public static Name Parse(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new InvalidNameException("Full name is required.");
        }

        fullName = fullName.Trim();
        var names = fullName.Split(" ", StringSplitOptions.RemoveEmptyEntries);

        if (names.Length < 2)
        {
            throw new InvalidNameException("Please enter first and last name.");
        }

        var firstName = names[0];
        var lastName = names[names.Length - 1];

        var newName = new Name(firstName, lastName);

        if (names.Length > 2)
        {
            for (var i = 1; i < names.Length - 1; i++)
            {
                if (names[i].Trim().Length < 2)
                {
                    throw new InvalidNameException("Middle names must be at least 2 characters long.");
                }

                newName.MiddleNames.Add(names[i].Trim());
            }
        }

        return newName;
    }

    public override string ToString()
    {
        if (MiddleNames.Count == 0)
        {
            return FirstName + " " + LastName;
        }

        var middleNames = string.Join(" ", MiddleNames);
        return FirstName + " " + middleNames + " " + LastName;
    }
}
