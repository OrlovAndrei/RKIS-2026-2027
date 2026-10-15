var profile = Register();
var todoList = new TodoList();
Console.WriteLine("Введите help, чтобы увидеть список команд.");

// Главный цикл: строку разбирает CommandParser, а выполняет полученная команда
while (true)
{
    Console.Write("> ");
    string? input = Console.ReadLine();
    if (input is null) // ввод закрыт (Ctrl+D / Ctrl+Z) — завершаем работу
        break;
    if (string.IsNullOrWhiteSpace(input))
        continue;

    ICommand command = CommandParser.Parse(input, todoList, profile);
    command.Execute();
}


// Регистрация пользователя: спрашиваем имя, фамилию и год рождения
Profile Register()
{
    string firstName = ReadNotEmpty("Введите имя: ");
    string lastName = ReadNotEmpty("Введите фамилию: ");

    int currentYear = DateTime.Now.Year;
    int birthYear;
    while (true)
    {
        Console.Write("Введите год рождения: ");
        string? line = Console.ReadLine();

        // TryParse не бросает исключение на некорректной строке, в отличие от int.Parse
        if (int.TryParse(line, out birthYear) && birthYear >= 1900 && birthYear <= currentYear)
            break;

        Console.WriteLine($"Год рождения должен быть числом от 1900 до {currentYear}!");
    }

    var newProfile = new Profile(firstName, lastName, birthYear);
    Console.WriteLine($"Добавлен пользователь {newProfile.GetInfo()}");
    return newProfile;
}

// Повторяем вопрос, пока пользователь не введёт непустую строку
string ReadNotEmpty(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        // ReadLine вернёт null, если ввод закрыт — тогда выходим из программы
        string? line = Console.ReadLine();
        if (line is null)
            Environment.Exit(0);

        line = line.Trim();
        if (line.Length > 0)
            return line;
    }
}
