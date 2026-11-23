using System.Text;

namespace TodoListApp;

internal class Program
{
    static void Main()
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Учебный проект TodoListApp");

        Console.Write("Имя: ");
        string? firstName = Console.ReadLine()?.Trim();
        Console.Write("Фамилия: ");
        string? lastName = Console.ReadLine()?.Trim();
        Console.Write("Год рождения: ");
        string? yearInput = Console.ReadLine();

        int currentYear = DateTime.Now.Year;
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            Console.WriteLine("Введите имя и фамилию.");
            return;
        }

        if (!int.TryParse(yearInput, out int birthYear) || birthYear < 1 || birthYear > currentYear)
        {
            Console.WriteLine("Введите корректный год рождения.");
            return;
        }

        int age = currentYear - birthYear;
        Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");
    }
}
