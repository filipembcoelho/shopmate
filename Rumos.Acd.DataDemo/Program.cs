
var ds = new User
{
    FirstName = "Daniel",
    LastName = "Paiva",
    IdCard = "123456789",
    TaxNumber = "123",
    Account = new Account
    {
        Username = "dpaive",
        Password = "xxx"
    }
};

// Save Changes();


class User
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string IdCard { get; set; }
    public string TaxNumber { get; set; }
    public string PhoneNumber1 { get; set; }
    public string PhoneNumber2 { get; set; }
    public string PhoneNumber3 { get; set; }
    public string MobileNumber1 { get; set; }
    public string Email { get; set; }
    public Account Account { get; set; } // 1-1
    public List<Address> Addresses { get; set; } // n-n
    public List<Contact> Contacts { get; set; } // 1-n
}

class Contact
{
    public string Value { get; set; }
    public ContactType Type { get; set; }
    public User User { get; set; }
}

enum ContactType
{
    Phone,
    Email,
}

class Account // 1 - 1
{
    public string Username { get; set; }
    public string Password { get; set; }
    public User User { get; set; }
}

class Address // n-n
{
    public string StreetOne { get; set; }
    public string StreetTwo { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public string Country { get; set; }
    public AddressType Type { get; set; }
    public List<User> Users { get; set; } // 6; 7
}

enum AddressType
{
    Billing,
    Shipping
}