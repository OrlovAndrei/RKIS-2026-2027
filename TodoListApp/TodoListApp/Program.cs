// TodoListApp — практические задания к Лекциям 2–6
// Лекция 6 «Введение в ООП» — список задач переписан на классы TodoItem и TodoList
// Работу выполнили: Талаева и Хрустель, группа 2121у

class Program
{
    // Поля класса 

    // Данные пользователя — объект класса Profile
    static Profile profile = new Profile("", "", 0);

    // Список задач — объект класса TodoList
    static TodoList list = new TodoList();

    // Точка входа

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

            // Диспетчер
            if (command == "help")
                ShowHelp();
            else if (command == "profile")
                ShowProfile();
            else if (command == "view" || command.StartsWith("view "))
                HandleView(command);
            else if (command == "add --multiline" || command == "add -m")
                HandleAddMultiline();
            else if (command == "add" || command.StartsWith("add "))
                HandleAdd(command);
            else if (command == "done" || command.StartsWith("done "))
                HandleDone(command);
            else if (command == "update" || command.StartsWith("update "))
                HandleUpdate(command);
            else if (command == "delete" || command.StartsWith("delete "))
                HandleDelete(command);
            else if (command == "read" || command.StartsWith("read "))
                HandleRead(command);
            else
                Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
        }

        Console.WriteLine("Программа завершена. До свидания!");
    }

    // Пользователь

    // Читает данные пользователя при старте и создаёт объект Profile
    static void ReadUserInfo()
    {
        Console.Write("Введите имя: ");
        string name = Console.ReadLine()!;

        Console.Write("Введите фамилию: ");
        string surname = Console.ReadLine()!;

        Console.Write("Введите год рождения: ");
        int year = int.Parse(Console.ReadLine()!);

        // Вызывается конструктор Profile
        profile = new Profile(name, surname, year);

        Console.WriteLine($"Добавлен пользователь {profile.GetInfo()}");
    }

    // Profile.GetInfo() считает возраст

    static void ShowProfile()
    {
        Console.WriteLine(profile.GetInfo());
    }

    // Команды

    static void ShowHelp()
    {
        Console.WriteLine("Доступные команды:");
        Console.WriteLine("  help    — список команд");
        Console.WriteLine("  profile — данные пользователя");
        Console.WriteLine("  view [флаги] — показать задачи (-i индекс, -s статус, -d дата, -a всё)");
        Console.WriteLine("  add \"текст задачи\" — добавить задачу");
        Console.WriteLine("  add --multiline (или add -m) — многострочная задача, ввод до строки !end");
        Console.WriteLine("  done <номер> — отметить задачу выполненной");
        Console.WriteLine("  update <номер> \"новый текст\" — изменить текст задачи");
        Console.WriteLine("  delete <номер> — удалить задачу");
        Console.WriteLine("  read <номер> — показать задачу целиком (текст, статус, дата)");
        Console.WriteLine("  exit    — выход из программы");
    }

    // Показывает задачи. Флаги: --index/-i, --status/-s, --update-date/-d, --all/-a
    static void HandleView(string command)
    {
        if (list.Count == 0)
        {
            Console.WriteLine("Список задач пуст.");
            return;
        }

        bool showIndex = false;
        bool showStatus = false;
        bool showDate = false;

        // parts[0] — слово view, дальше — флаги
        string[] parts = command.Split(' ');

        for (int i = 1; i < parts.Length; i++)
        {
            string flag = parts[i];

            // Длинные флаги
            if (flag == "--index")
                showIndex = true;
            else if (flag == "--status")
                showStatus = true;
            else if (flag == "--update-date")
                showDate = true;
            else if (flag == "--all")
            {
                showIndex = true;
                showStatus = true;
                showDate = true;
            }
            // Короткие флаги и их комбинации
            else if (flag.StartsWith("-") && !flag.StartsWith("--") && flag.Length > 1)
            {
                foreach (char c in flag.Substring(1))
                {
                    if (c == 'i')
                        showIndex = true;
                    else if (c == 's')
                        showStatus = true;
                    else if (c == 'd')
                        showDate = true;
                    else if (c == 'a')
                    {
                        showIndex = true;
                        showStatus = true;
                        showDate = true;
                    }
                    else
                        Console.WriteLine($"Неизвестный короткий флаг -{c} (проигнорирован)");
                }
            }
            else
            {
                Console.WriteLine($"Неизвестный флаг: {flag} (проигнорирован)");
            }
        }

        // Шапка и строки таблицы печатаются классом TodoList
        list.View(showIndex, showStatus, showDate);
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

        // Задача — объект (в конструкторе TodoItem)
        TodoItem item = new TodoItem(taskText);

        list.Add(item);

        Console.WriteLine($"Задача добавлена: {taskText}");
    }

    // Многострочный режим
    static void HandleAddMultiline()
    {
        Console.WriteLine("Многострочный режим: вводите строки задачи. Для завершения введите !end");

        string result = "";

        while (true)
        {
            Console.Write("> ");
            string? line = Console.ReadLine();

            if (line == null || line.Trim() == "!end")
                break;

            // Перед каждой новой строкой добавляем перевод строки \n
            // (кроме самой первой — она идёт без разделителя).
            if (result != "")
                result += "\n";

            result += line;
        }

        if (result == "")
        {
            Console.WriteLine("Задача не добавлена: текст пустой.");
            return;
        }

        // Многострочный текст хранится в TodoItem
        list.Add(new TodoItem(result));

        Console.WriteLine($"Задача добавлена: {result}");
    }

    // Отмечает задачу выполненной: done <номер>
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

        // Задачу получаем из списка через GetItem, меняем только через её метод
        TodoItem item = list.GetItem(index);
        item.MarkDone();

        Console.WriteLine($"Задача отмечена выполненной: {item.GetShortInfo()}");
    }

    // Обновляет текст задачи
    static void HandleUpdate(string command)
    {
        // parts[0] — update, parts[1] — номер, parts[2] — весь новый текст
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

        TodoItem item = list.GetItem(index);
        item.UpdateText(newText);

        Console.WriteLine($"Задача обновлена: {item.Text}");
    }

    // Удаляет задачу по номеру
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

        TodoItem item = list.GetItem(index);

        Console.WriteLine($"Задача удалена: {item.Text}");

        // Сдвиг массива внутри TodoList.Delete
        list.Delete(index);
    }

    // read <номер>: показывает задачу целиком (без обрезки)
    static void HandleRead(string command)
    {
        string[] parts = command.Split(' ');

        if (parts.Length < 2)
        {
            Console.WriteLine("Использование: read <номер задачи>");
            return;
        }

        int index = GetTaskIndex(parts[1]);

        if (index == -1)
        {
            Console.WriteLine("Задача с таким номером не найдена.");
            return;
        }

        // Полное описание теперь собирает TodoItem
        Console.WriteLine(list.GetItem(index).GetFullInfo());
    }

    // Общая логика для команд с номером задачи

    static int GetTaskIndex(string text)
    {
        if (!int.TryParse(text, out int number)) 
            return -1;

        int index = number - 1; // на экране нумерация с 1, в массиве с 0

        if (index < 0 || index >= list.Count)
            return -1;

        return index;
    }
}