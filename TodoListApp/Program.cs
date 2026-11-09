using System; // Подключаем базовые типы .NET (Console, DateTime, StringSplitOptions и др.)
using System.IO; // Подключаем классы для работы с файлами и путями (File, Path)
using System.Collections.Generic; // Подключаем обобщённые коллекции (List, HashSet)

// Выводим информацию об авторах программы.
Console.WriteLine("Работу выполнили Гонтарева Юлия и Рыжих Вероника"); // Печатаем строку с авторами
Console.WriteLine(); // Печатаем пустую строку для отступа

// Определяем папку и пути к файлам с данными.
string dataDirectory = Path.Combine( // Собираем путь к папке data, учитывая разделители ОС
    AppDomain.CurrentDomain.BaseDirectory, // Базовая папка, из которой запущена программа
    "data" // Имя вложенной папки с данными
); // Конец вызова Path.Combine

FileManager.EnsureDataDirectory(dataDirectory); // Создаём папку data, если её ещё нет

string profileFilePath = Path.Combine(dataDirectory, "profile.txt"); // Путь к файлу с данными профиля
string todoFilePath = Path.Combine(dataDirectory, "todo.csv"); // Путь к файлу со списком задач

// Загружаем профиль или создаём его при первом запуске.
Profile profile; // Объявляем переменную профиля (значение присвоим ниже)

if (File.Exists(profileFilePath)) // Если файл профиля уже существует (не первый запуск)
{
    profile = FileManager.LoadProfile(profileFilePath); // Читаем профиль из файла
}
else // Иначе это первый запуск, профиля ещё нет
{
    // Запрашиваем имя пользователя.
    Console.Write("Введите имя: "); // Выводим приглашение без перевода строки
    string firstName = Console.ReadLine() ?? ""; // Читаем имя; если ввод null, берём пустую строку

    // Запрашиваем фамилию пользователя.
    Console.Write("Введите фамилию: "); // Выводим приглашение для фамилии
    string lastName = Console.ReadLine() ?? ""; // Читаем фамилию; null заменяем пустой строкой

    // Устанавливаем минимально допустимый год рождения.
    const int MinBirthYear = 1900; // Константа: раньше этого года рождение считается ошибкой
    int birthYear; // Переменная для года рождения (заполнится в TryParse)

    // Повторяем ввод, пока год не станет корректным.
    while (true) // Бесконечный цикл, выходим из него через break
    {
        Console.Write("Введите год рождения: "); // Выводим приглашение для года рождения

        if (int.TryParse(Console.ReadLine(), out birthYear) // Пробуем преобразовать ввод в число
            && birthYear >= MinBirthYear // И год не меньше минимально допустимого
            && birthYear <= DateTime.Now.Year) // И год не больше текущего
        {
            break; // Ввод корректен, выходим из цикла
        }

        Console.WriteLine("Ошибка. Введите корректный год рождения."); // Сообщаем об ошибке и повторяем ввод
    }

    // Создаём профиль и сразу сохраняем его.
    profile = new Profile(firstName, lastName, birthYear); // Создаём объект профиля из введённых данных
    FileManager.SaveProfile(profile, profileFilePath); // Записываем профиль в файл, чтобы не вводить заново
}

// Загружаем список задач или создаём новый.
TodoList todoList; // Объявляем переменную списка задач

if (File.Exists(todoFilePath)) // Если файл с задачами уже существует
{
    todoList = FileManager.LoadTodos(todoFilePath); // Загружаем задачи из файла
}
else // Иначе файла с задачами ещё нет
{
    todoList = new TodoList(); // Создаём пустой список задач
    FileManager.SaveTodos(todoList, todoFilePath); // Сразу сохраняем пустой список в файл
}

// Выводим информацию о пользователе.
Console.WriteLine(profile.GetInfo()); // Печатаем имя, фамилию и возраст пользователя
Console.WriteLine("Введите help, чтобы увидеть список команд."); // Подсказываем, как узнать команды

// Объявляем константы названий команд.
const string CommandHelp = "help"; // Название команды справки
const string CommandProfile = "profile"; // Название команды просмотра профиля
const string CommandAdd = "add"; // Название команды добавления задачи
const string CommandView = "view"; // Название команды просмотра списка
const string CommandStatus = "status"; // Название команды изменения статуса
const string CommandDelete = "delete"; // Название команды удаления задачи
const string CommandUpdate = "update"; // Название команды изменения текста задачи
const string CommandRead = "read"; // Название команды чтения полной информации
const string CommandExit = "exit"; // Название команды выхода

