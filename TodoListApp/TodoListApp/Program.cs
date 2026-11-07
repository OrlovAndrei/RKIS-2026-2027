// TodoListApp — практическое задание к Лекции 2 «Введение в C# и .NET»
// Работу выполнили: Талаева и Хрустель, группа 2121у

Console.WriteLine("Работу выполнили Талаева и Хрустель");
Console.WriteLine();

Console.Write("Введите имя: ");
string? firstName = Console.ReadLine();

Console.Write("Введите фамилию: ");
string? lastName = Console.ReadLine();

Console.Write("Введите год рождения: ");
int birthYear = int.Parse(Console.ReadLine()!);

int currentYear = DateTime.Now.Year;
int age = currentYear - birthYear;

Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");

// Практическое задание к Лекции 3 «Ветвление и циклы»

// Массив для задач
string[] todos = new string[2];
int todoCount = 0;

Console.WriteLine();
Console.WriteLine("Добро пожаловать в TodoList! Введите help для списка команд.");

// бесконечный цикл: условие всегда истинно
while (true)
{
    Console.Write("> ");
    string? input = Console.ReadLine();

    // выходим из цикла
    if (input == null || input.Trim() == "exit")
        break;

    string command = input.Trim(); 

    if (command == "help")
    {
        Console.WriteLine("Доступные команды:");
        Console.WriteLine("  help    — список команд");
        Console.WriteLine("  profile — данные пользователя");
        Console.WriteLine("  view    — показать все задачи");
        Console.WriteLine("  add \"текст задачи\" — добавить задачу");
        Console.WriteLine("  exit    — выход из программы");
    }

    else if (command == "profile")
    {
        // Профиль: <Имя> <Фамилия>, <Год рождения>
        Console.WriteLine($"{firstName} {lastName}, {birthYear}");
    }

    else if (command == "view")
    {
        Console.WriteLine("Ваши задачи:");

        bool hasTasks = false; // встретили ли хоть одну задачу

        for (int i = 0; i < todos.Length; i++)
        {
            // Пустые ячейки массива пропускаем
            if (string.IsNullOrEmpty(todos[i]))
                continue;

            Console.WriteLine($"  {i + 1}. {todos[i]}");
            hasTasks = true;
        }

        if (!hasTasks)
            Console.WriteLine("  (список пуст)");
    }

    else if (command.StartsWith("add"))
    {
        string[] parts = command.Split(' ', 2);

        // Текст задачи: всё после слова add, без пробелов по краям и без кавычек
        string taskText = "";
        if (parts.Length > 1)
        {
            taskText = parts[1].Trim().Trim('"');
        }

        if (taskText == "")
        {
            Console.WriteLine("Использование: add \"текст задачи\"");
        }
        else
        {
            todos[todoCount] = taskText;
            todoCount++;
            Console.WriteLine($"Задача добавлена: {taskText}");
        }
    }

    else
    {
        Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
    }
}

Console.WriteLine("Программа завершена. До свидания!");