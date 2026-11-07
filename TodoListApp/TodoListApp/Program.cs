// TodoListApp — практические задания к Лекциям 2–4
// Работу выполнили: Талаева и Хрустель, группа 2121у

class Program
{
    // ==================== Константы (вместо магических значений) ====================

    const int StartCapacity = 2;   // начальная ёмкость массива задач

    // ==================== Поля класса — видны всем методам ====================

    static string? firstName;   // имя пользователя
    static string? lastName;    // фамилия пользователя
    static int birthYear;       // год рождения

    static string[] todos = new string[StartCapacity]; // тексты задач
    static int todoCount;       // сколько задач реально добавлено

    // ==================== Точка входа ====================

    static void Main()
    {
        Console.WriteLine("Работу выполнили Талаева и Хрустель");
        Console.WriteLine();

        ReadUserInfo();

        Console.WriteLine();
        Console.WriteLine("Добро пожаловать в TodoList! Введите help для списка команд.");

        // ---------- Диспетчер команд ----------
        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();

            if (input == null || input.Trim() == "exit")
                break;

            string command = input.Trim();

            if (command == "help")
            {
                ShowHelp();
            }
            else if (command == "profile")
            {
                ShowProfile();
            }
            else if (command == "view")
            {
                ShowTasks();
            }
            else if (command.StartsWith("add"))
            {
                string[] parts = command.Split(' ', 2);

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
                    AddTask(taskText);
                }
            }
            else
            {
                Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
            }
        }

        Console.WriteLine("Программа завершена. До свидания!");
    }

    // ==================== Методы ====================

    // Читает данные пользователя (Лекция 2)
    static void ReadUserInfo()
    {
        Console.Write("Введите имя: ");
        firstName = Console.ReadLine();

        Console.Write("Введите фамилию: ");
        lastName = Console.ReadLine();

        Console.Write("Введите год рождения: ");
        birthYear = int.Parse(Console.ReadLine()!);

        Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {CalculateAge()}");
    }

    // Сокращённый синтаксис => : метод сразу возвращает выражение
    static int CalculateAge() => DateTime.Now.Year - birthYear;

    static void ShowHelp()
    {
        Console.WriteLine("Доступные команды:");
        Console.WriteLine("  help    — список команд");
        Console.WriteLine("  profile — данные пользователя");
        Console.WriteLine("  view    — показать все задачи");
        Console.WriteLine("  add \"текст задачи\" — добавить задачу");
        Console.WriteLine("  exit    — выход из программы");
    }

    static void ShowProfile()
    {
        Console.WriteLine($"{firstName} {lastName}, {birthYear}");
    }

    static void ShowTasks()
    {
        Console.WriteLine("Ваши задачи:");

        bool hasTasks = false;

        for (int i = 0; i < todos.Length; i++)
        {
            if (string.IsNullOrEmpty(todos[i]))
                continue;

            Console.WriteLine($"  {i + 1}. {todos[i]}");
            hasTasks = true;
        }

        if (!hasTasks)
            Console.WriteLine("  (список пуст)");
    }

    // Добавляет задачу, при нехватке места сначала расширяет массив
    static void AddTask(string taskText)
    {
        if (todoCount == todos.Length)
        {
            ExpandArrays();
            Console.WriteLine($"(массив расширен до {todos.Length} элементов)");
        }

        todos[todoCount] = taskText;
        todoCount++;
        Console.WriteLine($"Задача добавлена: {taskText}");
    }

    // Создаёт новый массив в 2 раза длиннее и переносит данные циклом
    static void ExpandArrays()
    {
        string[] bigger = new string[todos.Length * 2];

        for (int i = 0; i < todos.Length; i++)
        {
            bigger[i] = todos[i];
        }

        todos = bigger;
    }
}