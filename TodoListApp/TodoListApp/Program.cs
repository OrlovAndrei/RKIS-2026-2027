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
    else
    {
        Console.WriteLine("Неизвестная команда. Введите help для списка команд.");
    }
}

Console.WriteLine("Программа завершена. До свидания!");