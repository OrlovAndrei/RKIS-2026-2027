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
            else if (input == "view")
            {
                ViewTodos();
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
        Console.WriteLine("  view - показать все задачи");
        Console.WriteLine("  done <номер> - отметить задачу выполненной");
        Console.WriteLine("  delete <номер> - удалить задачу");
        Console.WriteLine("  update <номер> \"текст\" - изменить задачу");
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

    static void ViewTodos()
    {
        Console.WriteLine("Список задач:");
        if (count == 0)
        {
            Console.WriteLine("Задач пока нет.");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            string status = statuses[i] ? "Сделано" : "Не сделано";
            string date = dates[i].ToString("dd.MM.yyyy HH:mm:ss");
            Console.WriteLine($"{i + 1}. {todos[i]} [{status}] ({date})");
        }
    }

    static void DoneTodo(string input)
    {
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length >= 2 && TryParseIndex(parts[1], out int index))
        {
            statuses[index] = true;
            dates[index] = DateTime.Now;
            Console.WriteLine($"Задача \"{todos[index]}\" отмечена как выполненная!");
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

            Console.WriteLine($"Задача \"{deletedTask}\" успешно удалена!");
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