// Метод выводит список доступных команд.
void ShowHelp() // Локальный метод без параметров и без возвращаемого значения
{
    Console.WriteLine("help    - список команд"); // Описание команды help
    Console.WriteLine("profile - данные пользователя"); // Описание команды profile
    Console.WriteLine("add \"текст\" - добавить задачу"); // Описание add (\" — экранированная кавычка)
    Console.WriteLine("add --multiline (-m) - многострочная задача, конец: !end"); // Описание многострочного add
    Console.WriteLine("view - показать задачи"); // Описание команды view
    Console.WriteLine("view --index/-i --status/-s --update-date/-d"); // Флаги столбцов для view
    Console.WriteLine("view --all/-a - показать все столбцы"); // Флаг показа всех столбцов
    Console.WriteLine("read <номер> - полная информация о задаче"); // Описание команды read
    Console.WriteLine("status <номер> <статус> - изменить статус (notstarted, inprogress, completed, postponed, failed)");
    Console.WriteLine("update <номер> \"новый текст\" - изменить задачу"); // Описание команды update
    Console.WriteLine("delete <номер> - удалить задачу"); // Описание команды delete
    Console.WriteLine("exit - завершить программу"); // Описание команды exit
} // Конец метода ShowHelp

// Метод разбирает флаги команды.
HashSet<string> ParseFlags(string commandArgs) // Принимает строку аргументов, возвращает набор флагов
{
    HashSet<string> flags = new HashSet<string>(); // Создаём пустое множество (дубликаты не хранятся)

    // Берём часть команды до кавычек.
    string flagsPart = commandArgs.Split('"')[0]; // Делим по кавычке и берём текст до первой кавычки

    // Разделяем аргументы по пробелам.
    foreach (string token in flagsPart.Split( // Перебираем каждый «кусочек» строки
        ' ', // Разделитель — пробел
        StringSplitOptions.RemoveEmptyEntries)) // Пустые элементы (двойные пробелы) отбрасываем
    {
        if (token.StartsWith("--")) // Если кусочек начинается с двух дефисов — это длинный флаг
        {
            // Сохраняем длинный флаг без двух дефисов.
            flags.Add(token.Substring(2).ToLower()); // Отрезаем "--", приводим к нижнему регистру, добавляем
        }
        else if (token.StartsWith("-")) // Если начинается с одного дефиса — это короткие флаги
        {
            // Разбираем каждый символ короткого флага.
            foreach (char symbol in token.Substring(1)) // Перебираем буквы после дефиса (например, в "-is")
            {
                flags.Add(char.ToLower(symbol).ToString()); // Каждую букву в нижнем регистре добавляем как флаг
            }
        }
    }

    return flags; // Возвращаем собранный набор флагов
} // Конец метода ParseFlags

// Метод проверяет наличие длинного или короткого флага.
bool HasFlag(HashSet<string> flags, string longName, string shortName) // Принимает набор и два имени флага
{
    return flags.Contains(longName) || flags.Contains(shortName); // true, если есть длинное или короткое имя
} // Конец метода HasFlag

// Метод считывает многострочную задачу.
string ReadMultilineText() // Возвращает весь введённый текст одной строкой
{
    List<string> lines = new List<string>(); // Список для накопления введённых строк

    Console.WriteLine("Вводите строки. Для завершения введите !end"); // Объясняем, как закончить ввод

    while (true) // Читаем строки, пока пользователь не завершит ввод
    {
        Console.Write("> "); // Приглашение к вводу очередной строки

        string? line = Console.ReadLine(); // Читаем строку (может быть null, поэтому тип string?)

        if (line == null || line.Trim() == "!end") // Если ввод закончился или введена команда !end
        {
            break; // Прекращаем чтение
        }

        lines.Add(line); // Добавляем введённую строку в список
    }

    // Объединяем введённые строки переносами.
    return string.Join("\n", lines); // Склеиваем строки через символ перевода строки и возвращаем
} // Конец метода ReadMultilineText

