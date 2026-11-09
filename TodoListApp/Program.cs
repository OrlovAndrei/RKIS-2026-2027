using System; // Базовые типы .NET
using System.IO; // Работа с файлами и путями

// Выводим информацию об авторах программы.
Console.WriteLine("Работу выполнили Гонтарева Юлия и Рыжих Вероника");
Console.WriteLine();

// Определяем папку и пути к файлам с данными.
string dataDirectory = Path.Combine(
    AppDomain.CurrentDomain.BaseDirectory,
    "data"
);

FileManager.EnsureDataDirectory(dataDirectory);

string profileFilePath = Path.Combine(dataDirectory, "profile.txt");
string todoFilePath = Path.Combine(dataDirectory, "todo.csv");

// Загружаем профиль или создаём его при первом запуске.
Profile profile;

if (File.Exists(profileFilePath))
{
    profile = FileManager.LoadProfile(profileFilePath);
}
else
{
    Console.Write("Введите имя: ");
    string firstName = Console.ReadLine() ?? "";

    Console.Write("Введите фамилию: ");
    string lastName = Console.ReadLine() ?? "";

    const int MinBirthYear = 1900;
    int birthYear;

    while (true)
    {
        Console.Write("Введите год рождения: ");

        if (int.TryParse(Console.ReadLine(), out birthYear)
            && birthYear >= MinBirthYear
            && birthYear <= DateTime.Now.Year)
        {
            break;
        }

        Console.WriteLine("Ошибка. Введите корректный год рождения.");
    }

    profile = new Profile(firstName, lastName, birthYear);
    FileManager.SaveProfile(profile, profileFilePath);
}

// Загружаем список задач или создаём новый.
TodoList todoList;

if (File.Exists(todoFilePath))
{
    todoList = FileManager.LoadTodos(todoFilePath);
}
else
{
    todoList = new TodoList();
    FileManager.SaveTodos(todoList, todoFilePath);
}

// Выводим информацию о пользователе.
Console.WriteLine(profile.GetInfo());
Console.WriteLine("Введите help, чтобы увидеть список команд.");

// Основной цикл принимает команды пользователя.
while (true)
{
    Console.Write("> ");

    // Считываем команду и удаляем пробелы по краям.
    string input = (Console.ReadLine() ?? "").Trim();

    // Пустой ввод пропускаем.
    if (input == "")
    {
        continue;
    }

    // Команду выхода обрабатываем прямо в цикле.
    if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("Завершение работы.");
        return;
    }

    // Разбираем строку и получаем объект команды.
    ICommand? command = CommandParser.Parse(input, todoList, profile);

    // Если команда не распознана, возвращаемся к вводу.
    if (command == null)
    {
        continue;
    }

    // Выполняем команду.
    command.Execute();

    // Сохраняем список, если команда его изменила.
    if (command is AddCommand or UpdateCommand or DeleteCommand or StatusCommand)
    {
        FileManager.SaveTodos(todoList, todoFilePath);
    }
}