using System; // Подключаем основные функции C#.
using System.Collections.Generic; // Подключаем коллекции HashSet и List.

// Выводим информацию об авторах программы.
Console.WriteLine("Работу выполнили Гонтарева Юлия и Рыжих Вероника");
Console.WriteLine(); // Печатаем пустую строку.

// Запрашиваем имя пользователя.
Console.Write("Введите имя: ");
string firstName = Console.ReadLine() ?? ""; // Считываем имя или используем пустую строку.

// Запрашиваем фамилию пользователя.
Console.Write("Введите фамилию: ");
string lastName = Console.ReadLine() ?? ""; // Считываем фамилию или используем пустую строку.

// Устанавливаем минимально допустимый год рождения.
const int MinBirthYear = 1900;
int birthYear; // Объявляем переменную для года рождения.

// Повторяем ввод, пока год не станет корректным.
while (true)
{
    Console.Write("Введите год рождения: "); // Запрашиваем год.

    if (int.TryParse(Console.ReadLine(), out birthYear) // Проверяем, удалось ли получить целое число.
        && birthYear >= MinBirthYear // Проверяем минимальный год.
        && birthYear <= DateTime.Now.Year) // Не допускаем год из будущего.
    {
        break; // Завершаем цикл при правильном вводе.
    }

    Console.WriteLine("Ошибка. Введите корректный год рождения."); // Сообщаем об ошибке.
}

// Создаём объект профиля пользователя.
Profile profile = new Profile(firstName, lastName, birthYear);

// Создаём объект списка задач.
TodoList todoList = new TodoList();

// Выводим информацию о пользователе.
Console.WriteLine(profile.GetInfo());
Console.WriteLine("Введите help, чтобы увидеть список команд.");

// Объявляем константы названий команд.
const string CommandHelp = "help"; // Команда справки.
const string CommandProfile = "profile"; // Команда просмотра профиля.
const string CommandAdd = "add"; // Команда добавления задачи.
const string CommandView = "view"; // Команда просмотра задач.
const string CommandDone = "done"; // Команда выполнения задачи.
const string CommandDelete = "delete"; // Команда удаления задачи.
const string CommandUpdate = "update"; // Команда изменения задачи.
const string CommandRead = "read"; // Команда чтения задачи.
const string CommandExit = "exit"; // Команда завершения программы.

// Метод выводит список доступных команд.
void ShowHelp()
{
    Console.WriteLine("help    - список команд");
    Console.WriteLine("profile - данные пользователя");
    Console.WriteLine("add \"текст\" - добавить задачу");
    Console.WriteLine("add --multiline (-m) - многострочная задача, конец: !end");
    Console.WriteLine("view - показать задачи");
    Console.WriteLine("view --index/-i --status/-s --update-date/-d");
    Console.WriteLine("view --all/-a - показать все столбцы");
    Console.WriteLine("read <номер> - полная информация о задаче");
    Console.WriteLine("done <номер> - выполнить задачу");
    Console.WriteLine("update <номер> \"новый текст\" - изменить задачу");
    Console.WriteLine("delete <номер> - удалить задачу");
    Console.WriteLine("exit - завершить программу");
}

// Метод разбирает флаги команды.
HashSet<string> ParseFlags(string commandArgs)
{
    HashSet<string> flags = new HashSet<string>(); // Создаём набор найденных флагов.

    string flagsPart = commandArgs.Split('"')[0]; // Берём часть команды до кавычек.

    foreach (string token in flagsPart.Split(' ', StringSplitOptions.RemoveEmptyEntries)) // Разделяем аргументы по пробелам.
    {
        if (token.StartsWith("--")) // Проверяем длинный флаг.
        {
            flags.Add(token.Substring(2).ToLower()); // Сохраняем флаг без двух дефисов.
        }
        else if (token.StartsWith("-")) // Проверяем короткий флаг.
        {
            foreach (char symbol in token.Substring(1)) // Разбираем каждый символ короткого флага.
            {
                flags.Add(char.ToLower(symbol).ToString()); // Сохраняем символ как отдельный флаг.
            }
        }
    }

    return flags; // Возвращаем найденные флаги.
}

