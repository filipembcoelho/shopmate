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
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
    }

    public static Name Parse(string fullName)
    {
        fullName = fullName.Trim();
        string[] names = fullName.Split(" ");
        
        string firstName = names[0];
        string lastName = names[names.Length - 1];

        Name newName = new Name(firstName, lastName);

        if (fullName.Split(" ").Length > 2)
        {
            for (int i = 1; i < fullName.Split(" ").Length - 1; i++)
            {
                newName.MiddleNames.Add(fullName.Split(" ")[i].Trim());
            }
        }

        return newName;
    }
}
