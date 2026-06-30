namespace Rumos.ShopMate.Domain.Model;

public class User
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
        Name = name;
        Account = account;
    }
}