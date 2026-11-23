using System.Globalization;
using System.Text;

namespace TodoListApp;

internal class Program
{

    private const int InitialCapacity = 2;
    private const int GrowthFactor = 2;
    private const string MultilineEnd = "!end";
    private const int PreviewLength = 30;
    private const string Ellipsis = "...";
    private const int TextWidth = PreviewLength + 3;
    private const int IndexWidth = 6;
    private const int StatusWidth = 12;
    private const string DateFormat = "dd.MM.yyyy HH:mm:ss";
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
                    ViewTasks(arguments);
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

                case "read":
                    ReadTask(arguments);
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
        Console.WriteLine("view [флаги] — показать таблицу задач");
        Console.WriteLine("  -i, --index         индекс");
        Console.WriteLine("  -s, --status        статус");
        Console.WriteLine("  -d, --update-date   дата изменения");
        Console.WriteLine("  -a, --all           все колонки");
        Console.WriteLine("  Флаги можно объединять: view -is");
        Console.WriteLine("done <индекс> — отметить задачу выполненной");
        Console.WriteLine("delete <индекс> — удалить задачу");
        Console.WriteLine("update <индекс> \"новый текст\" — изменить задачу");
        Console.WriteLine("read <индекс> — показать полную задачу");
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

    private static void ViewTasks(string arguments)
    {
        if (!TryGetViewFlags(arguments, out bool showIndex, out bool showStatus, out bool showDate))
        {
            Console.WriteLine("Неизвестный флаг команды view. Введите help.");
            return;
        }

        if (todoCount == 0)
        {
            Console.WriteLine("Список задач пуст.");
            return;
        }

        PrintTaskRow("Задача", showIndex ? "Индекс" : null,
            showStatus ? "Статус" : null, showDate ? "Изменена" : null);

        for (int i = 0; i < todoCount; i++)
        {
            PrintTaskRow(GetPreview(todos[i] ?? ""), showIndex ? i.ToString() : null,
                showStatus ? GetStatusText(i) : null,
                showDate ? dates[i].ToString(DateFormat) : null);
        }
    }

    private static bool TryGetViewFlags(string arguments, out bool showIndex,
        out bool showStatus, out bool showDate)
    {
        showIndex = false;
        showStatus = false;
        showDate = false;

        foreach (string flag in arguments.Split(Separators, StringSplitOptions.RemoveEmptyEntries))
        {
            string flags = flag switch
            {
                "--index" => "i",
                "--status" => "s",
                "--update-date" => "d",
                "--all" => "a",
                _ when flag.StartsWith('-') && !flag.StartsWith("--") => flag[1..],
                _ => ""
            };

            if (flags.Length == 0)
            {
                return false;
            }

            foreach (char letter in flags)
            {
                switch (letter)
                {
                    case 'i':
                        showIndex = true;
                        break;

                    case 's':
                        showStatus = true;
                        break;

                    case 'd':
                        showDate = true;
                        break;

                    case 'a':
                        showIndex = true;
                        showStatus = true;
                        showDate = true;
                        break;

                    default:
                        return false;
                }
            }
        }

        return true;
    }

    private static string GetPreview(string text)
    {
        string singleLine = text.Replace('\n', ' ').Replace('\t', ' ');
        int[] positions = StringInfo.ParseCombiningCharacters(singleLine);
        if (positions.Length > PreviewLength)
        {
            return singleLine[..positions[PreviewLength]] + Ellipsis;
        }

        return singleLine;
    }

    private static void PrintTaskRow(string text, string? index, string? status, string? date)
    {
        if (index != null)
        {
            Console.Write($"{index,IndexWidth} | ");
        }

        Console.Write(text);
        if (status != null || date != null)
        {
            int spaces = TextWidth - new StringInfo(text).LengthInTextElements;
            Console.Write(new string(' ', spaces));
        }

        if (status != null)
        {
            Console.Write($" | {status,-StatusWidth}");
        }

        if (date != null)
        {
            Console.Write($" | {date}");
        }

        Console.WriteLine();
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

    private static void ReadTask(string arguments)
    {
        if (!TryGetIndex(arguments, out int index))
        {
            return;
        }

        Console.WriteLine(todos[index]);
        Console.WriteLine($"Статус: {GetStatusText(index)}");
        Console.WriteLine($"Изменена: {dates[index].ToString(DateFormat)}");
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
