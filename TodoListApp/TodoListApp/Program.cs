
Console.WriteLine(".:| TodoListApp |:.");
Console.WriteLine("Введите имя:");
string name = Console.ReadLine() ?? "";

Console.WriteLine("Введите фамилию:");
string surname = Console.ReadLine() ?? "";

Console.WriteLine("Введите год рождения:");
int birthYear = int.Parse(Console.ReadLine() ?? "0");

int age = DateTime.Now.Year - birthYear;

Console.WriteLine();
Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст - {age}");
