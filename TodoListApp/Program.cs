Console.WriteLine("Работу выполнили Капинус Илья Алексеевич и Халлыев Тимур Константинович.");

Console.Write("Введите имя: ");
string firstName = Console.ReadLine()!;

Console.Write("Введите фамилию: ");
string lastName = Console.ReadLine()!;

Console.Write("Введите год рождения: ");
int birthYear = int.Parse(Console.ReadLine()!);

string[] todos = new string[2];

while (true)
{
    Console.Write("> ");
    string? command = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(command))
        continue;

    if (command == "help")
    {
        Console.WriteLine("Доступные команды:");
        Console.WriteLine("help - вывести список команд");
        Console.WriteLine("profile - вывести данные пользователя");
        Console.WriteLine("add \"текст задачи\" - добавить задачу");
        Console.WriteLine("view - вывести все задачи");
        Console.WriteLine("exit - завершить программу");
    }
    else if (command == "profile")
    {
        Console.WriteLine($"{firstName} {lastName}, {birthYear}");
    }
    else if (command.StartsWith("add ", StringComparison.Ordinal))
    {
        string[] parts = command.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length < 2)
        {
            Console.WriteLine("Используйте формат: add \"текст задачи\"");
            continue;
        }

        string task = parts[1].Trim().Trim('"');
        if (task.Length == 0)
        {
            Console.WriteLine("Текст задачи не может быть пустым.");
            continue;
        }

        int emptyIndex = -1;
        for (int i = 0; i < todos.Length; i++)
        {
            if (string.IsNullOrEmpty(todos[i]))
            {
                emptyIndex = i;
                break;
            }
        }

        if (emptyIndex == -1)
        {
            string[] biggerTodos = new string[todos.Length * 2];
            for (int i = 0; i < todos.Length; i++)
            {
                biggerTodos[i] = todos[i];
            }

            todos = biggerTodos;
            emptyIndex = todos.Length / 2;
        }

        todos[emptyIndex] = task;
        Console.WriteLine($"Добавлена задача: {task}");
    }
    else if (command == "view")
    {
        Console.WriteLine("Задачи:");
        bool hasTasks = false;
        foreach (string task in todos)
        {
            if (!string.IsNullOrEmpty(task))
            {
                Console.WriteLine(task);
                hasTasks = true;
            }
        }

        if (!hasTasks)
            Console.WriteLine("Список задач пуст.");
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
