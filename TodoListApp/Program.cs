using System;
using System.Text;

class Program
{
    const int InitialCapacity = 2;

    static string firstName = "";
    static string lastName = "";
    static int birthYear = 0;

    static string[] todos = new string[InitialCapacity];
    static bool[] statuses = new bool[InitialCapacity];
    static DateTime[] dates = new DateTime[InitialCapacity];
    static int count = 0;

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
        firstName = Console.ReadLine() ?? "";
        Console.Write("Введите фамилию: ");
        lastName = Console.ReadLine() ?? "";

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

        int age = DateTime.Now.Year - birthYear;
        Console.WriteLine($"\nДобавлен пользователь {firstName} {lastName}, возраст - {age}");
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
        Console.WriteLine($"{firstName} {lastName}, {birthYear}");
    }

    static bool TryParseIndex(string text, out int index)
    {
        index = -1;
        if (!int.TryParse(text, out int number))
        {
            return false;
        }

        index = number - 1;
        return index >= 0 && index < count;
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

        if (count == todos.Length)
        {
            ResizeArrays();
        }

        todos[count] = task;
        statuses[count] = false;
        dates[count] = DateTime.Now;
        count++;
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

        if (count == 0)
        {
            Console.WriteLine("Задач пока нет.");
            return;
        }

        string header = "";
        if (showIndex)
        {
            header += $"{"№",-4} | ";
        }
        header += $"{"Задача",-33}";
        if (showStatus)
        {
            header += $" | {"Статус",-12}";
        }
        if (showDate)
        {
            header += $" | {"Изменена",-19}";
        }

        Console.WriteLine(header);
        Console.WriteLine(new string('-', header.Length));

        for (int i = 0; i < count; i++)
        {
            string row = "";
            if (showIndex)
            {
                row += $"{i + 1,-4} | ";
            }
            row += $"{GetShortText(todos[i]),-33}";
            if (showStatus)
            {
                row += $" | {GetStatusText(statuses[i]),-12}";
            }
            if (showDate)
            {
                row += $" | {dates[i]:dd.MM.yyyy HH:mm:ss}";
            }
            Console.WriteLine(row);
        }
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

    static string GetShortText(string text)
    {
        string oneLine = text.Replace("\n", " ");
        if (oneLine.Length > 30)
        {
            return oneLine.Substring(0, 30) + "...";
        }
        return oneLine;
    }

    static string GetStatusText(bool isDone)
    {
        return isDone ? "Выполнена" : "Не выполнена";
    }

    static void DoneTodo(string input)
    {
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length >= 2 && TryParseIndex(parts[1], out int index))
        {
            statuses[index] = true;
            dates[index] = DateTime.Now;
            Console.WriteLine($"Задача \"{GetShortText(todos[index])}\" отмечена как выполненная!");
            return;
        }

        Console.WriteLine("Неверный номер задачи. Используйте: done <номер>");
    }

    static void DeleteTodo(string input)
    {
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length >= 2 && TryParseIndex(parts[1], out int index))
        {
            string deletedTask = todos[index];
            for (int i = index; i < count - 1; i++)
            {
                todos[i] = todos[i + 1];
                statuses[i] = statuses[i + 1];
                dates[i] = dates[i + 1];
            }

            todos[count - 1] = "";
            statuses[count - 1] = false;
            dates[count - 1] = default;
            count--;

            Console.WriteLine($"Задача \"{GetShortText(deletedTask)}\" успешно удалена!");
            return;
        }

        Console.WriteLine("Неверный номер задачи. Используйте: delete <номер>");
    }

    static void UpdateTodo(string input)
    {
        string[] quoteParts = input.Split('"');
        if (quoteParts.Length >= 2 && !string.IsNullOrWhiteSpace(quoteParts[1]))
        {
            string[] firstParts = quoteParts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (firstParts.Length >= 2 && TryParseIndex(firstParts[1], out int index))
            {
                todos[index] = quoteParts[1];
                dates[index] = DateTime.Now;
                Console.WriteLine($"Задача {index + 1} успешно обновлена!");
                return;
            }
        }

        Console.WriteLine("Неверный формат или номер задачи. Используйте: update <номер> \"новый текст\"");
    }

    static void ReadTodo(string input)
    {
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length >= 2 && TryParseIndex(parts[1], out int index))
        {
            Console.WriteLine($"Задача {index + 1}:");
            Console.WriteLine(todos[index]);
            Console.WriteLine($"Статус: {GetStatusText(statuses[index])}");
            Console.WriteLine($"Дата последнего изменения: {dates[index]:dd.MM.yyyy HH:mm:ss}");
            return;
        }

        Console.WriteLine("Неверный номер задачи. Используйте: read <номер>");
    }

    static void ResizeArrays()
    {
        int newCapacity = todos.Length * 2;
        string[] newTodos = new string[newCapacity];
        bool[] newStatuses = new bool[newCapacity];
        DateTime[] newDates = new DateTime[newCapacity];

        for (int i = 0; i < count; i++)
        {
            newTodos[i] = todos[i];
            newStatuses[i] = statuses[i];
            newDates[i] = dates[i];
        }

        todos = newTodos;
        statuses = newStatuses;
        dates = newDates;
    }
}
