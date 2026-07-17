using Rumos.Acd.AdoDemo;

var connectionString =
    "Server=localhost;Database=UserDemo;Trusted_Connection=True;TrustServerCertificate=True;";

var userRepository = new UserRepository(connectionString);

Console.WriteLine("--- Create ---");

var userToCreate = new User
{
    FirstName = "ADO",
    LastName = "Student",
    IdCard = "ADO-001",
    TaxNumber = "ADO-001"
};

var createdUser = userRepository.Create(userToCreate);

if (createdUser == null)
{
    return;
}

Console.WriteLine($"Created user with Id {createdUser.Id}.");

Console.WriteLine("--- Read ---");

var user = userRepository.GetById(createdUser.Id);

if (user == null)
{
    Console.WriteLine("User was not found.");
}
else
{
    Console.WriteLine($"User: {user.FirstName} {user.LastName}");

    Console.WriteLine("--- Update ---");

    user.LastName = "Student Updated";

    if (userRepository.Update(user))
    {
        Console.WriteLine("Updated the user's last name.");
    }

    Console.WriteLine("--- Delete ---");

    if (userRepository.Delete(user.Id))
    {
        Console.WriteLine("Deleted the temporary user.");
    }
}
