using System;
using System.Text;

class Program
{
    static TodoList todoList = new TodoList();
    static Profile? profile;

    static void Main()
    {
        PrintHeader();
        RegisterUser();

        Console.WriteLine("\nВведите команду (для справки введите help):");

        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                continue;
            }

            input = input.Trim();

            if (input == "help")
            {
                PrintHelp();
            }
            else if (input == "profile")
            {
                PrintProfile();
            }
            else if (input.StartsWith("add"))
            {
                AddTodo(input);
            }
            else if (input == "view" || input.StartsWith("view "))
            {
                ViewTodos(input);
            }
            else if (input.StartsWith("done"))
            {
                DoneTodo(input);
            }
            else if (input.StartsWith("delete"))
            {
                DeleteTodo(input);
            }
            else if (input.StartsWith("update"))
            {
                UpdateTodo(input);
            }
            else if (input.StartsWith("read"))
            {
                ReadTodo(input);
            }
            else if (input == "exit")
            {
                Console.WriteLine("Выход из программы.");
                break;
            }
            else
            {
                Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
            }
        }
    }

    static void PrintHeader()
    {
        Console.WriteLine("========================================");
        Console.WriteLine("Работу выполнили: Чичикин и Мартемьянов");
        Console.WriteLine("Группа: 3831");
        Console.WriteLine("========================================\n");
    }

    static void RegisterUser()
    {
        Console.Write("Введите имя: ");
        string firstName = Console.ReadLine() ?? "";
        Console.Write("Введите фамилию: ");
        string lastName = Console.ReadLine() ?? "";

        int birthYear;
        while (true)
        {
            Console.Write("Введите год рождения: ");
            string? yearInput = Console.ReadLine();
            if (int.TryParse(yearInput, out birthYear) && birthYear > 1900 && birthYear <= DateTime.Now.Year)
            {
                break;
            }
            Console.WriteLine("Некорректный год рождения, попробуйте снова.");
        }

        profile = new Profile(firstName, lastName, birthYear);
        Console.WriteLine($"\nДобавлен пользователь {profile.GetInfo()}");
    }

    static void PrintHelp()
    {
        Console.WriteLine("Список команд:");
        Console.WriteLine("  help - показать список доступных команд");
        Console.WriteLine("  profile - вывести данные пользователя");
        Console.WriteLine("  add \"текст\" - добавить новую задачу");
        Console.WriteLine("  add --multiline (-m) - добавить многострочную задачу, ввод завершается строкой !end");
        Console.WriteLine("  view [флаги] - показать задачи в виде таблицы");
        Console.WriteLine("      --index (-i) - показывать номер задачи");
        Console.WriteLine("      --status (-s) - показывать статус задачи");
        Console.WriteLine("      --update-date (-d) - показывать дату изменения");
        Console.WriteLine("      --all (-a) - показывать все данные");
        Console.WriteLine("      короткие флаги можно объединять, например: view -is");
        Console.WriteLine("  done <номер> - отметить задачу выполненной");
        Console.WriteLine("  delete <номер> - удалить задачу");
        Console.WriteLine("  update <номер> \"текст\" - изменить задачу");
        Console.WriteLine("  read <номер> - показать полный текст задачи, статус и дату изменения");
        Console.WriteLine("  exit - завершить работу программы");
    }

    static void PrintProfile()
    {
        if (profile == null)
        {
            Console.WriteLine("Пользователь не зарегистрирован.");
            return;
        }
        Console.WriteLine(profile.GetInfo());
    }

    static TodoItem? FindItem(string text, out int index)
    {
        index = -1;
        if (!int.TryParse(text, out int number))
        {
            return null;
        }

        index = number - 1;
        return todoList.GetItem(index);
    }

    static void AddTodo(string input)
    {
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        string? task;

        if (parts.Length == 2 && (parts[1] == "--multiline" || parts[1] == "-m"))
        {
            task = ReadMultilineText();
        }
        else
        {
            task = ExtractQuotedText(input);
        }

        if (string.IsNullOrWhiteSpace(task))
        {
            Console.WriteLine("Неверный формат. Используйте: add \"текст задачи\" или add --multiline");
            return;
        }

        todoList.Add(new TodoItem(task));
        Console.WriteLine("Задача успешно добавлена!");
    }

    static string ReadMultilineText()
    {
        Console.WriteLine("Введите текст задачи построчно, для завершения введите !end");
        StringBuilder sb = new StringBuilder();

        while (true)
        {
            Console.Write("> ");
            string? line = Console.ReadLine();

            if (line == null || line.Trim() == "!end")
            {
                break;
            }

            if (sb.Length > 0)
            {
                sb.Append('\n');
            }
            sb.Append(line);
        }

        return sb.ToString();
    }

    static string? ExtractQuotedText(string input)
    {
        string[] parts = input.Split('"');
        if (parts.Length >= 2 && !string.IsNullOrWhiteSpace(parts[1]))
        {
            return parts[1];
        }
        return null;
    }

    static void ViewTodos(string input)
    {
        if (!ParseViewFlags(input, out bool showIndex, out bool showStatus, out bool showDate))
        {
            return;
        }

        todoList.View(showIndex, showStatus, showDate);
    }

    static bool ParseViewFlags(string input, out bool showIndex, out bool showStatus, out bool showDate)
    {
        showIndex = false;
        showStatus = false;
        showDate = false;

        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 1; i < parts.Length; i++)
        {
            string flag = parts[i];

            if (flag == "--all")
            {
                showIndex = true;
                showStatus = true;
                showDate = true;
            }
            else if (flag == "--index")
            {
                showIndex = true;
            }
            else if (flag == "--status")
            {
                showStatus = true;
            }
            else if (flag == "--update-date")
            {
                showDate = true;
            }
            else if (flag.StartsWith("-") && !flag.StartsWith("--") && flag.Length > 1)
            {
                for (int j = 1; j < flag.Length; j++)
                {
                    char c = flag[j];
                    if (c == 'i')
                    {
                        showIndex = true;
                    }
                    else if (c == 's')
                    {
                        showStatus = true;
                    }
                    else if (c == 'd')
                    {
                        showDate = true;
                    }
                    else if (c == 'a')
                    {
                        showIndex = true;
                        showStatus = true;
                        showDate = true;
                    }
                    else
                    {
                        Console.WriteLine($"Неизвестный флаг: -{c}");
                        return false;
                    }
                }
            }
            else
            {
                Console.WriteLine($"Неизвестный флаг: {flag}");
                return false;
            }
        }

        return true;
    }

    static void DoneTodo(string input)
    {
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length >= 2)
        {
            TodoItem? item = FindItem(parts[1], out int index);
            if (item != null)
            {
                item.MarkDone();
                Console.WriteLine($"Задача \"{item.GetShortText()}\" отмечена как выполненная!");
                return;
            }
        }

        Console.WriteLine("Неверный номер задачи. Используйте: done <номер>");
    }

    static void DeleteTodo(string input)
    {
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length >= 2)
        {
            TodoItem? item = FindItem(parts[1], out int index);
            if (item != null && todoList.Delete(index))
            {
                Console.WriteLine($"Задача \"{item.GetShortText()}\" успешно удалена!");
                return;
            }
        }

        Console.WriteLine("Неверный номер задачи. Используйте: delete <номер>");
    }

    static void UpdateTodo(string input)
    {
        string[] quoteParts = input.Split('"');
        if (quoteParts.Length >= 2 && !string.IsNullOrWhiteSpace(quoteParts[1]))
        {
            string[] firstParts = quoteParts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (firstParts.Length >= 2)
            {
                TodoItem? item = FindItem(firstParts[1], out int index);
                if (item != null)
                {
                    item.UpdateText(quoteParts[1]);
                    Console.WriteLine($"Задача {index + 1} успешно обновлена!");
                    return;
                }
            }
        }

        Console.WriteLine("Неверный формат или номер задачи. Используйте: update <номер> \"новый текст\"");
    }

    static void ReadTodo(string input)
    {
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length >= 2)
        {
            TodoItem? item = FindItem(parts[1], out int index);
            if (item != null)
            {
                Console.WriteLine($"Задача {index + 1}:");
                Console.WriteLine(item.GetFullInfo());
                return;
            }
        }

        Console.WriteLine("Неверный номер задачи. Используйте: read <номер>");
    }
}
