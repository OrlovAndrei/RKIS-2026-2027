using System;
using System.IO;

namespace TodoListApp;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Работу выполнили Илиадис и Дедкова");

        FileManager.EnsureDataDirectory(FileManager.DataDirectory);

        if (!File.Exists(FileManager.ProfilePath))
            File.WriteAllText(FileManager.ProfilePath, "");

        if (!File.Exists(FileManager.TodoPath))
            File.WriteAllText(FileManager.TodoPath, "");

        Profile? profile = FileManager.LoadProfile(FileManager.ProfilePath);
        TodoList todoList = FileManager.LoadTodos(FileManager.TodoPath);

        if (profile == null)
        {
            profile = CreateProfile();
            FileManager.SaveProfile(profile, FileManager.ProfilePath);
        }
        else
        {
            Console.WriteLine($"\nС возвращением, {profile.FirstName}!");
        }

        Console.WriteLine("\nДобро пожаловать в TodoList!");
        Console.WriteLine("Введите help для списка команд.");

        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();

            if (input == null)
                break;

            if (string.IsNullOrWhiteSpace(input))
                continue;

            ICommand command = CommandParser.Parse(input, todoList, profile);

            if (command is ExitCommand)
                break;

            command.Execute();
        }

        Console.WriteLine("Программа завершена.");
    }

    private static Profile CreateProfile()
    {
        Console.Write("Введите имя: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Введите фамилию: ");
        string lastName = Console.ReadLine() ?? "";

        Console.Write("Введите год рождения: ");
        int birthYear;

        while (!int.TryParse(Console.ReadLine(), out birthYear))
        {
            Console.Write("Введите корректный год рождения: ");
        }

        return new Profile(name, lastName, birthYear);
    }
}