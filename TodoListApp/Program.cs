Console.WriteLine("Работу выполнили Кузнецов и Курбатов");

Console.Write("Введите имя: ");
string firstName = Console.ReadLine() ?? "";

Console.Write("Введите фамилию: ");
string lastName = Console.ReadLine() ?? "";

Console.Write("Введите год рождения: ");
int birthYear = int.Parse(Console.ReadLine() ?? "0");

int age = DateTime.Now.Year - birthYear;

Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");