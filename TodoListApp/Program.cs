using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TodoListApp;

public class Program
{
    private static readonly List<TodoItem> _tasks = new List<TodoItem>();

    public static void Main(string[] args)
    {
        // Ваша регистрация
        Console.WriteLine("Работу выполнили Илиадис и Дедкова");
        Console.Write("Введите имя: ");
        string? name = Console.ReadLine();
        Console.Write("Введите фамилию: ");
        string? lastName = Console.ReadLine();
        Console.Write("Введите год рождения: ");
        string? year = Console.ReadLine();

        Console.WriteLine("\nДобро пожаловать в TodoList!");
        Console.WriteLine("Введите help для списка команд.");

        // Основной цикл программы
        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) continue;
            if (input.ToLower() == "exit" || input.ToLower() == "quit") break;

            // Разбиваем ввод на аргументы (с поддержкой кавычек)
            var commandArgs = ParseCommandLine(input);
            if (commandArgs.Count == 0) continue;

            string command = commandArgs[0].ToLower();
            
            // Отделяем флаги от текста
            var flags = ParseFlags(commandArgs.Skip(1).ToArray());
            var positionalArgs = commandArgs.Skip(1).Where(a => !a.StartsWith("-")).ToArray();

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
                case "help": 
                    PrintHelp(); 
                    break;
                default: 
                    Console.WriteLine("Неизвестная команда. Введите help для списка команд."); 
                    break;
            }
        }
    }

    // Разбивает строку на слова, учитывая кавычки (например: add "Купить хлеб")
    private static List<string> ParseCommandLine(string input)
    {
        var result = new List<string>();
        bool inQuotes = false;
        string current = "";
        foreach (char c in input)
        {
            if (c == '"') { inQuotes = !inQuotes; continue; }
            if (c == ' ' && !inQuotes)
            {
                if (current != "") { result.Add(current); current = ""; }
            }
            else { current += c; }
        }
        if (current != "") result.Add(current);
        return result;
    }

    // Парсинг флагов (-m, --multiline, -i, -s и т.д.)
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

    private static void HandleAdd(Dictionary<string, bool> flags, string[] args)
    {
        bool isMulti = flags.ContainsKey("multiline");

        if (isMulti)
        {
            Console.WriteLine("Введите строки (для завершения введите '!end'):");
            var lines = new List<string>();
            string? line;
            while ((line = Console.ReadLine()) != null)
            {
                if (line.Trim().Equals("!end", StringComparison.OrdinalIgnoreCase)) break;
                lines.Add(line);
            }

            if (lines.Count > 0)
            {
                _tasks.Add(new TodoItem { Text = string.Join("\n", lines) });
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
                _tasks.Add(new TodoItem { Text = text.Trim() });
                Console.WriteLine("Задача добавлена.");
            }
        }
    }

    private static void HandleView(Dictionary<string, bool> flags)
    {
        if (_tasks.Count == 0) { Console.WriteLine("Список пуст."); return; }

        bool showIndex = flags.ContainsKey("index");
        bool showStatus = flags.ContainsKey("status");
        bool showDate = flags.ContainsKey("update-date");
        bool showAll = flags.ContainsKey("all");

        if (showAll) { showIndex = true; showStatus = true; showDate = true; }

        var headers = new List<string>();
        if (showIndex) headers.Add("ID");
        headers.Add("Текст задачи");
        if (showStatus) headers.Add("Статус");
        if (showDate) headers.Add("Дата изм.");

        var rows = new List<string[]>();
        for (int i = 0; i < _tasks.Count; i++)
        {
            var task = _tasks[i];
            var row = new List<string>();
            if (showIndex) row.Add((i + 1).ToString());

            // Обрезка до 30 символов
            string displayText = task.Text.Length > 30 ? task.Text.Substring(0, 30) + "..." : task.Text;
            displayText = displayText.Replace("\n", " ").Replace("\r", "");
            row.Add(displayText);

            if (showStatus) row.Add(task.IsCompleted ? "Выполнена" : "Не выполнена");
            if (showDate) row.Add(task.LastModified.ToString("dd.MM.yy HH:mm"));
            rows.Add(row.ToArray());
        }
        PrintTable(headers, rows);
    }

    private static void HandleRead(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Укажите ID: read <idx>");
            return;
        }

        if (!int.TryParse(args[0], out int index) || index < 1 || index > _tasks.Count)
        {
            Console.WriteLine($"Ошибка: укажите число от 1 до {_tasks.Count}.");
            return;
        }

        var task = _tasks[index - 1];
        Console.WriteLine("==================================");
        Console.WriteLine($"Задача #{index}:");
        Console.WriteLine("----------------------------------");
        Console.WriteLine(task.Text);
        Console.WriteLine("----------------------------------");
        Console.WriteLine($"Статус: {(task.IsCompleted ? "Выполнена" : "Не выполнена")}");
        Console.WriteLine($"Изменено: {task.LastModified:dd.MM.yyyy HH:mm:ss}");
        Console.WriteLine("==================================");
    }

    private static void PrintTable(List<string> headers, List<string[]> rows)
    {
        int[] widths = new int[headers.Count];
        for (int i = 0; i < headers.Count; i++)
        {
            widths[i] = headers[i].Length;
            foreach (var row in rows)
                if (i < row.Length && row[i].Length > widths[i]) widths[i] = row[i].Length;
        }

        PrintRow(headers.ToArray(), widths);
        Console.WriteLine(string.Concat(widths.Select(w => new string('-', w + 2))));
        foreach (var row in rows) PrintRow(row, widths);
    }

    private static void PrintRow(string[] cells, int[] widths)
    {
        for (int i = 0; i < cells.Length; i++)
            Console.Write(cells[i].PadRight(widths[i] + 2));
        Console.WriteLine();
    }

    private static void PrintHelp()
    {
        Console.WriteLine("Доступные команды:");
        Console.WriteLine("  add [текст]             - добавить задачу");
        Console.WriteLine("  add --multiline (-m)    - добавить многострочную задачу (ввод до !end)");
        Console.WriteLine("  view                    - показать задачи");
        Console.WriteLine("  view --index (-i)       - показать ID");
        Console.WriteLine("  view --status (-s)      - показать статус");
        Console.WriteLine("  view --update-date (-d) - показать дату изменения");
        Console.WriteLine("  view --all (-a)         - показать все столбцы");
        Console.WriteLine("  read <idx>              - полный текст задачи по ID");
        Console.WriteLine("  help                    - эта справка");
        Console.WriteLine("  exit                    - выход");
    }
}