// Метод создаёт и добавляет новую задачу.
void AddTask(string commandArgs) // Принимает аргументы команды add
{
    HashSet<string> flags = ParseFlags(commandArgs); // Получаем флаги из аргументов
    string text; // Переменная для текста задачи

    // Проверяем режим многострочного ввода.
    if (HasFlag(flags, "multiline", "m")) // Если указан флаг --multiline или -m
    {
        text = ReadMultilineText(); // Читаем текст из нескольких строк
    }
    else // Иначе ожидаем текст в кавычках
    {
        // Разделяем аргументы по кавычкам.
        string[] parts = commandArgs.Split('"'); // Для add "текст" получится ["", "текст", ""]

        if (parts.Length < 3 || parts[1].Trim() == "") // Если кавычек нет/не хватает или текст пустой
        {
            Console.WriteLine("Формат: add \"текст задачи\""); // Подсказываем правильный формат
            return; // Выходим из метода без добавления
        }

        text = parts[1]; // Берём текст между кавычками
    }

    // Проверяем, что текст задачи не пустой.
    if (text.Trim() == "") // Если после удаления пробелов ничего не осталось
    {
        Console.WriteLine("Ошибка. Задача пустая."); // Сообщаем об ошибке
        return; // Выходим без добавления
    }

    // Создаём и добавляем задачу.
    TodoItem item = new TodoItem(text); // Создаём объект задачи с этим текстом
    todoList.Add(item); // Добавляем задачу в список

    // Сохраняем список после добавления задачи.
    FileManager.SaveTodos(todoList, todoFilePath); // Записываем обновлённый список в файл

    Console.WriteLine("Задача добавлена."); // Подтверждаем добавление
} // Конец метода AddTask

// Метод получает задачу по введённому номеру.
TodoItem? GetTask(string commandArgs) // Возвращает задачу или null, если номер некорректен
{
    if (!int.TryParse(commandArgs.Trim(), out int index)) // Пробуем получить число из строки аргументов
    {
        Console.WriteLine("Ошибка. Укажите номер задачи."); // Если не число — сообщаем об ошибке
        return null; // Возвращаем null как признак неудачи
    }

    return todoList.GetItem(index); // Возвращаем задачу с этим номером из списка
} // Конец метода GetTask

// Метод разбирает команду status и меняет статус задачи.
void StatusTask(string commandArgs)
{
    // Делим аргументы на номер и статус.
    string[] parts = commandArgs.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    if (parts.Length < 2)
    {
        Console.WriteLine("Формат: status <номер> <статус>");
        return;
    }

    if (!int.TryParse(parts[0], out int index))
    {
        Console.WriteLine("Ошибка. Укажите номер задачи.");
        return;
    }

    // Преобразуем строку в enum без учёта регистра.
    if (!Enum.TryParse<TodoStatus>(parts[1], true, out TodoStatus status) || !Enum.IsDefined(status))
    {
        Console.WriteLine("Неизвестный статус. Доступны: notstarted, inprogress, completed, postponed, failed.");
        return;
    }

    StatusCommand command = new StatusCommand
    {
        TodoList = todoList,
        Index = index,
        Status = status
    };

    command.Execute();

    FileManager.SaveTodos(todoList, todoFilePath); // Сохраняем изменения.
}

// Метод изменяет текст существующей задачи.
void UpdateTask(string commandArgs) // Принимает аргументы команды update
{
    // Отделяем номер задачи от нового текста.
    string[] parts = commandArgs.Split('"'); // Для 1 "текст" получится ["1 ", "текст", ""]

    if (parts.Length < 3 || parts[1].Trim() == "") // Если нет кавычек или новый текст пустой
    {
        Console.WriteLine("Формат: update <номер> \"новый текст\""); // Подсказываем формат команды
        return; // Выходим без изменений
    }

    // Получаем задачу по номеру.
    TodoItem? item = GetTask(parts[0]); // Часть до кавычки — это номер задачи

    if (item == null) // Если задача не найдена
    {
        return; // Прекращаем выполнение
    }

    // Изменяем текст задачи.
    item.UpdateText(parts[1]); // Записываем новый текст и обновляем дату изменения

    // Сохраняем изменения.
    FileManager.SaveTodos(todoList, todoFilePath); // Записываем список в файл

    Console.WriteLine("Задача обновлена."); // Подтверждаем изменение
} // Конец метода UpdateTask

// Метод удаляет задачу из списка.
void DeleteTask(string commandArgs) // Принимает аргументы команды delete
{
    if (!int.TryParse(commandArgs.Trim(), out int index)) // Пробуем получить номер задачи из строки
    {
        Console.WriteLine("Ошибка. Укажите номер задачи."); // Если не число — сообщаем об ошибке
        return; // Выходим без удаления
    }

    // Удаляем задачу.
    todoList.Delete(index); // Удаляем задачу по номеру и сдвигаем остальные

    // Сохраняем актуальный список задач.
    FileManager.SaveTodos(todoList, todoFilePath); // Записываем список в файл
} // Конец метода DeleteTask

