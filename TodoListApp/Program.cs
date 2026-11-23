using System;

namespace TodoListApp;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Работу выполнили Илиадис и Дедкова");

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

        Profile profile = new Profile(name, lastName, birthYear);
        TodoList todoList = new TodoList();

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
}