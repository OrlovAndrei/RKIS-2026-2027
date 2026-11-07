using System;

namespace TodoListApp
{
    class Program
    {
        static void Main(string[] args)
        {
        Console.Write("Введите имя: ");
        string name = Console.ReadLine();

        Console.Write("Введите фамилию: ");
        string surname = Console.ReadLine();

        Console.Write("Введите год рождения: ");
        string birthYearText = Console.ReadLine();

        int birthYear = Convert.ToInt32(birthYearText);

        int currentYear = DateTime.Now.Year;
        int age = currentYear - birthYear;

        Console.WriteLine();
        Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст — {age}");
        }
    }
}
