using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoListApp;

public class Program
{
    private static readonly TodoList _tasks = new TodoList();
    private static Profile? _profile;

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

_profile = new Profile(name, lastName, birthYear);

        Console.WriteLine("\nДобро пожаловать в TodoList!");
        Console.WriteLine("Введите help для списка команд.");

        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                continue;

            if (input.Equals("exit", StringComparison.OrdinalIgnoreCase) ||
                input.Equals("quit", StringComparison.OrdinalIgnoreCase))
                break;

            List<string> commandArgs = ParseCommandLine(input);

            if (commandArgs.Count == 0)
                continue;

            string command = commandArgs[0].ToLower();

            var flags = ParseFlags(commandArgs.Skip(1).ToArray());

            string[] positionalArgs = commandArgs
                .Skip(1)
                .Where(a => !a.StartsWith("-"))
                .ToArray();

            switch (command)
            {
                case "add":
                    HandleAdd(flags, positionalArgs);
                    break;

                case "view":
                    HandleView(flags);
                    break;

                case "read":
                    HandleRead(positionalArgs);
                    break;

                case "done":
                    HandleDone(positionalArgs);
                    break;

                case "update":
                    HandleUpdate(positionalArgs);
                    break;

                case "delete":
                    HandleDelete(positionalArgs);
                    break;

                case "profile":
                    HandleProfile();
                    break;

                case "help":
                    PrintHelp();
                    break;

                default:
                    Console.WriteLine("Неизвестная команда. Введите help.");
                    break;
            }
        }

        Console.WriteLine("Программа завершена.");
    }

    private static List<string> ParseCommandLine(string input)
    {
        var result = new List<string>();
        bool inQuotes = false;
        string current = "";

        foreach (char c in input)
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
                continue;
            }

            if (c == ' ' && !inQuotes)
            {
                if (current != "")
                {
                    result.Add(current);
                    current = "";
                }
            }
            else
            {
                current += c;
            }
        }

        if (current != "")
            result.Add(current);

        return result;
    }

    private static Dictionary<string, bool> ParseFlags(string[] args)
    {
        var flags = new Dictionary<string, bool>();

        foreach (var arg in args)
        {
            if (arg.StartsWith("--"))
            {
                flags[arg.Substring(2).ToLower()] = true;
            }
            else if (arg.StartsWith("-") && arg.Length > 1)
            {
                string shortFlags = arg.Substring(1).ToLower();

                foreach (char c in shortFlags)
                {
                    string flag = c switch
                    {
                        'm' => "multiline",
                        'i' => "index",
                        's' => "status",
                        'd' => "update-date",
                        'a' => "all",
                        _ => c.ToString()
                    };

                    flags[flag] = true;
                }
            }
        }

        return flags;
    }

    private static void HandleAdd(
        Dictionary<string, bool> flags,
        string[] args)
    {
        bool isMulti = flags.ContainsKey("multiline");

        if (isMulti)
        {
            Console.WriteLine("Введите строки (для завершения введите '!end'):");

            var lines = new List<string>();
            string? line;

            while ((line = Console.ReadLine()) != null)
            {
                if (line.Trim().Equals("!end", StringComparison.OrdinalIgnoreCase))
                    break;

                lines.Add(line);
            }

            if (lines.Count > 0)
            {
                _tasks.Add(new TodoItem(string.Join("\n", lines)));
                Console.WriteLine("Многострочная задача добавлена.");
            }
        }
        else
        {
            string text = string.Join(" ", args);

            if (string.IsNullOrWhiteSpace(text))
            {
                Console.Write("Введите текст задачи: ");
                text = Console.ReadLine() ?? "";
            }

            if (!string.IsNullOrWhiteSpace(text))
            {
                _tasks.Add(new TodoItem(text.Trim()));
                Console.WriteLine("Задача добавлена.");
            }
        }
    }

    private static void HandleView(Dictionary<string, bool> flags)
    {
        bool showIndex = flags.ContainsKey("index");
        bool showStatus = flags.ContainsKey("status");
        bool showDate = flags.ContainsKey("update-date");
        bool showAll = flags.ContainsKey("all");

        if (showAll)
        {
            showIndex = true;
            showStatus = true;
            showDate = true;
        }

        _tasks.View(showIndex, showStatus, showDate);
    }

    private static void HandleRead(string[] args)
    {
        if (!TryGetTask(args, out TodoItem? task, out int index))
            return;

        Console.WriteLine("==================================");
        Console.WriteLine($"Задача #{index}:");
        Console.WriteLine("----------------------------------");
        Console.WriteLine(task!.GetFullInfo());
        Console.WriteLine("==================================");
    }

    private static void HandleDone(string[] args)
    {
        if (!TryGetTask(args, out TodoItem? task, out int index))
            return;

        task!.MarkDone();
        Console.WriteLine($"Задача #{index} отмечена как выполненная.");
    }

    private static void HandleUpdate(string[] args)
    {
        if (!TryGetTask(args, out TodoItem? task, out int index))
            return;

        string newText = string.Join(" ", args.Skip(1));

        if (string.IsNullOrWhiteSpace(newText))
        {
            Console.Write("Введите новый текст задачи: ");
            newText = Console.ReadLine() ?? "";
        }

        if (string.IsNullOrWhiteSpace(newText))
        {
            Console.WriteLine("Текст задачи не может быть пустым.");
            return;
        }

        task!.UpdateText(newText.Trim());
        Console.WriteLine($"Задача #{index} обновлена.");
    }

    private static void HandleDelete(string[] args)
    {
        if (!TryGetTask(args, out TodoItem? task, out int index))
            return;

        _tasks.Delete(index - 1);
        Console.WriteLine($"Задача #{index} удалена.");
    }

    private static bool TryGetTask(
        string[] args,
        out TodoItem? task,
        out int index)
    {
        task = null;
        index = 0;

        if (args.Length == 0 ||
            !int.TryParse(args[0], out index) ||
            index < 1)
        {
            Console.WriteLine("Укажите корректный номер задачи.");
            return false;
        }

        task = _tasks.GetItem(index - 1);

        if (task == null)
        {
            Console.WriteLine("Задачи с таким номером нет.");
            return false;
        }

        return true;
    }

    private static void HandleProfile()
    {
        Console.WriteLine(_profile!.GetInfo());
    }

    private static void PrintHelp()
    {
        Console.WriteLine("Доступные команды:");
        Console.WriteLine("  add [текст]             - добавить задачу");
        Console.WriteLine("  add --multiline (-m)    - добавить многострочную задачу");
        Console.WriteLine("  view                    - показать задачи");
        Console.WriteLine("  view --index (-i)       - показать номера задач");
        Console.WriteLine("  view --status (-s)      - показать статус");
        Console.WriteLine("  view --update-date (-d) - показать дату изменения");
        Console.WriteLine("  view --all (-a)         - показать все данные");
        Console.WriteLine("  read <idx>              - прочитать задачу полностью");
        Console.WriteLine("  done <idx>              - отметить задачу выполненной");
        Console.WriteLine("  update <idx> \"текст\"    - изменить текст задачи");
        Console.WriteLine("  delete <idx>            - удалить задачу");
        Console.WriteLine("  profile                 - показать профиль");
        Console.WriteLine("  help                    - список команд");
        Console.WriteLine("  exit                    - выход");
    }
}