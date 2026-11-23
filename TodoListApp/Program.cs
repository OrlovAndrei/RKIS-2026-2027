using System.Text;

namespace TodoListApp;

internal class Program
{

    private const int InitialCapacity = 2;
    private const int GrowthFactor = 2;
    private static readonly char[] Separators = { ' ', '\t' };

    private static string?[] todos = new string?[InitialCapacity];
    private static int todoCount;


    private static void Main()
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Учебный проект TodoListApp");

        Console.Write("Имя: ");
        string? firstName = Console.ReadLine()?.Trim();
        Console.Write("Фамилия: ");
        string? lastName = Console.ReadLine()?.Trim();
        Console.Write("Год рождения: ");
        string? yearInput = Console.ReadLine();

        int currentYear = DateTime.Now.Year;
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            Console.WriteLine("Введите имя и фамилию.");
            return;
        }

        if (!int.TryParse(yearInput, out int birthYear) || birthYear < 1 || birthYear > currentYear)
        {
            Console.WriteLine("Введите корректный год рождения.");
            return;
        }

        int age = currentYear - birthYear;
        Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");

        Console.WriteLine("Введите help для списка команд.");
        RunCommands(firstName, lastName, birthYear);
    }

    private static void RunCommands(string firstName, string lastName, int birthYear)
    {
        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();
            if (input == null)
            {
                return;
            }

            var (command, arguments) = GetCommand(input);
            switch (command)
            {
                case "":
                    break;

                case "help":
                    ShowHelp();
                    break;

                case "profile":
                    ShowProfile(firstName, lastName, birthYear);
                    break;

                case "add":
                    AddTask(arguments);
                    break;

                case "view":
                    ViewTasks();
                    break;

                case "exit":
                    Exit();
                    return;

                default:
                    Console.WriteLine("Неизвестная команда. Введите help.");
                    break;
            }
        }
    }

    private static void ShowHelp()
    {
        Console.WriteLine("help — список команд");
        Console.WriteLine("profile — данные пользователя");
        Console.WriteLine("add \"текст задачи\" — добавить задачу");
        Console.WriteLine("view — показать задачи");
        Console.WriteLine("exit — выход");
    }

    private static void ShowProfile(string firstName, string lastName, int birthYear)
    {
        Console.WriteLine($"{firstName} {lastName}, {birthYear}");
    }

    private static void Exit()
    {
        Console.WriteLine("До свидания!");
    }

    private static void AddTask(string arguments)
    {
        string? text = GetTaskText(arguments);
        if (text == null)
        {
            Console.WriteLine("Используйте add \"текст задачи\".");
            return;
        }

        if (todoCount == todos.Length)
        {
            GrowArrays();
        }

        todos[todoCount] = text;
        todoCount++;
        Console.WriteLine("Задача добавлена.");
    }

    private static void ViewTasks()
    {
        if (todoCount == 0)
        {
            Console.WriteLine("Список задач пуст.");
        }

        for (int i = 0; i < todoCount; i++)
        {
            Console.WriteLine(todos[i]);
        }
    }

    private static void GrowArrays()
    {
        var newTodos = new string?[todos.Length * GrowthFactor];
        for (int i = 0; i < todoCount; i++)
        {
            newTodos[i] = todos[i];
        }

        todos = newTodos;
    }


    private static (string Command, string Arguments) GetCommand(string input)
    {
        string[] parts = input.Split(Separators, 2, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
        {
            return ("", "");
        }

        string arguments = parts.Length == 2 ? parts[1].Trim() : "";
        return (parts[0], arguments);
    }

    private static string? GetTaskText(string arguments)
    {
        if (arguments.Length < 2 || arguments[0] != '"' || arguments[^1] != '"')
        {
            return null;
        }

        string text = arguments[1..^1].Trim();
        return text.Length == 0 ? null : text;
    }
}
