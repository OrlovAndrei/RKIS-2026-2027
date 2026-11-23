using System;
using System.Text;

namespace TodoListApp;

partial class Program
{
    private const int InitialCapacity = 2;
    private const int CurrentYear = 2026;
    private const int MaxTableTextLength = 30;

    private static string[] _todos = new string[InitialCapacity];
    private static bool[] _statuses = new bool[InitialCapacity];
    private static DateTime[] _dates = new DateTime[InitialCapacity];

    private static int _todoCount = 0;

    static void Main(string[] args)
    {
        Console.WriteLine("Работу выполнил Вахрамеев");

        Console.Write("Введите имя: ");
        string firstName = Console.ReadLine() ?? "";

        Console.Write("Введите фамилию: ");
        string lastName = Console.ReadLine() ?? "";

        Console.Write("Введите год рождения: ");
        string? yearInput = Console.ReadLine();
        if (!int.TryParse(yearInput, out int birthYear)){   birthYear = 0;}
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
                    HandleView(input);
                    break;

                case "exit":
                    Console.WriteLine("Программа завершена. До свидания!");
                    return;

                case "done":
                    HandleDone(input);
                    break;

                case "delete":
                    HandleDelete(input);
                    break;

                case "update":
                    HandleUpdate(input);
                    break;
                case "read":
                    HandleRead(input);
                    break;

                default:
                    Console.WriteLine("Неизвестная команда. Введите 'help' для списка команд.");
                    break;

            }
        }
    }


    private static void HandleHelp()
    {
        Console.WriteLine(@"Доступные команды:
  help                    - список команд
  profile                 - данные пользователя
  add <текст>                 - добавить задачу в одну строку
  add --multiline (-m)        - добавить многострочную задачу (до !end)
  view [-i] [-s] [-d] [-a]    - показать таблицу задач (флаги можно объединять: -is)
  done <индекс>           - отметить задачу выполненной
  delete <индекс>         - удалить задачу по индексу
  update <индекс> <текст> - обновить текст задачи
  exit                    - выход");
    }
    
    private static void HandleAdd(string input)
    {
        bool isMultiline = HasFlag(input, "--multiline", 'm');
        string taskText = isMultiline
            ? ReadMultilineText()
            : (input.Length > 4 ? input.Substring(4).Trim('"') : "");

        if (string.IsNullOrWhiteSpace(taskText))
        {
            Console.WriteLine("Ошибка: текст задачи не может быть пустым.");
            return;
        }

        if (_todoCount >= _todos.Length)
        {
            ResizeArrays();
        }

        _todos[_todoCount] = taskText;
        _statuses[_todoCount] = false;
        _dates[_todoCount] = DateTime.Now;
        _todoCount++;

        Console.WriteLine("Задача успешно добавлена!");
    }
    
    private static void HandleView(string input)
    {
        if (_todoCount == 0)
        {
            Console.WriteLine("Список задач пуст.");
            return;
        }

        bool showAll = HasFlag(input, "--all", 'a');
        bool showIndex = showAll || HasFlag(input, "--index", 'i');
        bool showStatus = showAll || HasFlag(input, "--status", 's');
        bool showDate = showAll || HasFlag(input, "--update-date", 'd');

        StringBuilder header = new StringBuilder();
        if (showIndex) header.Append($"| {"Индекс",-6} ");
        header.Append($"| {"Текст задачи",-MaxTableTextLength} ");
        if (showStatus) header.Append($"| {"Статус",-12} ");
        if (showDate) header.Append($"| {"Дата изменения",-19} ");
        header.Append("|");

        string line = new string('-', header.Length);
        Console.WriteLine(line);
        Console.WriteLine(header);
        Console.WriteLine(line);
        for (int i = 0; i < _todoCount; i++)
        {
            StringBuilder row = new StringBuilder();

            if (showIndex) row.Append($"| {i + 1,-6} ");

            string text = (_todos[i] ?? "").Replace('\n', ' ');
            if (text.Length > MaxTableTextLength)
            {
                text = text.Substring(0, MaxTableTextLength - 3) + "...";
            }
            row.Append($"| {text,-MaxTableTextLength} ");

            if (showStatus)
            {
                string status = _statuses[i] ? "выполнена" : "не выполнена";
                row.Append($"| {status,-12} ");
            }

            if (showDate)
            {
                row.Append($"| {_dates[i]:dd.MM.yyyy HH:mm:ss} ");
            }

            row.Append("|");
            Console.WriteLine(row);
        }
        Console.WriteLine(line);
    }
    
    private static void HandleDone(string input)
    {
        if (!TryGetActualIndex(input, out int actualIndex)) return;

        _statuses[actualIndex] = true; 
        _dates[actualIndex] = DateTime.Now; 
        Console.WriteLine($"Задача №{actualIndex + 1} отмечена выполненной.");
    }
    
    private static void HandleDelete(string input)
    {
    if (!TryGetActualIndex(input, out int actualIndex)) return;

    for (int i = actualIndex; i < _todoCount - 1; i++)
    {
        _todos[i] = _todos[i + 1];
        _statuses[i] = _statuses[i + 1];
        _dates[i] = _dates[i + 1];
    }

    _todoCount--;
    _todos[_todoCount] = null!;
    _statuses[_todoCount] = false;
    _dates[_todoCount] = default;

    Console.WriteLine($"Задача №{actualIndex + 1} успешно удалена со сдвигом.");
    }
    
    private static void HandleUpdate(string input)
    {
        if (!TryGetActualIndex(input, out int actualIndex)) return;

        string[] parts = input.Split(' ', 3);
        if (parts.Length < 3 || string.IsNullOrWhiteSpace(parts[2]))
        {
            Console.WriteLine("Ошибка: формат команды: update <idx> \"new_text\"");
           return;
        }

        _todos[actualIndex] = parts[2].Trim('"');
        _dates[actualIndex] = DateTime.Now;

        Console.WriteLine($"Задача №{actualIndex + 1} успешно обновлена.");
    }
    
    private static void HandleRead(string input)
    {
        if (!TryGetActualIndex(input, out int actualIndex)) return;

        string status = _statuses[actualIndex] ? "выполнена" : "не выполнена";
        Console.WriteLine($"Задача №{actualIndex + 1}");
        Console.WriteLine($"Статус: {status}");
        Console.WriteLine($"Дата последнего изменения: {_dates[actualIndex]:dd.MM.yyyy HH:mm:ss}");
        Console.WriteLine($"Полный текст:\n{_todos[actualIndex]}");
    }

     private static bool TryGetActualIndex(string input, out int actualIndex)
    {
        actualIndex = -1;
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 2 || !int.TryParse(parts[1], out int index))
        {
            Console.WriteLine("Ошибка: укажите корректный числовой индекс задачи.");
            return false;
        }

        actualIndex = index - 1;
        if (actualIndex < 0 || actualIndex >= _todoCount)
        {
            Console.WriteLine("Ошибка: задачи с таким индексом не существует.");
            return false;
        }

        return true;
    }    
    
    private static string ReadMultilineText()
    {
        Console.WriteLine("Многострочный режим (введите !end для завершения):");
        StringBuilder sb = new StringBuilder();

        while (true)
        {
            Console.Write("> ");
            string? line = Console.ReadLine();
            if (line == null || line.Trim() == "!end") break;

            if (sb.Length > 0) sb.Append('\n');
            sb.Append(line);
        }
        return sb.ToString();
    }
    
    private static bool HasFlag(string input, string longFlag, char shortFlag)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;

        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 1; i < parts.Length; i++)
        {
            if (parts[i] == longFlag) return true;

            if (parts[i].StartsWith("-") && !parts[i].StartsWith("--"))
            {
                if (parts[i].Contains(shortFlag)) return true;
            }
        }
        return false;
    }
    
    private static void ResizeArrays()
    {
        int newLength = _todos.Length * 2;
        string[] newTodos = new string[newLength];
        bool[] newStatuses = new bool[newLength];
        DateTime[] newDates = new DateTime[newLength];

        for (int i = 0; i < _todos.Length; i++)
        {
            newTodos[i] = _todos[i];
            newStatuses[i] = _statuses[i];
            newDates[i] = _dates[i];
        }

        _todos = newTodos;
        _statuses = newStatuses;
        _dates = newDates;

        Console.WriteLine($"Все массивы расширены до {_todos.Length} элементов.");
    }
}
