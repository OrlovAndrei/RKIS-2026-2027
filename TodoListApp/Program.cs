using System;
using System.Collections.Generic;

internal class Program
{
    private const int InitialCapacity = 2;
    private const int GrowthFactor = 2;
    private const int SplitLimit = 2;
    private const int IndexOffset = 1;
    private const int MaxPreviewLength = 30;

    private const string AuthorsMessage = "Работу выполнили Козлов и Голубев";
    private const string StatusDone = "сделано";
    private const string StatusNotDone = "не сделано";
    private const string DateFormat = "dd.MM.yyyy HH:mm";
    private const string Ellipsis = "...";
    private const string MultilineEndMarker = "!end";
    private const string ColumnSeparator = " | ";

    private const string CmdHelp = "help";
    private const string CmdProfile = "profile";
    private const string CmdAdd = "add";
    private const string CmdView = "view";
    private const string CmdRead = "read";
    private const string CmdDone = "done";
    private const string CmdDelete = "delete";
    private const string CmdUpdate = "update";
    private const string CmdExit = "exit";

    private const string FlagMultiline = "--multiline";
    private const string FlagMultilineAlt = "--multi-line";
    private const string FlagMultilineShort = "-m";

    private const string FlagIndex = "--index";
    private const string FlagStatus = "--status";
    private const string FlagUpdateDate = "--update-date";
    private const string FlagAll = "--all";

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
            string? rawInput = Console.ReadLine();

            if (rawInput == null)
            {
                break;
            }

            string input = rawInput.Trim();
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
        userName = (Console.ReadLine() ?? string.Empty).Trim();

        Console.Write("Введите фамилию: ");
        userSurname = (Console.ReadLine() ?? string.Empty).Trim();

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
        if (parts.Length == 0)
        {
            return true;
        }

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
                ViewTasks(arguments);
                break;

            case CmdRead:
                ReadTask(arguments);
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
        Console.WriteLine($"{CmdAdd} \"текст задачи\" - добавить однострочную задачу");
        Console.WriteLine($"{CmdAdd} {FlagMultiline} ({FlagMultilineShort}) - добавить многострочную задачу (завершение: {MultilineEndMarker})");
        Console.WriteLine($"{CmdView} [флаги] - показать таблицу задач (по умолчанию только текст)");
        Console.WriteLine($"  {FlagIndex}, -i - показать индекс");
        Console.WriteLine($"  {FlagStatus}, -s - показать статус");
        Console.WriteLine($"  {FlagUpdateDate}, -d - показать дату последнего изменения");
        Console.WriteLine($"  {FlagAll}, -a - показать все колонки (короткие флаги можно комбинировать, например -is)");
        Console.WriteLine($"{CmdRead} <idx> - показать полный текст, статус и дату задачи");
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
        string normalizedArg = arguments.Trim().ToLower();
        string taskText;

        if (normalizedArg == FlagMultiline ||
            normalizedArg == FlagMultilineAlt ||
            normalizedArg == FlagMultilineShort)
        {
            taskText = ReadMultilineTaskText();
        }
        else
        {
            taskText = ExtractTaskText(arguments);
        }

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

    private static string ReadMultilineTaskText()
    {
        Console.WriteLine($"Многострочный режим. Введите {MultilineEndMarker} на новой строке для завершения:");
        List<string> lines = new List<string>();

        while (true)
        {
            Console.Write("> ");
            string? line = Console.ReadLine();

            if (line == null || line.Trim() == MultilineEndMarker)
            {
                break;
            }

            lines.Add(line);
        }

        return string.Join("\n", lines);
    }

    private static void ViewTasks(string arguments)
    {
        if (!TryParseViewFlags(arguments, out bool showIndex, out bool showStatus, out bool showDate))
        {
            return;
        }

        if (todoCount == 0)
        {
            Console.WriteLine("Список задач пуст.");
            return;
        }

        List<string> headers = new List<string>();
        List<string[]> rows = new List<string[]>();

        if (showIndex) headers.Add("Индекс");
        headers.Add("Задача");
        if (showStatus) headers.Add("Статус");
        if (showDate) headers.Add("Дата изменения");

        for (int i = 0; i < todoCount; i++)
        {
            List<string> cells = new List<string>();

            if (showIndex) cells.Add((i + IndexOffset).ToString());
            cells.Add(FormatTaskPreview(todos[i]));
            if (showStatus) cells.Add(statuses[i] ? StatusDone : StatusNotDone);
            if (showDate) cells.Add(dates[i].ToString(DateFormat));

            rows.Add(cells.ToArray());
        }

        PrintTable(headers.ToArray(), rows);
    }

