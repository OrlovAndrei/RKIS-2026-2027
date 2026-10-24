using System;
using System.Text;

class Program
{
    const int InitialSize = 2;
    const int GrowthFactor = 2;

    static string[] todos = new string[InitialSize];
    static bool[] statuses = new bool[InitialSize];
    static DateTime[] dates = new DateTime[InitialSize];
    static int todoCount = 0;

    static void Main()
    {
        Console.WriteLine("Работу выполнили Кузнецов и Курбатов");

        Console.Write("Введите имя: ");
        string firstName = Console.ReadLine() ?? "";

        Console.Write("Введите фамилию: ");
        string lastName = Console.ReadLine() ?? "";

        Console.Write("Введите год рождения: ");
        int birthYear;
        while (!int.TryParse(Console.ReadLine(), out birthYear))
        {
            Console.Write("Введите год рождения числом: ");
        }

        int age = DateTime.Now.Year - birthYear;
        Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");

        while (true)
        {
            Console.Write("> ");
            string command = Console.ReadLine() ?? "";

            if (command == "help") PrintHelp();
            else if (command == "profile") PrintProfile(firstName, lastName, birthYear);
            else if (command == "add" || command.StartsWith("add ")) AddTask(command);
            else if (command == "view" || command.StartsWith("view ")) ViewTasks(command);
            else if (command.StartsWith("done ")) DoneTask(command);
            else if (command.StartsWith("delete ")) DeleteTask(command);
            else if (command.StartsWith("update ")) UpdateTask(command);
            else if (command == "exit")
            {
                Console.WriteLine("Выход из программы");
                break;
            }
            else Console.WriteLine("Неизвестная команда. Введите help.");
        }
    }

    static void PrintHelp()
    {
        Console.WriteLine("help - список команд");
        Console.WriteLine("profile - данные пользователя");
        Console.WriteLine("add \"текст задачи\" - добавить задачу");
        Console.WriteLine("add --multiline или add -m - многострочная задача до !end");
        Console.WriteLine("view - только текст задачи (в таблице)");
        Console.WriteLine("view -i, -s, -d, -a, -is - дополнительные колонки");
        Console.WriteLine("done <номер> - отметить выполненной");
        Console.WriteLine("delete <номер> - удалить задачу");
        Console.WriteLine("update <номер> \"текст\" - изменить задачу");
        Console.WriteLine("exit - выход");
    }

    static void PrintProfile(string firstName, string lastName, int birthYear)
    {
        Console.WriteLine($"{firstName} {lastName}, {birthYear}");
    }

    static void AddTask(string command)
    {
        if (command == "add --multiline" || command == "add -m" || command == "add --multi-line")
        {
            AddMultilineTask();
            return;
        }

        string[] parts = command.Split('"');
        if (parts.Length != 3 || parts[0].Trim() != "add" ||
            parts[2].Trim() != "" || string.IsNullOrWhiteSpace(parts[1]))
        {
            Console.WriteLine("Формат: add \"текст задачи\" или add -m");
            return;
        }

        SaveTask(parts[1]);
    }

    static void AddMultilineTask()
    {
        Console.WriteLine("Введите строки задачи. Для завершения напишите !end");
        StringBuilder text = new StringBuilder();
        bool firstLine = true;

        while (true)
        {
            Console.Write("> ");
            string? line = Console.ReadLine();
            if (line == null)
            {
                Console.WriteLine("Ввод прерван, задача не добавлена");
                return;
            }
            if (line == "!end") break;

            if (!firstLine) text.Append('\n');
            text.Append(line);
            firstLine = false;
        }

        if (string.IsNullOrWhiteSpace(text.ToString()))
        {
            Console.WriteLine("Нельзя добавить пустую задачу");
            return;
        }
        SaveTask(text.ToString());
    }

    static void SaveTask(string text)
    {
        if (todoCount == todos.Length) ExpandArrays();
        todos[todoCount] = text;
        statuses[todoCount] = false;
        dates[todoCount] = DateTime.Now;
        todoCount++;
        Console.WriteLine("Задача добавлена");
    }

    static void ExpandArrays()
    {
        int newSize = todos.Length * GrowthFactor;
        string[] newTodos = new string[newSize];
        bool[] newStatuses = new bool[newSize];
        DateTime[] newDates = new DateTime[newSize];

        for (int i = 0; i < todoCount; i++)
        {
            newTodos[i] = todos[i];
            newStatuses[i] = statuses[i];
            newDates[i] = dates[i];
        }

        todos = newTodos;
        statuses = newStatuses;
        dates = newDates;
    }

