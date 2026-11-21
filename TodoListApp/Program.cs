Console.WriteLine("Практическая работа №2. Добавление пользователя.");
Console.WriteLine("Работу выполнили Геворгян Давид и Вадим Шкутко, группа 3833.");

Console.Write("Введите имя: ");
var firstName = Console.ReadLine()?.Trim();

Console.Write("Введите фамилию: ");
var lastName = Console.ReadLine()?.Trim();

if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
{
    Console.WriteLine("Имя и фамилия не должны быть пустыми.");
    return;
}

Console.Write("Введите год рождения: ");
var birthYearInput = Console.ReadLine();
var currentYear = DateTime.Today.Year;

if (!int.TryParse(birthYearInput, out var birthYear) || birthYear < 1 || birthYear > currentYear)
{
    Console.WriteLine($"Введите целый год рождения от 1 до {currentYear}.");
    return;
}

var age = currentYear - birthYear;
Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");
