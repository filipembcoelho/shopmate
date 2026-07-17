Console.WriteLine($"Hello world");

// Entity Framework Core

ApplicationContext db = new ApplicationContext();
// INSERTS, Udpate, Delete

// Model first

// Migrations // Add-Migration "Initial state"
// Update DB schema // Update-Database

foreach (var firstName in db.Users)
{
    Console.WriteLine(firstName);
}

db.Users.Add(new User("John", "Doe"));
db.SaveChanges();

public class User
{
    public int Id { get; set; } // PK
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string IdCard { get; set; }
    public string TaxNumber { get; set; }
    public string Nickname { get; set; }
    public int Age { get; set; }
    public Account Account { get; set; } // Composition =>  1-1
    public List<Contact> Contacts { get; set; } // 1-n
    public List<Address> Addresses { get; set; } // n-n

    public User(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}

public class Account // 1 - 1
{
    public int Id { get; set; } // PK
    public string Username { get; set; }
    public string Password { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}

public class Contact
{
    public int Id { get; set; }
    public string Value { get; set; }
    public ContactType Type { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } // navigation property
}

public class Address // n-n
{
    public int Id { get; set; }
    public string StreetOne { get; set; }
    public string StreetTwo { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public string Country { get; set; }
    public AddressType Type { get; set; }
    public List<User> Users { get; set; }
}

public enum AddressType
{
    Billing,
    Shipping
}

public enum ContactType
{
    Phone,
    Email,
}