using System;

namespace TodoListApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Работу сделали Погиба и Гуляев!");

            Console.WriteLine("Введите своё имя: ");
            var firstName = Console.ReadLine();

            Console.WriteLine("Введите свою фамилию: ");
            var lastName = Console.ReadLine();

            Console.WriteLine("Введите год рождения: ");
            string birthDate = Console.ReadLine();

            int years = int.Parse(birthDate);
            years = 2026 - years;

            Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, год - {years}.");
        }
    }
}
