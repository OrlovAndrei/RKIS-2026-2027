Console.WriteLine("Работу выполнили Кузнецов и Курбатов");

Console.Write("Введите имя: ");
string firstName = Console.ReadLine() ?? "";

Console.Write("Введите фамилию: ");
string lastName = Console.ReadLine() ?? "";

Console.Write("Введите год рождения: ");
int birthYear = int.Parse(Console.ReadLine() ?? "0");

int age = DateTime.Now.Year - birthYear;

Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");
string[] todos = new string[2];
int todoCount = 0;
while (true)
{
    Console.Write("> ");
    string command = Console.ReadLine() ?? "";

    if (command == "help")
    {
        Console.WriteLine("Доступные команды:");
        Console.WriteLine("help - список команд");
        Console.WriteLine("profile - информация о пользователе");
        Console.WriteLine("add \"текст задачи\" - добавить задачу");
        Console.WriteLine("view - показать все задачи");
        Console.WriteLine("exit - выход из программы");
    }
        else if (command == "profile")
    {
        Console.WriteLine($"{firstName} {lastName}, {birthYear}");
    }
        else if (command == "exit")
    {
        Console.WriteLine("Выход из программы");
        break;
    }
    else if (command.StartsWith("add "))
{
    string[] parts = command.Split('"');

    if (parts.Length != 3 || string.IsNullOrWhiteSpace(parts[1]))
    {
        Console.WriteLine("Используйте формат: add \"текст задачи\"");
        continue;
    }

    string task = parts[1];

    if (todoCount == todos.Length)
    {
        string[] newTodos = new string[todos.Length * 2];

        for (int i = 0; i < todos.Length; i++)
        {
            newTodos[i] = todos[i];
        }

        todos = newTodos;
    }

    todos[todoCount] = task;
    todoCount++;

    Console.WriteLine("Задача добавлена");
}
}