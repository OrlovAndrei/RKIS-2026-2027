using System;

class Program
{
    static TodoList todoList = new TodoList();
    static Profile? profile;

    static void Main()
    {
        PrintHeader();
        RegisterUser();

        Console.WriteLine("\nВведите команду (для справки введите help):");

        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                continue;
            }

            ICommand command = CommandParser.Parse(input, todoList, profile);
            command.Execute();
        }
    }

    static void PrintHeader()
    {
        Console.WriteLine("========================================");
        Console.WriteLine("Работу выполнили: Чичикин и Мартемьянов");
        Console.WriteLine("Группа: 3831");
        Console.WriteLine("========================================\n");
    }

    static void RegisterUser()
    {
        Console.Write("Введите имя: ");
        string firstName = Console.ReadLine() ?? "";
        Console.Write("Введите фамилию: ");
        string lastName = Console.ReadLine() ?? "";

        int birthYear;
        while (true)
        {
            Console.Write("Введите год рождения: ");
            string? yearInput = Console.ReadLine();
            if (int.TryParse(yearInput, out birthYear) && birthYear > 1900 && birthYear <= DateTime.Now.Year)
            {
                break;
            }
            Console.WriteLine("Некорректный год рождения, попробуйте снова.");
        }

        profile = new Profile(firstName, lastName, birthYear);
        Console.WriteLine($"\nДобавлен пользователь {profile.GetInfo()}");
    }
}
