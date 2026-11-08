// TodoListApp — практические задания к Лекциям 2–4
// Лекция 4 «Методы, принципы проектирования и рефакторинг»
// Работу выполнили: Талаева и Хрустель, группа 2121у

class Program
{
    // ==================== Константы ====================

    const int StartCapacity = 2; 

    // ==================== Поля класса ====================

    // Данные пользователя
    static string firstName = "";
    static string lastName = "";
    static int birthYear = 0;

    // ==================== Три параллельных массива задач ====================

    // Тексты задач
    static string[] todos = new string[StartCapacity];

    // Выполнена ли задача
    static bool[] statuses = new bool[StartCapacity];

    // Дата создания / последнего изменения задачи
    static DateTime[] dates = new DateTime[StartCapacity];

    // Сколько задач реально добавлено
    static int todoCount = 0;

    // ==================== Точка входа ====================

    static void Main()
    {
        Console.WriteLine("Работу выполнили Талаева и Хрустель");
        Console.WriteLine();

        ReadUserInfo();

        Console.WriteLine();
        Console.WriteLine("Добро пожаловать в TodoList! Введите help для списка команд.");

        // Бесконечный цикл обработки команд
        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();

            if (input == null || input.Trim() == "exit")
                break;

            string command = input.Trim();

            // Диспетчер: каждая команда — отдельный метод
            if (command == "help")
                ShowHelp();
            else if (command == "profile")
                ShowProfile();
            else if (command == "view")
                ShowTasks();
            else if (command == "add" || command.StartsWith("add "))
                HandleAdd(command);
            else
                Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
        }

        Console.WriteLine("Программа завершена. До свидания!");
    }

    // ==================== Пользователь ====================

    // Читает данные пользователя при старте
    static void ReadUserInfo()
    {
        Console.Write("Введите имя: ");
        firstName = Console.ReadLine()!;

        Console.Write("Введите фамилию: ");
        lastName = Console.ReadLine()!;

        Console.Write("Введите год рождения: ");
        birthYear = int.Parse(Console.ReadLine()!);

        Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {CalculateAge(birthYear)}");
    }

    // Сокращённый синтаксис => метод из одной строки, сразу возвращает результат
    static int CalculateAge(int year) => DateTime.Now.Year - year;

    static void ShowProfile()
    {
        Console.WriteLine($"{firstName} {lastName}, {birthYear}");
    }

    // ==================== Команды ====================

    static void ShowHelp()
    {
        Console.WriteLine("Доступные команды:");
        Console.WriteLine("  help    — список команд");
        Console.WriteLine("  profile — данные пользователя");
        Console.WriteLine("  view    — показать все задачи");
        Console.WriteLine("  add \"текст задачи\" — добавить задачу");
        Console.WriteLine("  exit    — выход из программы");
    }

    static void ShowTasks()
    {
        if (todoCount == 0)
        {
            Console.WriteLine("Список задач пуст.");
            return;
        }

        Console.WriteLine("Ваши задачи:");
        for (int i = 0; i < todoCount; i++)
        {
            // Тернарный оператор: statuses[i] == true -> "сделано", иначе "не сделано"
            string statusText = statuses[i] ? "сделано" : "не сделано";

            string dateText = dates[i].ToString("dd.MM.yyyy");

            // Формат по заданию
            Console.WriteLine($"  {i + 1}. {todos[i]} [{statusText}] ({dateText})");
        }
    } 

    // Разбирает команду add и добавляет задачу во все три массива
    static void HandleAdd(string command)
    {
        // parts[0] — слово add, parts[1] — весь остальной текст задачи
        string[] parts = command.Split(' ', 2);
        string taskText = parts.Length > 1 ? parts[1].Trim().Trim('"') : "";

        if (taskText == "")
        {
            Console.WriteLine("Использование: add \"текст задачи\"");
            return;
        }

        // Убедиться, что во всех трёх массивах есть место
        EnsureCapacity();

        // Записываем задачу синхронно во все массивы под одним индексом
        todos[todoCount] = taskText;     // текст
        statuses[todoCount] = false;     // новая задача — не выполнена
        dates[todoCount] = DateTime.Now; // дата создания

        todoCount++;
        Console.WriteLine($"Задача добавлена: {taskText}");
    }

    // ==================== Расширение массивов ====================

    // Расширяет ВСЕ три массива в 2 раза, когда места не хватает.
    static void EnsureCapacity()
    {
        if (todoCount < todos.Length)
            return; 

        int newCapacity = todos.Length * 2;
        Console.WriteLine($"(массивы расширены до {newCapacity} элементов)");

        // Создаём новые, более длинные массивы и копируем старые значения
        string[] biggerTodos = new string[newCapacity];
        bool[] biggerStatuses = new bool[newCapacity];
        DateTime[] biggerDates = new DateTime[newCapacity];

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
} 