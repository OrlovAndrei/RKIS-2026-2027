using System.Text;

const int InitialArrayLength = 2;
const string DateFormat = "dd.MM.yyyy HH:mm";

Console.WriteLine("Работу выполнили Капинус Илья Алексеевич и Халлыев Тимур Константинович.");

Console.Write("Введите имя: ");
string? firstNameInput = Console.ReadLine();
if (firstNameInput is null)
    return;

Console.Write("Введите фамилию: ");
string? lastNameInput = Console.ReadLine();
if (lastNameInput is null)
    return;

string firstName = firstNameInput;
string lastName = lastNameInput;

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
    string arguments = parts.Length == 2 ? parts[1].Trim() : string.Empty;

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
        ViewTasks(arguments, todos, statuses, dates, DateFormat);
    }
    else if (command == "read")
    {
        ReadTask(arguments, todos, statuses, dates, DateFormat);
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
    Console.WriteLine("add --multiline (-m) - добавить многострочную задачу; завершите ввод строкой !end");
    Console.WriteLine("view [--index (-i)] [--status (-s)] [--update-date (-d)] [--all (-a)] - вывести задачи");
    Console.WriteLine("read <idx> - вывести полную информацию о задаче");
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
    bool multiline = false;
    string task;

    if (arguments.StartsWith("--", StringComparison.Ordinal) || arguments.StartsWith("-", StringComparison.Ordinal))
    {
        string[] parts = arguments.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        string flag = parts[0];
        if (flag == "--multiline" || flag == "-m")
        {
            multiline = true;
            if (parts.Length > 1)
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

static bool TryParseViewFlags(
    string arguments,
    out bool showIndex,
    out bool showStatus,
    out bool showUpdateDate,
    out string error)
{
    showIndex = false;
    showStatus = false;
    showUpdateDate = false;
    error = string.Empty;

    string[] flags = arguments.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    foreach (string flag in flags)
    {
        if (flag == "--all" || flag == "-a")
        {
            showIndex = true;
            showStatus = true;
            showUpdateDate = true;
        }
        else if (flag == "--index" || flag == "-i")
        {
            showIndex = true;
        }
        else if (flag == "--status" || flag == "-s")
        {
            showStatus = true;
        }
        else if (flag == "--update-date" || flag == "-d")
        {
            showUpdateDate = true;
        }
        else if (flag.StartsWith("-", StringComparison.Ordinal) && !flag.StartsWith("--", StringComparison.Ordinal))
        {
            for (int i = 1; i < flag.Length; i++)
            {
                if (flag[i] == 'i')
                    showIndex = true;
                else if (flag[i] == 's')
                    showStatus = true;
                else if (flag[i] == 'd')
                    showUpdateDate = true;
                else if (flag[i] == 'a')
                {
                    showIndex = true;
                    showStatus = true;
                    showUpdateDate = true;
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

static void ViewTasks(
    string arguments,
    string[] todos,
    bool[] statuses,
    DateTime[] dates,
    string dateFormat)
{
    if (!TryParseViewFlags(arguments, out bool showIndex, out bool showStatus, out bool showUpdateDate, out string error))
    {
        Console.WriteLine(error);
        return;
    }

    List<string> headers = new();
    if (showIndex)
        headers.Add("Индекс");

    headers.Add("Текст задачи");

    if (showStatus)
        headers.Add("Статус");

    if (showUpdateDate)
        headers.Add("Дата изменения");

    List<string[]> rows = new();
    for (int i = 0; i < todos.Length; i++)
    {
        if (string.IsNullOrEmpty(todos[i]))
            continue;

        List<string> cells = new();
        if (showIndex)
            cells.Add(i.ToString());

        cells.Add(FormatTaskPreview(todos[i]));

        if (showStatus)
            cells.Add(statuses[i] ? "выполнена" : "не выполнена");

        if (showUpdateDate)
            cells.Add(dates[i].ToString(dateFormat));

        rows.Add(cells.ToArray());
    }

    if (rows.Count == 0)
    {
        Console.WriteLine("Список задач пуст.");
        return;
    }

    int[] widths = new int[headers.Count];
    for (int i = 0; i < headers.Count; i++)
        widths[i] = headers[i].Length;

    foreach (string[] row in rows)
    {
        for (int i = 0; i < row.Length; i++)
            widths[i] = Math.Max(widths[i], row[i].Length);
    }

    Console.WriteLine(BuildTableBorder(widths));
    Console.WriteLine(BuildTableRow(headers, widths));
    Console.WriteLine(BuildTableBorder(widths));
    foreach (string[] row in rows)
        Console.WriteLine(BuildTableRow(row, widths));

    Console.WriteLine(BuildTableBorder(widths));
}

static string FormatTaskPreview(string task)
{
    string oneLineTask = task.Replace("\r", string.Empty, StringComparison.Ordinal)
        .Replace("\n", "\\n", StringComparison.Ordinal);

    return oneLineTask.Length > 30 ? oneLineTask[..30] + "..." : oneLineTask;
}

static string BuildTableBorder(int[] widths)
{
    StringBuilder border = new();
    foreach (int width in widths)
        border.Append('+').Append('-', width + 2);

    return border.Append('+').ToString();
}

static string BuildTableRow(IReadOnlyList<string> cells, int[] widths)
{
    StringBuilder row = new();

    for (int i = 0; i < cells.Count; i++)
        row.Append("| ").Append(cells[i].PadRight(widths[i])).Append(' ');

    return row.Append('|').ToString();
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

static void ReadTask(
    string arguments,
    string[] todos,
    bool[] statuses,
    DateTime[] dates,
    string dateFormat)
{
    if (!TryGetIndex(arguments, todos, out int index))
        return;

    Console.WriteLine(todos[index]);
    Console.WriteLine($"Статус: {(statuses[index] ? "выполнена" : "не выполнена")}");
    Console.WriteLine($"Дата последнего изменения: {dates[index].ToString(dateFormat)}");
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
