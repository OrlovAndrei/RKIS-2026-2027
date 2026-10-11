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
}