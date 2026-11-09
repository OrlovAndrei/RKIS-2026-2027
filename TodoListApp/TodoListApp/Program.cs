// TodoListApp — практические задания к Лекциям 2–4
// Лекция 4 «Методы, принципы проектирования и рефакторинг»
// Работу выполнили: Талаева и Хрустель, группа 2121у

class Program
{

    const int StartCapacity = 2;

    //Данные пользователя
    static string firstName = "";
    static string lastName = "";
    static int birthYear = 0;

    static string[] todos = new string[StartCapacity];

    static bool[] statuses = new bool[StartCapacity];

    static DateTime[] dates = new DateTime[StartCapacity];

    static int todoCount = 0;

    static void Main()
    {
        Console.WriteLine("Работу выполнили Талаева и Хрустель");
        Console.WriteLine();

        ReadUserInfo();

        Console.WriteLine();
        Console.WriteLine("Добро пожаловать в TodoList! Введите help для списка команд.");

        //Бесконечный цикл обработки команд
        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();

            if (input == null || input.Trim() == "exit")
                break;

            string command = input.Trim();

            //Диспетчер: каждая команда — отдельный метод
            if (command == "help")
                ShowHelp();
            else if (command == "profile")
                ShowProfile();
            else if (command == "view")
                ShowTasks();
            else if (command == "add" || command.StartsWith("add "))
                HandleAdd(command);
            else if (command == "done" || command.StartsWith("done "))
                HandleDone(command);
            else if (command == "update" || command.StartsWith("update "))
                HandleUpdate(command);
            else if (command == "delete" || command.StartsWith("delete "))
                HandleDelete(command);
            else
                Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
        }

        Console.WriteLine("Программа завершена. До свидания!");
    }

    //Пользователь

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

    static int CalculateAge(int year) => DateTime.Now.Year - year;

    static void ShowProfile()
    {
        Console.WriteLine($"{firstName} {lastName}, {birthYear}");
    }

    //Команды

    static void ShowHelp()
    {
        Console.WriteLine("Доступные команды:");
        Console.WriteLine("  help    — список команд");
        Console.WriteLine("  profile — данные пользователя");
        Console.WriteLine("  view    — показать все задачи");
        Console.WriteLine("  add \"текст задачи\" — добавить задачу");
        Console.WriteLine("  done <номер> — отметить задачу выполненной");
        Console.WriteLine("  update <номер> \"новый текст\" — изменить текст задачи");
        Console.WriteLine("  delete <номер> — удалить задачу");
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
            string statusText = statuses[i] ? "сделано" : "не сделано";

            string dateText = dates[i].ToString("dd.MM.yyyy");

            Console.WriteLine($"  {i + 1}. {todos[i]} [{statusText}] ({dateText})");
        }
    }

    //Разбирает команду add и добавляет задачу во все три массива
    static void HandleAdd(string command)
    {
        string[] parts = command.Split(' ', 2);
        string taskText = parts.Length > 1 ? parts[1].Trim().Trim('"') : "";

        if (taskText == "")
        {
            Console.WriteLine("Использование: add \"текст задачи\"");
            return;
        }

        EnsureCapacity();

        todos[todoCount] = taskText; 
        statuses[todoCount] = false; 
        dates[todoCount] = DateTime.Now; 

        todoCount++;
        Console.WriteLine($"Задача добавлена: {taskText}");
    }

    //Отмечает задачу выполненной
    static void HandleDone(string command)
    {
        string[] parts = command.Split(' ');

        if (parts.Length < 2)
        {
            Console.WriteLine("Использование: done <номер задачи>");
            return;
        }

        int index = GetTaskIndex(parts[1]);

        if (index == -1)
        {
            Console.WriteLine("Задача с таким номером не найдена.");
            return;
        }

        statuses[index] = true; 
        dates[index] = DateTime.Now; 
        Console.WriteLine($"Задача отмечена выполненной: {todos[index]}");
    }

    //Обновляет текст задачи
    static void HandleUpdate(string command)
    {
        string[] parts = command.Split(' ', 3);

        if (parts.Length < 3)
        {
            Console.WriteLine("Использование: update <номер> \"новый текст\"");
            return;
        }

        int index = GetTaskIndex(parts[1]);

        if (index == -1)
        {
            Console.WriteLine("Задача с таким номером не найдена.");
            return;
        }

        string newText = parts[2].Trim().Trim('"');

        if (newText == "")
        {
            Console.WriteLine("Использование: update <номер> \"новый текст\"");
            return;
        }

        todos[index] = newText; 
        dates[index] = DateTime.Now; 
        Console.WriteLine($"Задача обновлена: {todos[index]}");
    }

    //Удаляет задачу по номеру
    static void HandleDelete(string command)
    {
        string[] parts = command.Split(' ');

        if (parts.Length < 2)
        {
            Console.WriteLine("Использование: delete <номер задачи>");
            return;
        }

        int index = GetTaskIndex(parts[1]);

        if (index == -1)
        {
            Console.WriteLine("Задача с таким номером не найдена.");
            return;
        }

        Console.WriteLine($"Задача удалена: {todos[index]}");

        for (int i = index; i < todoCount - 1; i++)
        {
            todos[i] = todos[i + 1];
            statuses[i] = statuses[i + 1];
            dates[i] = dates[i + 1];
        }

        todoCount--;
    }

    //Расширение массивов

    static void EnsureCapacity()
    {
        if (todoCount < todos.Length)
            return; 

        int newCapacity = todos.Length * 2;
        Console.WriteLine($"(массивы расширены до {newCapacity} элементов)");

        //Создаём новые, более длинные массивы и копируем старые значения
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

    //Общая логика для команд с номером задачи

    static int GetTaskIndex(string text)
    {
        if (!int.TryParse(text, out int number))
            return -1;

        int index = number - 1;

        if (index < 0 || index >= todoCount)
            return -1;

        return index;
    }
}