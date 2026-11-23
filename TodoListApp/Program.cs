using System.Text;
using TodoListApp;

Console.WriteLine("Работу выполнили Капинус Илья Алексеевич и Халлыев Тимур Константинович.");

Console.Write("Введите имя: ");
string? firstName = Console.ReadLine();
if (string.IsNullOrWhiteSpace(firstName))
    return;

Console.Write("Введите фамилию: ");
string? lastName = Console.ReadLine();
if (string.IsNullOrWhiteSpace(lastName))
    return;

int birthYear;
while (true)
{
    Console.Write("Введите год рождения: ");
    string? birthYearInput = Console.ReadLine();
    if (birthYearInput is null)
        return;

    if (int.TryParse(birthYearInput, out birthYear))
        break;

    Console.WriteLine("Введите год числом.");
}

Profile profile = new(firstName, lastName, birthYear);
TodoList todoList = new();

while (true)
{
    Console.Write("> ");
    string? input = Console.ReadLine();

    if (input is null)
        break;

    if (string.IsNullOrWhiteSpace(input))
        continue;

    string[] commandParts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
    string command = commandParts[0];
    string arguments = commandParts.Length == 2 ? commandParts[1].Trim() : string.Empty;

    if (command == "help")
    {
        PrintHelp();
    }
    else if (command == "profile")
    {
        PrintProfile(profile);
    }
    else if (command == "add")
    {
        AddTask(arguments, todoList);
    }
    else if (command == "view")
    {
        ViewTasks(arguments, todoList);
    }
    else if (command == "read")
    {
        ReadTask(arguments, todoList);
    }
    else if (command == "done")
    {
        MarkTaskDone(arguments, todoList);
    }
    else if (command == "delete")
    {
        DeleteTask(arguments, todoList);
    }
    else if (command == "update")
    {
        UpdateTask(arguments, todoList);
    }
    else if (command == "exit")
    {
        break;
    }
    else
    {
        Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
    }
}

static void PrintHelp()
{
    Console.WriteLine("Доступные команды:");
    Console.WriteLine("help - вывести список команд");
    Console.WriteLine("profile - вывести данные пользователя");
    Console.WriteLine("add \"текст задачи\" - добавить задачу");
    Console.WriteLine("add --multiline, --multi-line (-m) - добавить многострочную задачу; завершите ввод строкой !end");
    Console.WriteLine("view [--index (-i)] [--status (-s)] [--update-date (-d)] [--all (-a)] - вывести задачи");
    Console.WriteLine("read <idx> - вывести полную информацию о задаче");
    Console.WriteLine("done <idx> - отметить задачу выполненной");
    Console.WriteLine("delete <idx> - удалить задачу");
    Console.WriteLine("update <idx> \"новый текст\" - обновить задачу");
    Console.WriteLine("exit - завершить программу");
}

static void PrintProfile(Profile profile)
{
    Console.WriteLine(profile.GetInfo());
}

static void AddTask(string arguments, TodoList todoList)
{
    bool multiline = false;
    string task;

    if (arguments.StartsWith("-", StringComparison.Ordinal))
    {
        string[] flagParts = arguments.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        string flag = flagParts[0];
        if (flag == "--multiline" || flag == "--multi-line" || flag == "-m")
        {
            multiline = true;
            if (flagParts.Length > 1)
            {
                Console.WriteLine("В многострочном режиме вводите задачу отдельными строками.");
                return;
            }
        }
        else
        {
            Console.WriteLine($"Неизвестный флаг команды add: {flag}");
            return;
        }
    }

    if (multiline)
    {
        Console.WriteLine("Вводите строки задачи. Для завершения введите !end.");
        StringBuilder taskBuilder = new();
        bool hasLines = false;

        while (true)
        {
            Console.Write("> ");
            string? line = Console.ReadLine();
            if (line is null)
            {
                Console.WriteLine("Ввод завершён до !end. Задача не добавлена.");
                return;
            }

            if (line == "!end")
                break;

            if (hasLines)
                taskBuilder.Append('\n');

            taskBuilder.Append(line);
            hasLines = true;
        }

        task = taskBuilder.ToString();
    }
    else
    {
        task = arguments.Trim().Trim('"');
    }

    if (string.IsNullOrWhiteSpace(task))
    {
        Console.WriteLine("Текст задачи не может быть пустым.");
        return;
    }

    todoList.Add(new TodoItem(task));
    Console.WriteLine($"Добавлена задача: {task}");
}