// Метод проверяет наличие длинного или короткого флага.
bool HasFlag(HashSet<string> flags, string longName, string shortName)
{
    return flags.Contains(longName) || flags.Contains(shortName); // Возвращаем результат проверки.
}

// Метод считывает многострочную задачу.
string ReadMultilineText()
{
    List<string> lines = new List<string>(); // Создаём список строк задачи.

    Console.WriteLine("Вводите строки. Для завершения введите !end");

    while (true) // Повторяем ввод строк.
    {
        Console.Write("> "); // Показываем приглашение ввода.

        string? line = Console.ReadLine(); // Считываем очередную строку.

        if (line == null || line.Trim() == "!end") // Проверяем конец ввода.
        {
            break; // Завершаем цикл.
        }

        lines.Add(line); // Добавляем строку в список.
    }

    return string.Join("\n", lines); // Объединяем строки переносами строк.
}

// Метод создаёт и добавляет новую задачу.
void AddTask(string commandArgs)
{
    HashSet<string> flags = ParseFlags(commandArgs); // Получаем флаги команды.
    string text; // Объявляем переменную для текста задачи.

    if (HasFlag(flags, "multiline", "m")) // Проверяем режим многострочного ввода.
    {
        text = ReadMultilineText(); // Считываем несколько строк.
    }
    else // Если используется обычный ввод.
    {
        string[] parts = commandArgs.Split('"'); // Разделяем аргументы по кавычкам.

        if (parts.Length < 3 || parts[1].Trim() == "") // Проверяем формат и наличие текста.
        {
            Console.WriteLine("Формат: add \"текст задачи\""); // Показываем правильный формат.
            return; // Прерываем добавление.
        }

        text = parts[1]; // Получаем текст между кавычками.
    }

    if (text.Trim() == "") // Проверяем, что текст не пустой.
    {
        Console.WriteLine("Ошибка. Задача пустая."); // Сообщаем об ошибке.
        return; // Прерываем добавление.
    }

    TodoItem item = new TodoItem(text); // Создаём объект задачи.
    todoList.Add(item); // Передаём задачу списку.
}

// Метод получает задачу по введённому номеру.
TodoItem? GetTask(string commandArgs)
{
    if (!int.TryParse(commandArgs.Trim(), out int index)) // Проверяем, является ли аргумент числом.
    {
        Console.WriteLine("Ошибка. Укажите номер задачи."); // Сообщаем об ошибке ввода.
        return null; // Возвращаем отсутствие задачи.
    }

    return todoList.GetItem(index); // Получаем задачу из списка.
}

// Метод отмечает задачу выполненной.
void DoneTask(string commandArgs)
{
    TodoItem? item = GetTask(commandArgs); // Получаем задачу по номеру.

    if (item == null) // Проверяем, найдена ли задача.
    {
        return; // Прерываем выполнение при ошибке.
    }

    item.MarkDone(); // Меняем статус задачи через метод класса.
    Console.WriteLine("Задача отмечена выполненной."); // Подтверждаем выполнение.
}

// Метод изменяет текст существующей задачи.
void UpdateTask(string commandArgs)
{
    string[] parts = commandArgs.Split('"'); // Отделяем номер от нового текста.

    if (parts.Length < 3 || parts[1].Trim() == "") // Проверяем формат команды.
    {
        Console.WriteLine("Формат: update <номер> \"новый текст\""); // Показываем правильный формат.
        return; // Прерываем изменение.
    }

    TodoItem? item = GetTask(parts[0]); // Получаем задачу по номеру.

    if (item == null) // Проверяем, существует ли задача.
    {
        return; // Прерываем изменение при ошибке.
    }

    item.UpdateText(parts[1]); // Изменяем текст через метод класса.
    Console.WriteLine("Задача обновлена."); // Подтверждаем изменение.
}

