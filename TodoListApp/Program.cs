using System;
using System.Data;

Console.WriteLine("Работу выполнили Деркач и Остапенко");
Console.WriteLine("Введите вашу фамилию");
String firstName = Console.ReadLine();
Console.WriteLine("Введите ваше имя");
String secondName = Console.ReadLine();
Console.WriteLine("Введите год рождения");
int year = int.Parse(Console.ReadLine());
int number = 2026;
int age = number - year;
Console.WriteLine($"{firstName} {secondName} возраст: {age}");

string[] todos = new string[2];
int taskCount = 0;
bool[] statuses = new bool[todos.Length];
DateTime[] dates = new DateTime[todos.Length];

void ShowHelp()
{
    Console.WriteLine("     help — список команд");
    Console.WriteLine("     profile — данные пользователя");
    Console.WriteLine("     add \"задача\" — добавить задачу");
    Console.WriteLine("     view — показать задачи");
    Console.WriteLine("     read < idx > — показать полный тект задачи");
    Console.WriteLine("     exit — выход");
    Console.WriteLine("     done < idx > — отмечает задачу выполненной");
    Console.WriteLine("     update < idx > \"new_text\" обновляет текст задачи");
    Console.WriteLine("     delete < idx > — удаление задачи");
}

void AddTask(string input, string[] parts)
{
    if (parts.Length < 2)
    {
        Console.WriteLine("Ошибка: укажите текст задачи в кавычках. Формат: add \"текст задачи\"");
        return;
    }
    string raw = input.Substring(4).Trim('"');
    bool multiline = raw.Contains("--multiline") || raw.Contains("-m");

    if (multiline)
        raw = raw.Replace("--multiline", "").Replace("-m", "").Trim();

    string task;

    if (multiline)
    {
        Console.WriteLine("Многострочный режим. Вводите строки, для завершения введите !end");

        List<string> lines = new List<string>();
        while (true)
        {
            Console.Write("> ");
            string line = Console.ReadLine();

            if (line == "!end")
                break;

            if (!string.IsNullOrWhiteSpace(line))
                lines.Add(line);
        }

        if (lines.Count == 0)
        {
            Console.WriteLine("Ошибка: задача не может быть пустой.");
            return;
        }

        task = "\n" + string.Join("\n", lines) + "\n";
    }
    else
    {
        if (string.IsNullOrWhiteSpace(raw))
        {
            Console.WriteLine("Ошибка: укажите текст задачи в кавычках. Формат: add \"текст задачи\"");
            return;
        }
        task = raw.Trim('"');
    }

    if (taskCount >= todos.Length)
    {
        ExtendArrays(todos, statuses, dates, out todos, out statuses, out dates);
    }

    todos[taskCount] = task;
    statuses[taskCount] = false;
    dates[taskCount] = DateTime.Now;
    taskCount++;
    Console.WriteLine("Задача добавлена.");
}

void ShowProfile(string firstName, string secondName, int age)
{
    Console.WriteLine($"{firstName} {secondName} возраст: {age}");
}

void ShowRead(string[] parts)
{
    if (parts.Length < 2)
    {
        Console.WriteLine("Индекс не введен");
        return;
    }

    int num1 = int.Parse(parts[1]);

    if (num1 > todos.Length)
    {
        Console.WriteLine("Задачи под данным индексом не найдено. Проверить список команд: help");
        return;
    }
            
    Console.WriteLine($"{todos[num1-1]} {statuses[num1-1]} {dates[num1-1]}");
}

void DoneTask(string[] parts, bool[] statuses, DateTime[] dates)
{
    if (parts.Length < 2)
    {
        Console.WriteLine("Индекс не введен");
        return;
    }
    if (!int.TryParse(parts[1], out int idx))
    {
        Console.WriteLine("Ошибка: номер задачи должен быть числом.");
        return;
    }
    int index = idx - 1;
    statuses[index] = true;
    dates[index] = DateTime.Now;
}

void DeleteTask(string[] parts, ref string[] todos, ref bool[] statuses, ref DateTime[] dates, ref int taskCount)
{
    if (parts.Length < 2)
    {
        Console.WriteLine("Индекс не введен");
        return;
    }
    if (!int.TryParse(parts[1], out  int id))
    {
        Console.WriteLine("Ошибка: номер задачи должен быть числом.");
        return;
    }
    int indexDelete = id - 1;
    for (int i = indexDelete; i < taskCount - 1; i++)
    {
        todos[i] = todos[i + 1];
        statuses[i] = statuses[i + 1];
        dates[i] = dates[i + 1];
               
    } 
    taskCount--;
    Console.WriteLine("Задача удалена.");
}