static bool TryParseViewFlags(
    string arguments,
    out bool showIndex,
    out bool showDone,
    out bool showDate,
    out string error)
{
    showIndex = false;
    showDone = false;
    showDate = false;
    error = string.Empty;

    string[] flags = arguments.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    foreach (string flag in flags)
    {
        if (flag == "--all" || flag == "-a")
        {
            showIndex = true;
            showDone = true;
            showDate = true;
        }
        else if (flag == "--index" || flag == "-i")
        {
            showIndex = true;
        }
        else if (flag == "--status" || flag == "-s")
        {
            showDone = true;
        }
        else if (flag == "--update-date" || flag == "-d")
        {
            showDate = true;
        }
        else if (flag.StartsWith("-", StringComparison.Ordinal) && !flag.StartsWith("--", StringComparison.Ordinal))
        {
            for (int i = 1; i < flag.Length; i++)
            {
                if (flag[i] == 'i')
                    showIndex = true;
                else if (flag[i] == 's')
                    showDone = true;
                else if (flag[i] == 'd')
                    showDate = true;
                else if (flag[i] == 'a')
                {
                    showIndex = true;
                    showDone = true;
                    showDate = true;
                }
                else
                {
                    error = $"Неизвестный флаг команды view: -{flag[i]}";
                    return false;
                }
            }
        }
        else
        {
            error = $"Неизвестный флаг команды view: {flag}";
            return false;
        }
    }

    return true;
}

static void ViewTasks(string arguments, TodoList todoList)
{
    if (!TryParseViewFlags(arguments, out bool showIndex, out bool showDone, out bool showDate, out string error))
    {
        Console.WriteLine(error);
        return;
    }

    todoList.View(showIndex, showDone, showDate);
}

static TodoItem? FindTask(string arguments, TodoList todoList)
{
    if (!int.TryParse(arguments, out int index))
    {
        Console.WriteLine("Укажите корректный индекс задачи.");
        return null;
    }

    TodoItem? item = todoList.GetItem(index);
    if (item is null)
        Console.WriteLine("Задачи с таким индексом не существует.");

    return item;
}

static void ReadTask(string arguments, TodoList todoList)
{
    TodoItem? item = FindTask(arguments, todoList);
    if (item is null)
        return;

    Console.WriteLine(item.GetFullInfo());
}

static void MarkTaskDone(string arguments, TodoList todoList)
{
    TodoItem? item = FindTask(arguments, todoList);
    if (item is null)
        return;

    item.MarkDone();
    Console.WriteLine($"Задача {arguments} отмечена выполненной.");
}

static void DeleteTask(string arguments, TodoList todoList)
{
    TodoItem? item = FindTask(arguments, todoList);
    if (item is null)
        return;

    int.TryParse(arguments, out int index);
    todoList.Delete(index);
    Console.WriteLine($"Задача {index} удалена.");
}

static void UpdateTask(string arguments, TodoList todoList)
{
    string[] updateParts = arguments.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
    if (updateParts.Length < 2)
    {
        Console.WriteLine("Используйте формат: update <idx> \"новый текст\"");
        return;
    }

    TodoItem? item = FindTask(updateParts[0], todoList);
    if (item is null)
        return;

    string newText = updateParts[1].Trim().Trim('"');
    if (string.IsNullOrWhiteSpace(newText))
    {
        Console.WriteLine("Новый текст задачи не может быть пустым.");
        return;
    }

    item.UpdateText(newText);
    Console.WriteLine($"Задача {updateParts[0]} обновлена.");
}
