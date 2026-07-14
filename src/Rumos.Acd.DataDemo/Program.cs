using Microsoft.EntityFrameworkCore;

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

class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Address> Addresses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // var cs = "Server=localhost;Database=UserDemo;Trusted_Connection=True;TrustServerCertificate=True;";
        var cs = "Server=94.46.180.24;Database=codedev2026;User Id=fcoelho; Password=NS8cVzc2_kpfrc5@;TrustServerCertificate=True;";
        options.UseSqlServer(cs);
    }
}

class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string IdCard { get; set; }
    public string TaxNumber { get; set; }
    public string Nickname { get; set; }
    public Account Account { get; set; } // Composition =>  1-1
    public List<Contact> Contacts { get; set; } // 1-n
    public List<Address> Addresses { get; set; } // n-n

    public User(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}

class Account // 1 - 1
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public User User { get; set; }
}

class Contact
{
    public int Id { get; set; }
    public string Value { get; set; }
    public ContactType Type { get; set; }
    public User User { get; set; } // navigation property
}

class Address // n-n
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

enum AddressType
{
    Billing,
    Shipping
}

enum ContactType
{
    Phone,
    Email,
}