    private static bool TryParseViewFlags(
        string arguments,
        out bool showIndex,
        out bool showStatus,
        out bool showDate)
    {
        showIndex = false;
        showStatus = false;
        showDate = false;

        if (string.IsNullOrWhiteSpace(arguments))
        {
            return true;
        }

        string[] tokens = arguments.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        foreach (string rawToken in tokens)
        {
            string token = rawToken.ToLower();

            if (token.StartsWith("--"))
            {
                switch (token)
                {
                    case FlagIndex:
                        showIndex = true;
                        break;
                    case FlagStatus:
                        showStatus = true;
                        break;
                    case FlagUpdateDate:
                        showDate = true;
                        break;
                    case FlagAll:
                        showIndex = true;
                        showStatus = true;
                        showDate = true;
                        break;
                    default:
                        Console.WriteLine($"Неизвестный флаг: {rawToken}");
                        return false;
                }
            }
            else if (token.StartsWith("-") && token.Length > 1)
            {
                for (int i = 1; i < token.Length; i++)
                {
                    switch (token[i])
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
                            Console.WriteLine($"Неизвестный флаг: -{token[i]}");
                            return false;
                    }
                }
            }
            else
            {
                Console.WriteLine($"Некорректный параметр: {rawToken}");
                return false;
            }
        }

        return true;
    }

    private static string FormatTaskPreview(string? taskText)
    {
        string singleLine = (taskText ?? string.Empty)
            .Replace("\r\n", " ")
            .Replace("\n", " ");

        if (singleLine.Length > MaxPreviewLength)
        {
            return singleLine.Substring(0, MaxPreviewLength) + Ellipsis;
        }

        return singleLine;
    }

    private static void PrintTable(string[] headers, List<string[]> rows)
    {
        int columnCount = headers.Length;
        int[] widths = new int[columnCount];

        for (int col = 0; col < columnCount; col++)
        {
            widths[col] = headers[col].Length;
            foreach (string[] row in rows)
            {
                if (row[col].Length > widths[col])
                {
                    widths[col] = row[col].Length;
                }
            }
        }

        PrintTableRow(headers, widths);

        string[] separatorCells = new string[columnCount];
        for (int col = 0; col < columnCount; col++)
        {
            separatorCells[col] = new string('-', widths[col]);
        }
        Console.WriteLine(string.Join("-+-", separatorCells));

        foreach (string[] row in rows)
        {
            PrintTableRow(row, widths);
        }
    }

    private static void PrintTableRow(string[] cells, int[] widths)
    {
        string[] padded = new string[cells.Length];
        for (int col = 0; col < cells.Length; col++)
        {
            padded[col] = cells[col].PadRight(widths[col]);
        }

        Console.WriteLine(string.Join(ColumnSeparator, padded));
    }

    private static void ReadTask(string arguments)
    {
        if (!TryParseTaskIndex(arguments, out int index))
        {
            return;
        }

        string statusText = statuses[index] ? StatusDone : StatusNotDone;
        string formattedDate = dates[index].ToString(DateFormat);

        Console.WriteLine($"Задача #{index + IndexOffset}:");
        Console.WriteLine(todos[index] ?? string.Empty);
        Console.WriteLine($"Статус: {statusText}");
        Console.WriteLine($"Последнее изменение: {formattedDate}");
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

    private static bool TryParseTaskIndex(string? rawIndex, out int arrayIndex)
    {
        arrayIndex = -1;

        if (string.IsNullOrWhiteSpace(rawIndex) || !int.TryParse(rawIndex.Trim(), out int userIndex))
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

    private static string ExtractTaskText(string? rawText)
    {
        return (rawText ?? string.Empty).Trim().Trim('"').Trim();
    }
}