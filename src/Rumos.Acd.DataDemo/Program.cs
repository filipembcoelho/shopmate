using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Rumos.Acd.DataDemo;

Console.WriteLine($"Hello world");

// Entity Framework Core

ApplicationContext db = new ApplicationContext();
//db.Database.EnsureDeleted();
db.Database.Migrate();

foreach (var dbUser in db.Users)
{


    dbUser.FirstName = dbUser.FirstName.ToUpper();

    if (dbUser.FirstName == "John")
    {
        db.Users.Remove(dbUser);
    }

}



// LINQ

//User user = new User("Edson", "Manuel")
//{
//    IdCard = "123456",
//    TaxNumber = "987654",
//    Account = new Account
//    {
//        Username = "emanuel",
//        Password = "password123"
//    },
//};

//user.Contacts.Add(new Contact
//{
//    Value = "123456789",
//    Type = ContactType.Phone
//});


//user.Addresses.Add(new Address
//{
//    StreetOne = "123 Main St",
//    City = "Luanda",
//    ZipCode = "1000",
//    Country = "Angola",
//    Type = AddressType.Billing
//});

//db.Users.Add(user); // x0009

//db.Users.Remove(user); // x0009

db.SaveChanges();

