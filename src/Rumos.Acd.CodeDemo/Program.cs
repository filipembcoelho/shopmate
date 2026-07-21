using System.Globalization;

Console.WriteLine("Hello, World!");


List<string> names = new List<string>();
names.Add("Bruno");
names.Add("Filipe");
names.Add("Edson");
names.Add("Rodrigo");

// IEnumerable

// DbSet : 



for (int i = 0; i < names.Count; i++)
{
    if (names[i].Length > 5)
    {
        Console.WriteLine(names[i]);
    }
}

foreach (var thing in names)
{
    Console.WriteLine(thing);
}

// LINQ


var namesGreaterThanFive = names
    .Where(name => name.Length > 5)
    .ToList();


// name => name.Length > 5



// ---------------
INotifier cons = new ConsoleNotifier();
cons.Send("Hello, World!");

INotifier mob = new MobileNotifier();
mob.Send("Hello, World!");

EmailNotifier ema = new EmailNotifier();
SmsNotifier sms = new SmsNotifier();


interface INotifier
{
    void Send(string message);
}

class ConsoleNotifier : INotifier // is-a
{
    public void Send(string message)
    {
    }
}

interface Notifiable
{
}

// inherits
// implements

class MobileNotifier : INotifier
{
    public void Send(string message)
    {
    }
}


class EmailNotifier : INotifier
{
    public void Send(string message)
    {
        Console.WriteLine(message);
    }
}

class SmsNotifier : INotifier
{
    public void Send(string message)
    {
        Console.WriteLine(message);
    }
}