// Метод удаляет задачу из списка.
void DeleteTask(string commandArgs)
{
    if (!int.TryParse(commandArgs.Trim(), out int index)) // Проверяем правильность номера.
    {
        Console.WriteLine("Ошибка. Укажите номер задачи."); // Сообщаем об ошибке.
        return; // Прерываем удаление.
    }

    todoList.Delete(index); // Передаём номер методу удаления.
}

// Метод выводит полную информацию о задаче.
void ReadTask(string commandArgs)
{
    TodoItem? item = GetTask(commandArgs); // Получаем задачу по номеру.

    if (item == null) // Проверяем, найдена ли задача.
    {
        return; // Прерываем вывод при ошибке.
    }

    Console.WriteLine(item.GetFullInfo()); // Выводим информацию методом класса.
}

// Метод отображает таблицу задач с выбранными столбцами.
void ViewTasks(string commandArgs)
{
    HashSet<string> flags = ParseFlags(commandArgs); // Получаем флаги отображения.

    bool showAll = HasFlag(flags, "all", "a"); // Проверяем запрос всех столбцов.

    bool showIndex = showAll || HasFlag(flags, "index", "i"); // Определяем, показывать ли номера.
    bool showDone = showAll || HasFlag(flags, "status", "s"); // Определяем, показывать ли статусы.
    bool showDate = showAll || HasFlag(flags, "update-date", "d"); // Определяем, показывать ли даты.

    todoList.View(showIndex, showDone, showDate); // Передаём настройки отображения списку.
}

// Основной цикл принимает команды пользователя.
while (true)
{
    Console.Write("> "); // Выводим приглашение для команды.

    string input = (Console.ReadLine() ?? "").Trim(); // Считываем команду и убираем пробелы по краям.

    if (input == "") // Проверяем, не введена ли пустая строка.
    {
        continue; // Переходим к следующему вводу.
    }

    string[] commandParts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries); // Разделяем команду и аргументы.

    string command = commandParts[0].ToLower(); // Получаем название команды.

    string commandArgs = commandParts.Length > 1 // Проверяем, есть ли аргументы.
        ? commandParts[1].Trim() // Получаем аргументы команды.
        : ""; // Если аргументов нет, используем пустую строку.

    switch (command) // Выбираем действие по названию команды.
    {
        case CommandHelp: // Если введена команда справки.
            ShowHelp(); // Показываем список команд.
            break; // Завершаем обработку команды.

        case CommandProfile: // Если запрошен профиль.
            Console.WriteLine(profile.GetInfo()); // Выводим данные пользователя.
            break; // Завершаем обработку команды.

        case CommandAdd: // Если нужно добавить задачу.
            AddTask(commandArgs); // Вызываем метод добавления.
            break; // Завершаем обработку команды.

        case CommandView: // Если нужно показать список задач.
            ViewTasks(commandArgs); // Вызываем метод отображения.
            break; // Завершаем обработку команды.

        case CommandDone: // Если нужно выполнить задачу.
            DoneTask(commandArgs); // Вызываем метод изменения статуса.
            break; // Завершаем обработку команды.

        case CommandDelete: // Если нужно удалить задачу.
            DeleteTask(commandArgs); // Вызываем метод удаления.
            break; // Завершаем обработку команды.

        case CommandUpdate: // Если нужно изменить текст задачи.
            UpdateTask(commandArgs); // Вызываем метод изменения.
            break; // Завершаем обработку команды.

        case CommandRead: // Если нужно прочитать задачу.
            ReadTask(commandArgs); // Выводим полную информацию.
            break; // Завершаем обработку команды.

        case CommandExit: // Если пользователь завершает программу.
            Console.WriteLine("Завершение работы."); // Выводим сообщение.
            return; // Завершаем программу.

        default: // Если команда не распознана.
            Console.WriteLine("Неизвестная команда. Введите help."); // Сообщаем об ошибке.
            break; // Возвращаемся к вводу команды.
    }
}