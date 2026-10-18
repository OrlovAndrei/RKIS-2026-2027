using System;

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
            else if (command.StartsWith("add ")) AddTask(command);
            else if (command == "view") ViewTasks();
            else if (command.StartsWith("done ")) DoneTask(command);
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
        Console.WriteLine("view - показать задачи");
        Console.WriteLine("done <номер> - отметить выполненной");
        Console.WriteLine("exit - выход");
    }

    static void PrintProfile(string firstName, string lastName, int birthYear)
    {
        Console.WriteLine($"{firstName} {lastName}, {birthYear}");
    }

    static void AddTask(string command)
    {
        string[] parts = command.Split('"');
        if (parts.Length != 3 || parts[0].Trim() != "add" ||
            parts[2].Trim() != "" || string.IsNullOrWhiteSpace(parts[1]))
        {
            Console.WriteLine("Формат: add \"текст задачи\"");
            return;
        }

        if (todoCount == todos.Length) ExpandArrays();
        todos[todoCount] = parts[1];
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

    static void ViewTasks()
    {
        if (todoCount == 0)
        {
            Console.WriteLine("Список задач пуст");
            return;
        }
        for (int i = 0; i < todoCount; i++)
        {
            string status = statuses[i] ? "сделано" : "не сделано";
            Console.WriteLine($"{i + 1}. {todos[i]} | {status} | {dates[i]:dd.MM.yyyy HH:mm:ss}");
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
}
