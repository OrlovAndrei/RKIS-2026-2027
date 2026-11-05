
using System;

class Program
{
    const int InitialCapacity = 2;
    const int CapacityMultiplier = 2;
    const string DateFormat = "dd.MM.yyyy HH:mm";

    static void Main()
    {
        Console.WriteLine(".:| TodoListApp |:.");

        Console.Write("Введите имя: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Введите фамилию: ");
        string surname = Console.ReadLine() ?? "";

        Console.Write("Введите год рождения: ");
        int birthYear;

        while (!int.TryParse(Console.ReadLine(), out birthYear))
        {
            Console.Write("Введите год рождения: ");
        }

        string[] todos = new string[InitialCapacity];
        bool[] statuses = new bool[InitialCapacity];
        DateTime[] dates = new DateTime[InitialCapacity];

        int todoCount = 0;
        bool running = true;

        Console.WriteLine();
        Console.WriteLine($"Добро пожаловать, {name} {surname}!");
        Console.WriteLine("Введите help для просмотра команд.");

        while (running)
        {
            Console.WriteLine();
            Console.Write("> ");

            string command = Console.ReadLine() ?? "";

            string[] parts = command.Trim().Split(
                ' ',
                2,
                StringSplitOptions.RemoveEmptyEntries
            );

            if (parts.Length == 0)
                continue;

            string action = parts[0].ToLower();

            switch (action)
            {
                case "help":
                    ShowHelp();
                    break;

                case "profile":
                    ShowProfile(name, surname, birthYear);
                    break;

                case "add":
                    AddTask(
                        parts, ref todos, ref statuses,
                        ref dates, ref todoCount
                    );
                    break;

                case "view":
                    ViewTasks(todos, statuses, dates, todoCount);
                    break;

                case "done":
                    MarkTaskDone(
                        parts, todos, statuses, dates, todoCount
                    );
                    break;

                case "delete":
                    DeleteTask(
                        parts, ref todos, ref statuses,
                        ref dates, ref todoCount
                    );
                    break;

                case "update":
                    UpdateTask(parts, todos, dates, todoCount);
                    break;

                case "exit":
                    running = false;
                    Console.WriteLine("Программа завершена.");
                    break;

                default:
                    Console.WriteLine("Непонятная команда. Введите help.");
                    break;
            }
        }
    }

    // СПИСОК КОМАНД
    static void ShowHelp()
    {
        Console.WriteLine("Доступные команды:");
        Console.WriteLine("help - список команд");
        Console.WriteLine("profile - информация о пользователе");
        Console.WriteLine("add <текст> - добавить задачу");
        Console.WriteLine("view - показать задачи");
        Console.WriteLine("done <id> - выполнить задачу");
        Console.WriteLine("delete <id> - удалить задачу");
        Console.WriteLine("update <id> <текст> - изменить задачу");
        Console.WriteLine("exit - выход");
    }

    // ПРОФИЛЬ
    static void ShowProfile(string name, string surname, int birthYear)
    {
        int age = DateTime.Now.Year - birthYear;

        Console.WriteLine(
            $"{name} {surname}, год рождения: {birthYear}, возраст: {age}"
        );
    }

    static void EnsureCapacity(
        ref string[] todos,
        ref bool[] statuses,
        ref DateTime[] dates,
        int requiredCount)
    {
        if (requiredCount <= todos.Length)
            return;

        int newCapacity = todos.Length * CapacityMultiplier;

        while (newCapacity < requiredCount)
        {
            newCapacity *= CapacityMultiplier;
        }

        Array.Resize(ref todos, newCapacity);
        Array.Resize(ref statuses, newCapacity);
        Array.Resize(ref dates, newCapacity);

        Console.WriteLine($"Размер массивов увеличен до {newCapacity}.");
    }

    // ДОБАВЛЕНИЕ
    static void AddTask(
        string[] parts,
        ref string[] todos,
        ref bool[] statuses,
        ref DateTime[] dates,
        ref int todoCount)
    {
        if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[1]))
        {
            Console.WriteLine("Использование: add <текст задачи>");
            return;
        }

        EnsureCapacity(
            ref todos, ref statuses, ref dates, todoCount + 1
        );

        todos[todoCount] = parts[1];
        statuses[todoCount] = false;
        dates[todoCount] = DateTime.Now;

        todoCount++;

        Console.WriteLine("Задача добавлена!");
    }

    // ПОСМОТРЕТЬ
    static void ViewTasks(
        string[] todos,
        bool[] statuses,
        DateTime[] dates,
        int todoCount)
    {
        if (todoCount == 0)
        {
            Console.WriteLine("Список задач пуст.");
            return;
        }

        Console.WriteLine("Список задач:");

        for (int i = 0; i < todoCount; i++)
        {
            string status = statuses[i] ? "сделано" : "не сделано";

            Console.WriteLine(
                $"{i + 1} {todos[i]} {status} {dates[i].ToString(DateFormat)}"
            );
        }
    }


    static bool TryGetTaskIndex(
        string[] parts,
        int todoCount,
        out int index)
    {
        index = -1;

        if (parts.Length < 2)
        {
            Console.WriteLine("Укажите номер задачи.");
            return false;
        }

        string[] arguments = parts[1].Split(
            ' ',
            2,
            StringSplitOptions.RemoveEmptyEntries
        );

        if (!int.TryParse(arguments[0], out int userIndex))
        {
            Console.WriteLine("Укажите корректный номер задачи.");
            return false;
        }

        if (userIndex < 1 || userIndex > todoCount)
        {
            Console.WriteLine("Задача с таким номером не найдена.");
            return false;
        }

        index = userIndex - 1;
        return true;
    }


    static void MarkTaskDone(
        string[] parts,
        string[] todos,
        bool[] statuses,
        DateTime[] dates,
        int todoCount)
    {
        if (!TryGetTaskIndex(parts, todoCount, out int index))
            return;

        statuses[index] = true;
        dates[index] = DateTime.Now;

        Console.WriteLine($"Задача \"{todos[index]}\" выполнена!");
    }

    static void DeleteTask(
        string[] parts,
        ref string[] todos,
        ref bool[] statuses,
        ref DateTime[] dates,
        ref int todoCount)
    {
        if (!TryGetTaskIndex(parts, todoCount, out int index))
            return;

        string deletedTask = todos[index];

        for (int i = index; i < todoCount - 1; i++)
        {
            todos[i] = todos[i + 1];
            statuses[i] = statuses[i + 1];
            dates[i] = dates[i + 1];
        }

        todoCount--;

        todos[todoCount] = null!;
        statuses[todoCount] = false;
        dates[todoCount] = default;

        Console.WriteLine($"Задача \"{deletedTask}\" удалена.");
    }

    static void UpdateTask(
        string[] parts,
        string[] todos,
        DateTime[] dates,
        int todoCount)
    {
        if (!TryGetTaskIndex(parts, todoCount, out int index))
            return;

        string[] updateParts = parts[1].Split(
            ' ',
            2,
            StringSplitOptions.RemoveEmptyEntries
        );

        if (updateParts.Length < 2 ||
            string.IsNullOrWhiteSpace(updateParts[1]))
        {
            Console.WriteLine("Использование: update <id> <новый текст>");
            return;
        }

        todos[index] = updateParts[1];
        dates[index] = DateTime.Now;

        Console.WriteLine("Задача обновлена!");
    }
}
