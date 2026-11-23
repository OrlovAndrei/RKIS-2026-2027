using System.Text;

namespace TodoListApp;

internal class Program
{

    private const int InitialCapacity = 2;
    private const int GrowthFactor = 2;
    private const string MultilineEnd = "!end";
    private static readonly char[] Separators = { ' ', '\t' };

    private static string?[] todos = new string?[InitialCapacity];
    private static bool[] statuses = new bool[InitialCapacity];
    private static DateTime[] dates = new DateTime[InitialCapacity];
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


                case "done":
                    CompleteTask(arguments);
                    break;

                case "delete":
                    DeleteTask(arguments);
                    break;

                case "update":
                    UpdateTask(arguments);
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
        Console.WriteLine("add --multiline или -m — добавить несколько строк до !end");
        Console.WriteLine("view — показать задачи");
        Console.WriteLine("done <индекс> — отметить задачу выполненной");
        Console.WriteLine("delete <индекс> — удалить задачу");
        Console.WriteLine("update <индекс> \"новый текст\" — изменить задачу");
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
        string? text;
        switch (arguments)
        {
            case "-m":
            case "--multiline":
            case "--multi-line":
                text = ReadMultilineTask();
                break;

            default:
                text = GetTaskText(arguments);
                break;
        }

        if (string.IsNullOrWhiteSpace(text))
        {
            Console.WriteLine("Введите текст в кавычках или используйте add -m.");
            return;
        }

        if (todoCount == todos.Length)
        {
            GrowArrays();
        }

        todos[todoCount] = text;
        statuses[todoCount] = false;
        dates[todoCount] = DateTime.Now;
        todoCount++;
        Console.WriteLine("Задача добавлена.");
    }


    private static string? ReadMultilineTask()
    {
        Console.WriteLine("Введите текст задачи. Для завершения введите !end.");
        var text = new StringBuilder();
        bool firstLine = true;

        while (true)
        {
            Console.Write("> ");
            string? line = Console.ReadLine();
            if (line == null)
            {
                return null;
            }

            if (line == MultilineEnd)
            {
                return text.ToString();
            }

            if (!firstLine)
            {
                text.Append('\n');
            }

            text.Append(line);
            firstLine = false;
        }
    }

    private static void ViewTasks()
    {
        if (todoCount == 0)
        {
            Console.WriteLine("Список задач пуст.");
        }

        for (int i = 0; i < todoCount; i++)
        {
            Console.WriteLine($"{i} {todos[i]} {GetStatusText(i)} {dates[i]:dd.MM.yyyy HH:mm:ss}");
        }
    }

    private static void GrowArrays()
    {
        int capacity = todos.Length * GrowthFactor;
        var newTodos = new string?[capacity];
        var newStatuses = new bool[capacity];
        var newDates = new DateTime[capacity];
        for (int i = 0; i < todoCount; i++)
        {
            newTodos[i] = todos[i];
            newStatuses[i] = statuses[i];
            newDates[i] = dates[i];
        }

        todos = newTodos;
        statuses = newStatuses;
        dates = newDates;
    }


    private static string GetStatusText(int index)
    {
        return statuses[index] ? "сделано" : "не сделано";
    }

    private static bool TryGetIndex(string argument, out int index)
    {
        if (int.TryParse(argument, out index) && index >= 0 && index < todoCount)
        {
            return true;
        }

        Console.WriteLine("Нет задачи с таким индексом.");
        return false;
    }

    private static void CompleteTask(string arguments)
    {
        if (!TryGetIndex(arguments, out int index))
        {
            return;
        }

        statuses[index] = true;
        dates[index] = DateTime.Now;
        Console.WriteLine("Задача выполнена.");
    }

    private static void DeleteTask(string arguments)
    {
        if (!TryGetIndex(arguments, out int index))
        {
            return;
        }

        for (int i = index; i < todoCount - 1; i++)
        {
            todos[i] = todos[i + 1];
            statuses[i] = statuses[i + 1];
            dates[i] = dates[i + 1];
        }

        todoCount--;
        todos[todoCount] = null;
        statuses[todoCount] = false;
        dates[todoCount] = default;
        Console.WriteLine("Задача удалена.");
    }

    private static void UpdateTask(string arguments)
    {
        var (indexArgument, textArgument) = GetCommand(arguments);
        string? text = GetTaskText(textArgument);
        if (text == null)
        {
            Console.WriteLine("Используйте update <индекс> \"новый текст\".");
            return;
        }

        if (!TryGetIndex(indexArgument, out int index))
        {
            return;
        }

        todos[index] = text;
        dates[index] = DateTime.Now;
        Console.WriteLine("Задача изменена.");
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
