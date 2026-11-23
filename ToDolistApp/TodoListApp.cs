using System;
using System.Text;

namespace TodoListApp;

partial class Program
{
    private static TodoList _todoList = new TodoList();
    private static Profile _profile = null!;

    static void Main(string[] args)
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("Работу выполнил Вахрамеев");

        Console.Write("Введите имя: ");
        string firstName = Console.ReadLine() ?? "";

        Console.Write("Введите фамилию: ");
        string lastName = Console.ReadLine() ?? "";

        Console.Write("Введите год рождения: ");
        string? yearInput = Console.ReadLine();
        if (!int.TryParse(yearInput, out int birthYear)){   birthYear = 0;}

        _profile = new Profile(firstName, lastName, birthYear);
        Console.WriteLine($"Добавлен пользователь {_profile.GetInfo()}");
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
                    Console.WriteLine(_profile.GetInfo());
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
  help                        - список команд
  profile                     - данные пользователя
  add <текст>                 - добавить задачу в одну строку
  add --multiline (-m)        - добавить многострочную задачу (до !end)
  view [-i] [-s] [-d] [-a]    - показать таблицу задач (флаги можно объединять: -is)
  read <индекс>               - показать полный текст, статус и дату задачи
  done <индекс>               - отметить задачу выполненной
  delete <индекс>             - удалить задачу по индексу
  update <индекс> <текст>     - обновить текст задачи
  exit                        - выход");
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

        TodoItem newItem = new TodoItem(taskText);
        _todoList.Add(newItem);

        Console.WriteLine("Задача успешно добавлена!");
    }
    
    private static void HandleView(string input)
    {
        bool showAll = HasFlag(input, "--all", 'a');
        bool showIndex = showAll || HasFlag(input, "--index", 'i');
        bool showStatus = showAll || HasFlag(input, "--status", 's');
        bool showDate = showAll || HasFlag(input, "--update-date", 'd');

        _todoList.View(showIndex, showStatus, showDate);
    }
    
    private static void HandleDone(string input)
    {
        if (!TryGetActualIndex(input, out int actualIndex)) return;

        TodoItem? item = _todoList.GetItem(actualIndex);
        if (item != null)
        {
            item.MarkDone(); 
        Console.WriteLine($"Задача №{actualIndex + 1} отмечена выполненной.");
        }
    }

    private static void HandleDelete(string input)
    {
    if (!TryGetActualIndex(input, out int actualIndex)) return;

    _todoList.Delete(actualIndex);
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

        TodoItem? item = _todoList.GetItem(actualIndex);
        if (item != null)
        {
            item.UpdateText(parts[2].Trim('"'));
            Console.WriteLine($"Задача №{actualIndex + 1} успешно обновлена.");
        }
    }
    
    private static void HandleRead(string input)
    {
        if (!TryGetActualIndex(input, out int actualIndex)) return;

        TodoItem? item = _todoList.GetItem(actualIndex);
        if (item != null)
        {
            Console.WriteLine($"Задача №{actualIndex + 1}");
            Console.WriteLine(item.GetFullInfo());
        }
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
        if (actualIndex < 0 || actualIndex >= _todoList.Count)
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
    
}