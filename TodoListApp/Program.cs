using System;

namespace TodoListApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнил Логвин");
            Console.WriteLine("Введите ваше имя:");
            string name = Console.ReadLine();
            Console.WriteLine("Введите вашу фамилию:");
            string surname = Console.ReadLine();
            Console.WriteLine("Введите ваш год рождения");
            int yearOfBirth = int.Parse(Console.ReadLine());
            int currentYear = 2026;
            Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст - {currentYear - yearOfBirth}");
        }
    }
}
