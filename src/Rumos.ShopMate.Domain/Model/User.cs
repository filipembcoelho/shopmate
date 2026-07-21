using Rumos.ShopMate.Domain.Exceptions;
using Rumos.ShopMate.Domain.Model.Common;

namespace Rumos.ShopMate.Domain.Model;

public class User : Entity
{
    public Name Name { get; set; }
    public Account Account { get; set; }

    public User(string fullName, string username, string password)
    {
        Name = Name.Parse(fullName);
        Account = new Account(username, password);
    }

    public User(Name name, Account account)
    {
        if (name == null)
        {
            throw new InvalidUserException("Name is required.");
        }

        if (account == null)
        {
            throw new InvalidUserException("Account is required.");
        }

        Name = name;
        Account = account;
    }

    public override string ToString()
    {
        return Name.ToString() + " (" + Account.Username + ")";
    }
}