using System;

namespace TodoListApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Задание выполнили Дегтерев Пелехов 3832");

            Console.WriteLine("Введите имя:");
            string firstName = Console.Readline();

            Console.WriteLine("Введите фамилию:");
            string secondName = Console.ReadLine();
            
            Console.WriteLine("Введите год рождения");
            int birthYear = Covert.ToInt32(Console.ReadLine());
            int currentYear = DateTime.Now.Year;
            int age = currentYear - birthYear;

            Console.WriteLine($"Добавлен новый пользователь {firstName} {secondName}, возраст - {age}");

        }
    }
}
