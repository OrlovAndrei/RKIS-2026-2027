const int InitialArrayLength = 2;
const string DateFormat = "dd.MM.yyyy HH:mm";

Console.WriteLine("Работу выполнили Капинус Илья Алексеевич и Халлыев Тимур Константинович.");

Console.Write("Введите имя: ");
string firstName = Console.ReadLine()!;

Console.Write("Введите фамилию: ");
string lastName = Console.ReadLine()!;

Console.Write("Введите год рождения: ");
int birthYear = int.Parse(Console.ReadLine()!);

string[] todos = new string[InitialArrayLength];
bool[] statuses = new bool[InitialArrayLength];
DateTime[] dates = new DateTime[InitialArrayLength];

while (true)
{
    Console.Write("> ");
    string? input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input))
        continue;

    string[] parts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
    string command = parts[0];
    string arguments = parts.Length == 2 ? parts[1] : string.Empty;

    if (command == "help")
    {
        PrintHelp();
    }
    else if (command == "profile")
    {
        PrintProfile(firstName, lastName, birthYear);
    }
    else if (command == "add")
    {
        AddTask(arguments, ref todos, ref statuses, ref dates);
    }
    else if (command == "view")
    {
        ViewTasks(todos, statuses, dates, DateFormat);
    }
    else if (command == "done")
    {
        MarkTaskDone(arguments, todos, statuses, dates);
    }
    else if (command == "delete")
    {
        DeleteTask(arguments, todos, statuses, dates);
    }
    else if (command == "update")
    {
        UpdateTask(arguments, todos, statuses, dates);
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
    Console.WriteLine("view - вывести все задачи");
    Console.WriteLine("done <idx> - отметить задачу выполненной");
    Console.WriteLine("delete <idx> - удалить задачу");
    Console.WriteLine("update <idx> \"новый текст\" - обновить задачу");
    Console.WriteLine("exit - завершить программу");
}

static void PrintProfile(string firstName, string lastName, int birthYear)
{
    Console.WriteLine($"{firstName} {lastName}, {birthYear}");
}

static int FindFirstEmptyIndex(string[] todos)
{
    for (int i = 0; i < todos.Length; i++)
    {
        if (string.IsNullOrEmpty(todos[i]))
            return i;
    }

    return -1;
}

static void ExpandArrays(ref string[] todos, ref bool[] statuses, ref DateTime[] dates)
{
    int newLength = todos.Length * 2;
    string[] biggerTodos = new string[newLength];
    bool[] biggerStatuses = new bool[newLength];
    DateTime[] biggerDates = new DateTime[newLength];

    for (int i = 0; i < todos.Length; i++)
    {
        biggerTodos[i] = todos[i];
        biggerStatuses[i] = statuses[i];
        biggerDates[i] = dates[i];
    }

    todos = biggerTodos;
    statuses = biggerStatuses;
    dates = biggerDates;
}

static void AddTask(
    string arguments,
    ref string[] todos,
    ref bool[] statuses,
    ref DateTime[] dates)
{
    string task = arguments.Trim().Trim('"');
    if (task.Length == 0)
    {
        Console.WriteLine("Используйте формат: add \"текст задачи\"");
        return;
    }

    int emptyIndex = FindFirstEmptyIndex(todos);
    if (emptyIndex == -1)
    {
        ExpandArrays(ref todos, ref statuses, ref dates);
        emptyIndex = todos.Length / 2;
    }

    todos[emptyIndex] = task;
    statuses[emptyIndex] = false;
    dates[emptyIndex] = DateTime.Now;
    Console.WriteLine($"Добавлена задача: {task}");
}

static void ViewTasks(string[] todos, bool[] statuses, DateTime[] dates, string dateFormat)
{
    Console.WriteLine("Задачи:");
    bool hasTasks = false;

    for (int i = 0; i < todos.Length; i++)
    {
        if (string.IsNullOrEmpty(todos[i]))
            continue;

        string status = statuses[i] ? "сделано" : "не сделано";
        Console.WriteLine($"{i} {todos[i]} {status} {dates[i].ToString(dateFormat)}");
        hasTasks = true;
    }

    if (!hasTasks)
        Console.WriteLine("Список задач пуст.");
}

static bool TryGetIndex(string arguments, string[] todos, out int index)
{
    if (!int.TryParse(arguments, out index) || index < 0 || index >= todos.Length)
    {
        Console.WriteLine("Укажите корректный индекс задачи.");
        return false;
    }

    if (string.IsNullOrEmpty(todos[index]))
    {
        Console.WriteLine("Задачи с таким индексом не существует.");
        return false;
    }

    return true;
}

static void MarkTaskDone(string arguments, string[] todos, bool[] statuses, DateTime[] dates)
{
    if (!TryGetIndex(arguments, todos, out int index))
        return;

    statuses[index] = true;
    dates[index] = DateTime.Now;
    Console.WriteLine($"Задача {index} отмечена выполненной.");
}

static void DeleteTask(string arguments, string[] todos, bool[] statuses, DateTime[] dates)
{
    if (!TryGetIndex(arguments, todos, out int index))
        return;

    for (int i = index; i < todos.Length - 1; i++)
    {
        todos[i] = todos[i + 1];
        statuses[i] = statuses[i + 1];
        dates[i] = dates[i + 1];
    }

    todos[todos.Length - 1] = string.Empty;
    statuses[statuses.Length - 1] = false;
    dates[dates.Length - 1] = default;
    Console.WriteLine($"Задача {index} удалена.");
}

static void UpdateTask(string arguments, string[] todos, bool[] statuses, DateTime[] dates)
{
    string[] updateParts = arguments.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
    if (updateParts.Length < 2)
    {
        Console.WriteLine("Используйте формат: update <idx> \"новый текст\"");
        return;
    }

    if (!TryGetIndex(updateParts[0], todos, out int index))
        return;

    string newText = updateParts[1].Trim().Trim('"');
    if (newText.Length == 0)
    {
        Console.WriteLine("Новый текст задачи не может быть пустым.");
        return;
    }

    todos[index] = newText;
    dates[index] = DateTime.Now;
    Console.WriteLine($"Задача {index} обновлена.");
}
