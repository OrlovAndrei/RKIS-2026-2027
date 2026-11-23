using System;

internal class Program
{
    private const int InitialCapacity = 2;
    private const int GrowthFactor = 2;
    private const int SplitLimit = 2;
    private const int IndexOffset = 1;

    private const string AuthorsMessage = "Работу выполнили Козлов и Голубев";
    private const string StatusDone = "сделано";
    private const string StatusNotDone = "не сделано";
    private const string DateFormat = "dd.MM.yyyy HH:mm";

    private const string CmdHelp = "help";
    private const string CmdProfile = "profile";
    private const string CmdAdd = "add";
    private const string CmdView = "view";
    private const string CmdDone = "done";
    private const string CmdDelete = "delete";
    private const string CmdUpdate = "update";
    private const string CmdExit = "exit";

    private static string userName = string.Empty;
    private static string userSurname = string.Empty;
    private static int userBirthYear;

    private static string[] todos = new string[InitialCapacity];
    private static bool[] statuses = new bool[InitialCapacity];
    private static DateTime[] dates = new DateTime[InitialCapacity];
    private static int todoCount = 0;

    private static void Main()
    {
        Console.WriteLine(AuthorsMessage);
        RegisterUser();
        Console.WriteLine($"Введите {CmdHelp}, чтобы увидеть список команд.");

        bool isRunning = true;

        while (isRunning)
        {
            Console.Write("Введите команду: ");
            string input = (Console.ReadLine() ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                continue;
            }

            isRunning = ProcessCommand(input);
        }
    }

    private static void RegisterUser()
    {
        Console.Write("Введите имя: ");
        userName = Console.ReadLine() ?? string.Empty;

        Console.Write("Введите фамилию: ");
        userSurname = Console.ReadLine() ?? string.Empty;

        Console.Write("Введите год рождения: ");
        while (!int.TryParse(Console.ReadLine(), out userBirthYear))
        {
            Console.Write("Некорректный год. Введите целое число: ");
        }

        int age = DateTime.Now.Year - userBirthYear;
        Console.WriteLine($"Добавлен пользователь {userName} {userSurname}, возраст - {age}");
    }

    private static bool ProcessCommand(string input)
    {
        string[] parts = input.Split(' ', SplitLimit, StringSplitOptions.RemoveEmptyEntries);
        string action = parts[0].ToLower();
        string arguments = parts.Length > 1 ? parts[1].Trim() : string.Empty;

        switch (action)
        {
            case CmdHelp:
                ShowHelp();
                break;

            case CmdProfile:
                ShowProfile();
                break;

            case CmdAdd:
                AddTask(arguments);
                break;

            case CmdView:
                ViewTasks();
                break;

            case CmdDone:
                MarkTaskDone(arguments);
                break;

            case CmdDelete:
                DeleteTask(arguments);
                break;

            case CmdUpdate:
                UpdateTask(arguments);
                break;

            case CmdExit:
                Console.WriteLine("Программа завершена.");
                return false;

            default:
                Console.WriteLine($"Неизвестная команда. Введите {CmdHelp}.");
                break;
        }

        return true;
    }

    private static void ShowHelp()
    {
        Console.WriteLine("Доступные команды:");
        Console.WriteLine($"{CmdHelp} - список команд");
        Console.WriteLine($"{CmdProfile} - данные пользователя");
        Console.WriteLine($"{CmdAdd} \"текст задачи\" - добавить задачу");
        Console.WriteLine($"{CmdView} - показать задачи");
        Console.WriteLine($"{CmdDone} <idx> - отметить задачу выполненной");
        Console.WriteLine($"{CmdDelete} <idx> - удалить задачу");
        Console.WriteLine($"{CmdUpdate} <idx> \"новый текст\" - обновить текст задачи");
        Console.WriteLine($"{CmdExit} - выход");
    }

    private static void ShowProfile()
    {
        Console.WriteLine($"{userName} {userSurname}, {userBirthYear}");
    }

    private static void AddTask(string arguments)
    {
        string taskText = ExtractTaskText(arguments);

        if (string.IsNullOrWhiteSpace(taskText))
        {
            Console.WriteLine("Введите текст задачи.");
            return;
        }

        ExpandArraysIfNeeded();

        todos[todoCount] = taskText;
        statuses[todoCount] = false;
        dates[todoCount] = DateTime.Now;
        todoCount++;

        Console.WriteLine("Задача добавлена.");
    }

    private static void ViewTasks()
    {
        if (todoCount == 0)
        {
            Console.WriteLine("Список задач пуст.");
            return;
        }

        for (int i = 0; i < todoCount; i++)
        {
            string statusText = statuses[i] ? StatusDone : StatusNotDone;
            string formattedDate = dates[i].ToString(DateFormat);
            Console.WriteLine($"{i + IndexOffset} {todos[i]} {statusText} {formattedDate}");
        }
    }

    private static void MarkTaskDone(string arguments)
    {
        if (!TryParseTaskIndex(arguments, out int index))
        {
            return;
        }

        statuses[index] = true;
        dates[index] = DateTime.Now;

        Console.WriteLine($"Задача {index + IndexOffset} отмечена как выполненная.");
    }

    private static void DeleteTask(string arguments)
    {
        if (!TryParseTaskIndex(arguments, out int index))
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
        todos[todoCount] = string.Empty;
        statuses[todoCount] = false;
        dates[todoCount] = default;

        Console.WriteLine($"Задача {index + IndexOffset} удалена.");
    }

    private static void UpdateTask(string arguments)
    {
        string[] parts = arguments.Split(' ', SplitLimit, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < SplitLimit)
        {
            Console.WriteLine($"Неверный формат. Используйте: {CmdUpdate} <idx> \"новый текст\"");
            return;
        }

        if (!TryParseTaskIndex(parts[0], out int index))
        {
            return;
        }

        string newText = ExtractTaskText(parts[1]);

        if (string.IsNullOrWhiteSpace(newText))
        {
            Console.WriteLine("Новый текст задачи не может быть пустым.");
            return;
        }

        todos[index] = newText;
        dates[index] = DateTime.Now;

        Console.WriteLine($"Задача {index + IndexOffset} обновлена.");
    }

    private static void ExpandArraysIfNeeded()
    {
        if (todoCount < todos.Length)
        {
            return;
        }

        int newCapacity = todos.Length * GrowthFactor;
        string[] newTodos = new string[newCapacity];
        bool[] newStatuses = new bool[newCapacity];
        DateTime[] newDates = new DateTime[newCapacity];

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

    private static bool TryParseTaskIndex(string rawIndex, out int arrayIndex)
    {
        arrayIndex = -1;

        if (!int.TryParse(rawIndex, out int userIndex))
        {
            Console.WriteLine("Укажите корректный номер задачи.");
            return false;
        }

        int targetIndex = userIndex - IndexOffset;

        if (targetIndex < 0 || targetIndex >= todoCount)
        {
            Console.WriteLine("Задачи с таким номером не существует.");
            return false;
        }

        arrayIndex = targetIndex;
        return true;
    }

    private static string ExtractTaskText(string rawText)
    {
        return rawText.Trim().Trim('"').Trim();
    }
}