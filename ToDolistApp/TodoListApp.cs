using System;
using System.Text;

namespace TodoListApp;

partial class Program
{
    static void Main(string[] args)
    
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("Работу выполнил Вахрамеев");

        Console.Write("Введите имя: ");
        string firstName = Console.ReadLine() ?? "";

        Console.Write("Введите фамилию: ");
        string lastName = Console.ReadLine() ?? "";

        Console.Write("Введите год рождения: ");
        string? yearInput = Console.ReadLine();
        if (!int.TryParse(yearInput, out int birthYear)){   birthYear = 0;}

        TodoList todoList = new TodoList();
        Profile profile = new Profile(firstName, lastName, birthYear);
        Console.WriteLine($"Добавлен пользователь {profile.GetInfo()}");
        Console.WriteLine("--------------------------------------------------");

        while (true)
        {
            Console.Write("\nВведите команду: ");
            string input = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(input)) continue;

            ICommand command = CommandParser.Parse(input, todoList, profile);
            command.Execute();
        }
    }
}