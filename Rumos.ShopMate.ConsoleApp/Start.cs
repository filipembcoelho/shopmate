using Rumos.ShopMate.Data;
using Rumos.ShopMate.Domain.Model;

public static class Start
{
    public static void Run()
    {
        bool exit = false;

        do
        {
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.WriteLine("0. Exit");
            Console.Write("Enter option: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    Login();
                    break;
                case "2":
                    Register();
                    break;
                case "0":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    Run();
                    break;
            }
        } while (!exit);
    }


    private static void Login()
    {
        User foundUser = null;

        do
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            foreach (User user in Database.Users)
            {
                if (user.Account.Username == username && user.Account.Password == password)
                {
                    foundUser = user;
                }
            }

            if (foundUser == null)
            {
                Console.Clear();
                Console.WriteLine("Invalid credentials, enter again.");
            }
        } while (foundUser == null);

        Console.WriteLine($"Hello, {foundUser.Name}!");
    }

    private static void Register()
    {
        Console.Write("Enter name: ");
        string name = Console.ReadLine();

        Console.Write("Enter username: ");
        string username = Console.ReadLine();

        bool isPassValdiated = false;
        string password = null;
        do
        {
            Console.Write("Enter password (8 characters long): ");
            password = Console.ReadLine();

            if (password.Length != 8)
            {
                Console.WriteLine("Password must be 8 characters long.");
            }
            else
            {
                isPassValdiated = true;
            }
        } while (!isPassValdiated);


        User user = new User(name, username, password);
        Database.Users.Add(user);
    }
}