    static void ViewTasks(string command)
    {
        bool showIndex = false;
        bool showStatus = false;
        bool showDate = false;
        bool showAll = false;

        string[] flags = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        for (int i = 1; i < flags.Length; i++)
        {
            string flag = flags[i];
            if (flag == "--all" || flag == "-a") showAll = true;
            else if (flag == "--index" || flag == "-i") showIndex = true;
            else if (flag == "--status" || flag == "-s") showStatus = true;
            else if (flag == "--update-date" || flag == "-d") showDate = true;
            else if (flag.Length > 2 && flag[0] == '-' && flag[1] != '-')
            {
                for (int j = 1; j < flag.Length; j++)
                {
                    if (flag[j] == 'a') showAll = true;
                    else if (flag[j] == 'i') showIndex = true;
                    else if (flag[j] == 's') showStatus = true;
                    else if (flag[j] == 'd') showDate = true;
                    else
                    {
                        Console.WriteLine("Неизвестный флаг: -" + flag[j]);
                        return;
                    }
                }
            }
            else
            {
                Console.WriteLine("Неизвестный флаг: " + flag);
                return;
            }
        }

        if (showAll)
        {
            showIndex = true;
            showStatus = true;
            showDate = true;
        }

        if (todoCount == 0)
        {
            Console.WriteLine("Список задач пуст");
            return;
        }

        string header = "";
        if (showIndex) header += "№".PadRight(3) + " | ";
        header += "Текст задачи".PadRight(33);
        if (showStatus) header += " | " + "Статус".PadRight(12);
        if (showDate) header += " | Дата изменения";

        Console.WriteLine(header);
        Console.WriteLine(new string('-', header.Length));
        for (int i = 0; i < todoCount; i++)
        {
            // В таблице заменяем переносы строк пробелами; полный текст доступен через read.
            string preview = todos[i].Replace("\r", " ").Replace("\n", " ");
            if (preview.Length > 30) preview = preview.Substring(0, 30) + "...";

            string row = "";
            if (showIndex) row += (i + 1).ToString().PadRight(3) + " | ";
            row += preview.PadRight(33);
            if (showStatus)
            {
                string status = statuses[i] ? "сделано" : "не сделано";
                row += " | " + status.PadRight(12);
            }
            if (showDate) row += " | " + dates[i].ToString("dd.MM.yyyy HH:mm:ss");
            Console.WriteLine(row);
        }
    }

    static bool TryGetIndex(string text, out int index)
    {
        index = -1;
        if (!int.TryParse(text, out int number) || number < 1 || number > todoCount)
        {
            Console.WriteLine("Задачи с таким номером нет");
            return false;
        }
        index = number - 1;
        return true;
    }

    static void DoneTask(string command)
    {
        string[] parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2)
        {
            Console.WriteLine("Формат: done 1");
            return;
        }
        if (!TryGetIndex(parts[1], out int index)) return;

        statuses[index] = true;
        dates[index] = DateTime.Now;
        Console.WriteLine("Задача выполнена");
    }

    static void DeleteTask(string command)
    {
        string[] parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2)
        {
            Console.WriteLine("Формат: delete 1");
            return;
        }
        if (!TryGetIndex(parts[1], out int index)) return;

        for (int i = index; i < todoCount - 1; i++)
        {
            todos[i] = todos[i + 1];
            statuses[i] = statuses[i + 1];
            dates[i] = dates[i + 1];
        }

        todoCount--;
        todos[todoCount] = "";
        statuses[todoCount] = false;
        dates[todoCount] = default;
        Console.WriteLine("Задача удалена");
    }

    static void UpdateTask(string command)
    {
        string[] parts = command.Split('"');
        string[] head = parts[0].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 3 || head.Length != 2 || head[0] != "update" ||
            parts[2].Trim() != "" || string.IsNullOrWhiteSpace(parts[1]))
        {
            Console.WriteLine("Формат: update 1 \"новый текст\"");
            return;
        }
        if (!TryGetIndex(head[1], out int index)) return;

        todos[index] = parts[1];
        dates[index] = DateTime.Now;
        Console.WriteLine("Задача изменена");
    }
}
