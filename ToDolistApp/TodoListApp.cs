using System;
using System.Text;
using System.IO;

namespace TodoListApp;

partial class Program
{
    static void Main(string[] args)
    
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("Работу выполнил Вахрамеев");
        string dataDirPath = Path.Combine(AppContext.BaseDirectory, "data");
        string profilePath = Path.Combine(dataDirPath, "profile.txt");
        string todoPath = Path.Combine(dataDirPath, "todo.csv");
        FileManager.EnsureDataDirectory(dataDirPath);
        Profile profile;
        TodoList todoList;
        if (File.Exists(profilePath) && !string.IsNullOrWhiteSpace(File.ReadAllText(profilePath)))
        {
            // Если файл есть — загружаем профиль из него
            profile = FileManager.LoadProfile(profilePath);
            Console.WriteLine($"Загружен пользователь {profile.GetInfo()}");
        }
        else
        {
            Console.Write("Введите имя: ");
            string firstName = Console.ReadLine() ?? "";

            Console.Write("Введите фамилию: ");
            string lastName = Console.ReadLine() ?? "";

            Console.Write("Введите год рождения: ");
            string? yearInput = Console.ReadLine();
            if (!int.TryParse(yearInput, out int birthYear))
            {
                birthYear = 0;
            }

            profile = new Profile(firstName, lastName, birthYear);
            FileManager.SaveProfile(profile, profilePath);
            Console.WriteLine($"Добавлен пользователь {profile.GetInfo()}");
        }
        if (File.Exists(todoPath))
        {
            todoList = FileManager.LoadTodos(todoPath);
            Console.WriteLine($"Загружено задач из файла: {todoList.Count}");
        }
        else
        {
            todoList = new TodoList();
            FileManager.SaveTodos(todoList, todoPath);
        }

        Console.WriteLine("--------------------------------------------------");

        while (true)
        {
            Console.Write("\nВведите команду: ");
            string input = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(input)) continue;

            ICommand command = CommandParser.Parse(input, todoList, profile,todoPath);
            command.Execute();
        }
    }
}