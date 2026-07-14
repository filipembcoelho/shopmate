Console.WriteLine($"Hello world");

// Entity Framework Core


class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string IdCard { get; set; }
    public string TaxNumber { get; set; }
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

