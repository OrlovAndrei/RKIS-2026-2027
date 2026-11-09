// Подключаем функции консольного вывода и работу со строками.
using System;

// Подключаем коллекции для хранения флагов.
using System.Collections.Generic;

// Создаём статический класс для разбора введённых команд.
public static class CommandParser
{
    // Названия команд.
    private const string CommandHelp = "help";
    private const string CommandProfile = "profile";
    private const string CommandAdd = "add";
    private const string CommandView = "view";
    private const string CommandStatus = "status";
    private const string CommandDelete = "delete";
    private const string CommandUpdate = "update";
    private const string CommandRead = "read";

    // Определяем команду по строке и создаём объект нужного класса.
    public static ICommand? Parse(string inputString, TodoList todoList, Profile profile)
    {
        // Делим строку на название команды и аргументы.
        string[] parts = inputString.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

        // Если строка пустая, команды нет.
        if (parts.Length == 0)
        {
            return null;
        }

        // Название команды приводим к нижнему регистру.
        string command = parts[0].ToLower();

        // Всё, что после названия команды, считаем аргументами.
        string args = parts.Length > 1 ? parts[1].Trim() : "";

        // Выбираем разбор по названию команды.
        switch (command)
        {
            case CommandHelp:
                return new HelpCommand();

            case CommandProfile:
                return new ProfileCommand { Profile = profile };

            case CommandAdd:
                return ParseAdd(args, todoList);

            case CommandView:
                return ParseView(args, todoList);

            case CommandRead:
                return ParseRead(args, todoList);

            case CommandStatus:
                return ParseStatus(args, todoList);

            case CommandUpdate:
                return ParseUpdate(args, todoList);

            case CommandDelete:
                return ParseDelete(args, todoList);

            default:
                Console.WriteLine("Неизвестная команда. Введите help.");
                return null;
        }
    }

    // Разбираем команду add.
    private static ICommand? ParseAdd(string args, TodoList todoList)
    {
        // Получаем флаги команды.
        HashSet<string> flags = ParseFlags(args);

        // Многострочный режим: текст команда запросит сама.
        if (HasFlag(flags, "multiline", "m"))
        {
            return new AddCommand { TodoList = todoList, Multiline = true };
        }

        // Однострочный режим: текст стоит между кавычками.
        string[] parts = args.Split('"');

        // Проверяем, что кавычки есть и текст не пустой.
        if (parts.Length < 3 || parts[1].Trim() == "")
        {
            Console.WriteLine("Формат: add \"текст задачи\"");
            return null;
        }

        return new AddCommand { TodoList = todoList, Text = parts[1] };
    }

    // Разбираем команду view.
    private static ICommand ParseView(string args, TodoList todoList)
    {
        // Получаем флаги команды.
        HashSet<string> flags = ParseFlags(args);

        // Флаг --all включает все столбцы.
        bool showAll = HasFlag(flags, "all", "a");

        return new ViewCommand
        {
            TodoList = todoList,
            ShowIndex = showAll || HasFlag(flags, "index", "i"),
            ShowDone = showAll || HasFlag(flags, "status", "s"),
            ShowDate = showAll || HasFlag(flags, "update-date", "d")
        };
    }

    // Разбираем команду read.
    private static ICommand? ParseRead(string args, TodoList todoList)
    {
        if (!TryParseIndex(args, out int index))
        {
            return null;
        }

        return new ReadCommand { TodoList = todoList, Index = index };
    }

    // Разбираем команду delete.
    private static ICommand? ParseDelete(string args, TodoList todoList)
    {
        if (!TryParseIndex(args, out int index))
        {
            return null;
        }

        return new DeleteCommand { TodoList = todoList, Index = index };
    }

    // Разбираем команду update.
    private static ICommand? ParseUpdate(string args, TodoList todoList)
    {
        // Для: 1 "текст" получится ["1 ", "текст", ""].
        string[] parts = args.Split('"');

        if (parts.Length < 3 || parts[1].Trim() == "")
        {
            Console.WriteLine("Формат: update <номер> \"новый текст\"");
            return null;
        }

        // Часть до кавычки — это номер задачи.
        if (!TryParseIndex(parts[0], out int index))
        {
            return null;
        }

        return new UpdateCommand { TodoList = todoList, Index = index, Text = parts[1] };
    }

    // Разбираем команду status.
    private static ICommand? ParseStatus(string args, TodoList todoList)
    {
        // Делим аргументы на номер и статус.
        string[] parts = args.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (parts.Length < 2)
        {
            Console.WriteLine("Формат: status <номер> <статус>");
            return null;
        }

        if (!TryParseIndex(parts[0], out int index))
        {
            return null;
        }

        // Преобразуем строку в enum без учёта регистра.
        if (!Enum.TryParse<TodoStatus>(parts[1], true, out TodoStatus status) || !Enum.IsDefined(status))
        {
            Console.WriteLine("Неизвестный статус. Доступны: notstarted, inprogress, completed, postponed, failed.");
            return null;
        }

        return new StatusCommand { TodoList = todoList, Index = index, Status = status };
    }

    // Преобразуем строку в номер задачи.
    private static bool TryParseIndex(string text, out int index)
    {
        if (int.TryParse(text.Trim(), out index))
        {
            return true;
        }

        Console.WriteLine("Ошибка. Укажите номер задачи.");
        return false;
    }

    // Разбираем флаги команды.
    private static HashSet<string> ParseFlags(string args)
    {
        HashSet<string> flags = new HashSet<string>();

        // Берём часть команды до кавычек.
        string flagsPart = args.Split('"')[0];

        foreach (string token in flagsPart.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            if (token.StartsWith("--"))
            {
                // Длинный флаг без двух дефисов.
                flags.Add(token.Substring(2).ToLower());
            }
            else if (token.StartsWith("-"))
            {
                // Каждая буква короткого флага (например, в -is) — отдельный флаг.
                foreach (char symbol in token.Substring(1))
                {
                    flags.Add(char.ToLower(symbol).ToString());
                }
            }
        }

        return flags;
    }

    // Проверяем наличие длинного или короткого флага.
    private static bool HasFlag(HashSet<string> flags, string longName, string shortName)
    {
        return flags.Contains(longName) || flags.Contains(shortName);
    }
}