using Rumos.ShopMate.Domain.Exceptions;

namespace Rumos.ShopMate.Domain.Model;

public class ShoppingList
{
    private List<ShoppingListMember> _users; // x010
    public string Name { get; set; }
    public User Owner { get; set; }

    public IReadOnlyList<ShoppingListMember> Users
    {
        get
        {
            return _users.AsReadOnly();
        }
        private set;
    } // x011

    public List<ShoppingListItem> Items { get; set; }

    public DateTime ExpireDate { get; set; }
    public bool IsArchive { get; set; }

    private ShoppingList()
    {
        _users = new List<ShoppingListMember>();
        Users = new List<ShoppingListMember>();
        Items = new List<ShoppingListItem>();
    }

    public ShoppingList(string name, User owner) : this()
    {
        Name = name;
        Owner = owner;
    }

    // add and remove users
    public void AddUser(ShoppingListMember user)
    {
        foreach (ShoppingListMember member in _users)
        {
            if (member.User.Account.Username == user.User.Account.Username)
            {
                throw new DuplicateMemberException("User already exists.");
            }
        }

        _users.Add(user);
    }

    // Add and remove items
}