// Метод выводит полную информацию о задаче.
void ReadTask(string commandArgs) // Принимает аргументы команды read
{
    TodoItem? item = GetTask(commandArgs); // Получаем задачу по номеру (может быть null)

    if (item == null) // Если задача не найдена
    {
        return; // Прекращаем выполнение
    }

    Console.WriteLine(item.GetFullInfo()); // Печатаем полный текст, статус и дату изменения
} // Конец метода ReadTask

// Метод отображает таблицу задач с выбранными столбцами.
void ViewTasks(string commandArgs) // Принимает аргументы команды view (флаги)
{
    HashSet<string> flags = ParseFlags(commandArgs); // Получаем набор флагов из аргументов

    // Проверяем, запрошены ли все столбцы.
    bool showAll = HasFlag(flags, "all", "a"); // true, если указан --all или -a

    bool showIndex = showAll || HasFlag(flags, "index", "i"); // Показывать номера: при --all или --index/-i
    bool showDone = showAll || HasFlag(flags, "status", "s"); // Показывать статусы: при --all или --status/-s
    bool showDate = showAll || HasFlag(flags, "update-date", "d"); // Показывать даты: при --all или --update-date/-d

    // Передаём настройки отображения списку задач.
    todoList.View(showIndex, showDone, showDate); // Список сам выводит таблицу с нужными столбцами
} // Конец метода ViewTasks

// Основной цикл принимает команды пользователя.
while (true) // Бесконечный цикл: программа работает до команды exit
{
    Console.Write("> "); // Выводим приглашение к вводу команды

    // Считываем команду и удаляем пробелы по краям.
    string input = (Console.ReadLine() ?? "").Trim(); // null заменяем пустой строкой, обрезаем пробелы

    if (input == "") // Если пользователь ничего не ввёл
    {
        continue; // Пропускаем итерацию и снова запрашиваем ввод
    }

    // Разделяем команду и её аргументы.
    string[] commandParts = input.Split( // Делим строку на название команды и остальное
        ' ', // Разделитель — пробел
        2, // Максимум 2 части: команда и всё остальное целиком
        StringSplitOptions.RemoveEmptyEntries // Пустые части отбрасываем
    ); // Конец вызова Split

    string command = commandParts[0].ToLower(); // Название команды в нижнем регистре (Add = add)

    string commandArgs = commandParts.Length > 1 // Если после команды есть что-то ещё...
        ? commandParts[1].Trim() // ...берём это как аргументы, обрезав пробелы
        : ""; // ...иначе аргументов нет — пустая строка

    // Выбираем действие по названию команды.
    switch (command) // Сравниваем команду с известными вариантами
    {
        case CommandHelp: // Если введено help
            ShowHelp(); // Показываем справку
            break; // Выходим из switch

        case CommandProfile: // Если введено profile
            // Выводим сохранённый профиль пользователя.
            Console.WriteLine(profile.GetInfo()); // Печатаем данные пользователя
            break; // Выходим из switch

        case CommandAdd: // Если введено add
            AddTask(commandArgs); // Добавляем задачу
            break; // Выходим из switch

        case CommandView: // Если введено view
            ViewTasks(commandArgs); // Показываем таблицу задач
            break; // Выходим из switch

        case CommandStatus: // Если введено status
            StatusTask(commandArgs); // Меняем статус задачи
            break; // Выходим из switch

        case CommandDelete: // Если введено delete
            DeleteTask(commandArgs); // Удаляем задачу
            break; // Выходим из switch

        case CommandUpdate: // Если введено update
            UpdateTask(commandArgs); // Изменяем текст задачи
            break; // Выходим из switch

        case CommandRead: // Если введено read
            ReadTask(commandArgs); // Показываем полную информацию о задаче
            break; // Выходим из switch

        case CommandExit: // Если введено exit
            Console.WriteLine("Завершение работы."); // Прощальное сообщение
            return; // Завершаем программу (выход из верхнеуровневого кода)

        default: // Если команда не совпала ни с одной из известных
            Console.WriteLine("Неизвестная команда. Введите help."); // Сообщаем, что команда неизвестна
            break; // Выходим из switch
    }
}