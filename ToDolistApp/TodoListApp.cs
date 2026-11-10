using System;

namespace TodoListApp;

partial class Program
{
    private const int InitialCapacity = 2;
    private const int CurrentYear = 2026;

    private static string[] _todos = new string[InitialCapacity];
    private static int _todoCount = 0;

    static void Main(string[] args)
    {
        Console.WriteLine("Работу выполнил Вахрамеев");
        
        Console.Write("Введите имя: ");
        string firstName = Console.ReadLine() ?? "";

        Console.Write("Введите фамилию: ");
        string lastName = Console.ReadLine() ?? "";

        Console.Write("Введите год рождения: ");
        string yearInput = Console.ReadLine() ?? "";

        int birthYear = Convert.ToInt32(string.IsNullOrWhiteSpace(yearInput) ? "0" : yearInput);
        int age = CurrentYear - birthYear;

        Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст {age}");
        Console.WriteLine("--------------------------------------------------");

        while (true)
        {
            Console.Write("\nВведите команду: ");
            string input = Console.ReadLine()?.Trim() ?? "";

            if (string.IsNullOrEmpty(input)) continue;

            string command = input.Split(' ')[0].ToLower();

            switch (command)
            {
                case "help":
                    HandleHelp();
                    break;

                case "profile":
                    Console.WriteLine($"{firstName} {lastName}, {birthYear}");
                    break;

                case "add":
                    HandleAdd(input);
                    break;

                case "view":
                    HandleView();
                    break;

                case "exit":
                    Console.WriteLine("Программа завершена. До свидания!");
                    return;

                default:
                    Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
                    break;
            }
        }
    }


    private static void HandleHelp()
    {
        Console.WriteLine(@"Доступные команды:
  help    - список команд
  profile - данные пользователя
  add     - добавить новую задачу
  view    - показать все задачи
  exit    - выход");
    }

    private static void HandleAdd(string input)
    {
        if (input.Length <= 4)
        {
            Console.WriteLine("Ошибка: введите текст задачи после add.");
            return;
        }

        string taskText = input.Substring(4).Trim('"');

        if (_todoCount >= _todos.Length)
        {
            ResizeArrays();
        }

        _todos[_todoCount++] = taskText;
        Console.WriteLine("Задача успешно добавлена!");
    }

    private static void HandleView()
    {
        if (_todoCount == 0)
        {
            Console.WriteLine("Список задач пуст.");
            return;
        }

        for (int i = 0; i < _todoCount; i++)
        {
            Console.WriteLine($"{i + 1}. {_todos[i]}");
        }
    }

    private static void ResizeArrays()
    {
        string[] newTodos = new string[_todos.Length * 2];
        for (int i = 0; i < _todos.Length; i++)
        {
            newTodos[i] = _todos[i];
        }
        _todos = newTodos;
        Console.WriteLine($"[Система]: Массив увеличен до {_todos.Length} элементов.");
    }
}
