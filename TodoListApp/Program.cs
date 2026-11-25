using System;
using System.Text;

namespace TodoListApp
{
    class Program
    {
        const int InitialCapacity = 2;
        const int GrowthFactor = 2;
        const int IndexOffset = 1;
        const char Quote = '"';            
        const char Separator = ' '; 

        static void Main(string[] args)
        {
            
            Console.WriteLine("Работу выполнил Омельчук Зайцев и Сальников");

            Console.WriteLine("Введите Имя:");
            string firstName = Console.ReadLine() ?? "";

            Console.WriteLine("Введите Фамилию:");
            string lastName = Console.ReadLine() ?? "";

            Console.WriteLine("Введите Год Рождения:");

            int birthYear;

            while (true)
            {
                string? yearInput = Console.ReadLine();

                if (yearInput == null)
                    return;

                if (int.TryParse(yearInput, out birthYear))
                    break;

                Console.WriteLine("Введите корректный год рождения:");
            }
            
            Console.WriteLine($"Добавлен новый пользователь {firstName} {secondName}, возраст - {age}");

            string[] todos = new string[InitialCapacity];
            bool[] statuses = new bool[InitialCapacity];
            DateTime[] dates = new DateTime[InitialCapacity];

            int todoCount = 0;

            while (true)
            {
                Console.Write("> ");
                string? input = Console.ReadLine();

                if (input == null)
                            break;

                if (string.IsNullOrWhiteSpace(input))
                            continue;

                string[] commandParts = input.Trim().Split(' ', 2);
                string command = commandParts[0].ToLower();

                switch (command)
                {
                    case "help":
                        Console.WriteLine("Доступные команды:");
                        Console.WriteLine("help - список команд");
                        Console.WriteLine("profile - профиль пользователя");
                        Console.WriteLine("add \"текст\" - добавить задачу");
                        Console.WriteLine("add --multiline или -m - многострочный ввод");
                        Console.WriteLine("view - показать задачи");
                        Console.WriteLine("view --index или -i - показать индекс");
                        Console.WriteLine("view --status или -s - показать статус");
                        Console.WriteLine("view --update-date или -d - показать дату");
                        Console.WriteLine("view --all или -a - показать всё");
                        Console.WriteLine("view -is - показать индекс и статус");
                        Console.WriteLine("read <idx> - прочитать задачу");
                        Console.WriteLine("done <idx> - выполнить задачу");
                        Console.WriteLine("delete <idx> - удалить задачу");
                        Console.WriteLine("update <idx> \"текст\" - изменить задачу");
                        Console.WriteLine("exit - выход");
                        break;
                    case "profile":
                        Console.WriteLine($"{firstName} {secondName}, {birthYear}.");
                        break;
                      case "add":
                        if (commandParts.Length < 2)
                        {
                            Console.WriteLine("Введите текст задачи.");
                            break;
}

                        string task;

                        if (commandParts[1].Trim() == "--multiline" ||
                            commandParts[1].Trim() == "-m")
                        {
                            task = ReadMultiline();
                        }
                        else
                        {
                            string taskWithQuotes = commandParts[1].Trim();

                            if (taskWithQuotes.Length < 2 ||
                                taskWithQuotes[0] != '"' ||
                                taskWithQuotes[taskWithQuotes.Length - 1] != '"')
                            {
                                Console.WriteLine("Заключите задачу в кавычки.");
                                break;
                            }

                            task = taskWithQuotes.Substring(
                                1, taskWithQuotes.Length - 2);
                        }

                        if (string.IsNullOrWhiteSpace(task))
                        {
                            Console.WriteLine("Задача не может быть пустой.");
                            break;
                        }

                        if (todoCount >= todos.Length)
                        {
                            ExpandArrays(ref todos, ref statuses, ref dates);
                        }

                        todos[todoCount] = task;
                        statuses[todoCount] = false;
                        dates[todoCount] = DateTime.Now;
                        todoCount++;

                        Console.WriteLine("Задача добавлена!");
                        break;

            case "view":
                 ViewTodos(commandParts, todos, statuses, dates, todoCount);
                break;

            case "done":
                DoneTodo(commandParts, statuses, dates, todoCount);
                break;

            case "delete":
                DeleteTodo(commandParts, todos, statuses, dates, ref todoCount);
                break;

            case "update":
                UpdateTodo(commandParts, todos, dates, todoCount);
                break;
            
            case "read":
                ReadTodo(commandParts, todos, statuses, dates, todoCount);
                break;

            case "exit":
            Console.WriteLine("Выход из программы.");
            return;

        default:
            Console.WriteLine("Неизвестная команда. Введите help для просмотра команд.");
            break;
            
        }
    }
}
static void ExpandArrays(ref string[] todos,
    ref bool[] statuses, ref DateTime[] dates)
{
    int newSize = todos.Length * GrowthFactor;

    Array.Resize(ref todos, newSize);
    Array.Resize(ref statuses, newSize);
    Array.Resize(ref dates, newSize);
}

static void DoneTodo(string[] commandParts,
    bool[] statuses, DateTime[] dates, int todoCount)
{
    if (commandParts.Length < 2 ||
        !int.TryParse(commandParts[1], out int index))
    {
        Console.WriteLine("Введите номер задачи.");
        return;
    }

    index--;

    if (index < 0 || index >= todoCount)
    {
        Console.WriteLine("Задача не найдена.");
        return;
    }

    statuses[index] = true;
    dates[index] = DateTime.Now;

    Console.WriteLine("Задача выполнена!");
}

static void DeleteTodo(string[] commandParts,
    string[] todos, bool[] statuses,
    DateTime[] dates, ref int todoCount)
{
    if (commandParts.Length < 2 ||
        !int.TryParse(commandParts[1], out int index))
    {
        Console.WriteLine("Введите номер задачи.");
        return;
    }

    index--;

    if (index < 0 || index >= todoCount)
    {
        Console.WriteLine("Задача не найдена.");
        return;
    }

    for (int i = index; i < todoCount - 1; i++)
    {
        todos[i] = todos[i + 1];
        statuses[i] = statuses[i + 1];
        dates[i] = dates[i + 1];
    }

    todoCount--;

    Console.WriteLine("Задача удалена!");
}

static void UpdateTodo(string[] commandParts,
    string[] todos, DateTime[] dates, int todoCount)
{
    if (commandParts.Length < 2)
    {
        Console.WriteLine("Введите номер и новый текст задачи.");
        return;
    }

    string[] updateParts = commandParts[1].Trim().Split(' ', 2);

    if (updateParts.Length < 2 ||
        !int.TryParse(updateParts[0], out int index))
    {
        Console.WriteLine("Некорректная команда.");
        return;
    }

    index--;

    if (index < 0 || index >= todoCount)
    {
        Console.WriteLine("Задача не найдена.");
        return;
    }

    string newText = updateParts[1].Trim();

    if (newText.Length < 2 ||
        !newText.StartsWith("\"") ||
        !newText.EndsWith("\""))
    {
        Console.WriteLine("Введите текст в кавычках.");
        return;
    }

    newText = newText.Trim('"').Trim();

    if (string.IsNullOrWhiteSpace(newText))
    {
        Console.WriteLine("Текст не может быть пустым.");
        return;
    }

    todos[index] = newText;
    dates[index] = DateTime.Now;

    Console.WriteLine("Задача обновлена!");
}

static bool HasFlag(string input, string longFlag, char shortFlag)
{
    string[] flags = input.Split(' ');

    foreach (string flag in flags)
    {
        if (flag == longFlag)
        {
            return true;
        }

        if (flag.StartsWith("-") &&
            !flag.StartsWith("--") &&
            flag.Substring(1).Contains(shortFlag))
        {
            return true;
        }
    }

    return false;
}

static string ReadMultiline()
{
    StringBuilder text = new StringBuilder();

    Console.WriteLine("Введите текст задачи. Для завершения введите !end");

    while (true)
    {
        Console.Write("> ");
        string line = Console.ReadLine();

        if (line == null || line == "!end")
        {
            break;
        }

        if (text.Length > 0)
        {
            text.Append("\n");
        }

        text.Append(line);
    }

    return text.ToString();
}

static void ViewTodos(string[] commandParts,
    string[] todos, bool[] statuses,
    DateTime[] dates, int todoCount)
{
    string flags = commandParts.Length > 1
        ? commandParts[1] : "";

    bool showAll = HasFlag(flags, "--all", 'a');
    bool showIndex = showAll || HasFlag(flags, "--index", 'i');
    bool showStatus = showAll || HasFlag(flags, "--status", 's');
    bool showDate = showAll || HasFlag(flags, "--update-date", 'd');

    if (todoCount == 0)
    {
        Console.WriteLine("Список задач пуст.");
        return;
    }

    if (showIndex)
        Console.Write($"{"Индекс",-8}");

    Console.Write($"{"Задача",-35}");

    if (showStatus)
        Console.Write($"{"Статус",-15}");

    if (showDate)
        Console.Write($"{"Дата",-20}");

    Console.WriteLine();

    for (int index = 0; index < todoCount; index++)
    {
        string task = todos[index]
            .Replace("\r", "")
            .Replace("\n", " ");

        if (task.Length > 30)
        {
            task = task.Substring(0, 30) + "...";
        }

        if (showIndex)
            Console.Write($"{index + 1,-8}");

        Console.Write($"{task,-35}");

        if (showStatus)
        {
            string status = statuses[index]
                ? "Сделано" : "Не сделано";

            Console.Write($"{status,-15}");
        }

        if (showDate)
            Console.Write($"{dates[index]:dd.MM.yyyy HH:mm}");

        Console.WriteLine();
    }
}

static void ReadTodo(string[] commandParts,
    string[] todos, bool[] statuses,
    DateTime[] dates, int todoCount)
{
    if (commandParts.Length < 2 ||
        !int.TryParse(commandParts[1], out int index))
    {
        Console.WriteLine("Введите номер задачи.");
        return;
    }

    index--;

    if (index < 0 || index >= todoCount)
    {
        Console.WriteLine("Задача не найдена.");
        return;
    }

    Console.WriteLine($"Текст задачи: {todos[index]}");

    string status = statuses[index]
        ? "Выполнена" : "Не выполнена";

    Console.WriteLine($"Статус: {status}");
    Console.WriteLine($"Дата: {dates[index]:dd.MM.yyyy HH:mm}");
}