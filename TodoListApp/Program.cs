Console.WriteLine("Работу выполнили Капинус Илья Алексеевич и Халлыев Тимур Константинович.");

Console.Write("Введите имя: ");
string firstName = Console.ReadLine()!;

Console.Write("Введите фамилию: ");
string lastName = Console.ReadLine()!;

Console.Write("Введите год рождения: ");
int birthYear = int.Parse(Console.ReadLine()!);

int age = DateTime.Now.Year - birthYear;
Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");