void ShowView(string input, string[] todos, bool[] statuses, DateTime[] dates, int taskCount)
{
    if (taskCount == 0)
    {
        Console.WriteLine("Список задач пуст.");
    }

    string[] flags = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    bool showIndex = false, showStatus = false, showDate = false;

    for (int f = 1; f < flags.Length; f++)
    {
        string flag = flags[f];

        if (flag.StartsWith("--"))
        {

            switch (flag.ToLower())
            {
                case "--index":       showIndex = true; break;
                case "--status":      showStatus = true; break;
                case "--update-date": showDate = true; break;
                case "--all":         showIndex = true; showStatus = true; showDate = true; break;
                default: Console.WriteLine($"Неизвестный флаг: {flag}"); break;
            }
        }
        else if (flag.StartsWith("-") && flag.Length > 1)
        {
        
            for (int c = 1; c < flag.Length; c++)
            {
                switch (flag[c])
                {
                    case 'i': showIndex = true; break;
                    case 's': showStatus = true; break;
                    case 'd': showDate = true; break;
                    case 'a': showIndex = true; showStatus = true; showDate = true; break;
                    default: Console.WriteLine($"Неизвестный флаг: -{flag[c]}"); break;
                }
            }
        }
        else
        {
            Console.WriteLine($"Неизвестный флаг: {flag}");
        }
    }
    string[] numbers = new string[taskCount];
    string[] texts   = new string[taskCount];
    string[] states  = new string[taskCount];
    string[] datesStr = new string[taskCount];

    for (int i = 0; i < taskCount; i++)
    {
        numbers[i] = (i + 1).ToString();

        string t = todos[i].Replace("\r", "").Replace("\n", " ").Trim();
        texts[i] = t.Length > 30 ? t.Substring(0, 30) + "..." : t;

        states[i]  = statuses[i] ? "выполнена" : "не выполнена";
        datesStr[i] = dates[i].ToString("dd.MM.yyyy HH:mm");
    }

    int wNum = 1, wText = 6, wState = 6, wDate = 14;
    for (int i = 0; i < taskCount; i++)
    {
        if (numbers[i].Length > wNum) wNum = numbers[i].Length;
        if (texts[i].Length > wText) wText = texts[i].Length;
        if (states[i].Length > wState) wState = states[i].Length;
    }

    string header = "";
    if (showIndex) header += "№".PadRight(wNum) + "  ";
    header += "Задача".PadRight(wText);
    if (showStatus) header += "  " + "Статус".PadRight(wState);
    if (showDate)   header += "  " + "Дата изменения".PadRight(wDate);

    Console.WriteLine(header);
    Console.WriteLine(new string('-', header.Length));

    for (int i = 0; i < taskCount; i++)
    {
        string row = "";
        if (showIndex)  row += numbers[i].PadRight(wNum) + "  ";
        row += texts[i].PadRight(wText);
        if (showStatus) row += "  " + states[i].PadRight(wState);
        if (showDate)   row += "  " + datesStr[i].PadRight(wDate);
        Console.WriteLine(row);
    }
}

void UpdateTask(string[] parts, string[] todos, DateTime[] dates)
{
    if (parts.Length < 3)
    {
        Console.WriteLine("Неверный ввод , чего то не хватает");
        return;
    }
    if (!int.TryParse(parts[1].Trim("<>"), out int id))
    {
        Console.WriteLine("Ошибка: номер задачи должен быть числом.");
        return;
    }

    if (string.IsNullOrWhiteSpace(parts[2]))
    {
        Console.WriteLine("Текст задачи не может быть пустым.");
        return;
    }
    int indexUpdate = id - 1;
    todos[indexUpdate] = parts[2].Trim('"');
    dates[indexUpdate] = DateTime.Now;
}

void ExtendArrays(
    string[] todos,
    bool[] statuses,
    DateTime[] dates,
    out string[] newTodos,
    out bool[] newStatuses,
    out DateTime[] newDates)
{
    int newSize = todos.Length * 2;

    newTodos = new string[newSize];
    newStatuses = new bool[newSize];
    newDates = new DateTime[newSize];

    Array.Copy(todos, newTodos, todos.Length);
    Array.Copy(statuses, newStatuses, statuses.Length);
    Array.Copy(dates, newDates, dates.Length);
}

while (true)
{
    Console.Write("Введите команду: ");
    string input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input)) continue;

    string[] parts = input.Split(' ', 3);
    string command = parts[0].ToLower();
    
    
    switch (command)
    {
        case "help": ShowHelp(); break;
        case "add": AddTask(input, parts); break;
        case "profile": ShowProfile(firstName, secondName, age); break;
        case "done": DoneTask(parts, statuses, dates); break;
        case "view": ShowView(input, todos, statuses, dates, taskCount); break;
        case "delete": DeleteTask(parts, ref todos, ref statuses, ref dates, ref taskCount); break;
        case "update": UpdateTask(parts, todos, dates); break;
        case "read": ShowRead(parts); break;
        case "exit":
            Console.WriteLine("Завершение программы...");
            return;
        default:
            Console.WriteLine("Неизвестная комманда. Введите help, для отображения списка команд");
            break;
    }
}