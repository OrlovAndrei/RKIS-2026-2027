using System;

namespace TodoListApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Сотник и Шпак");
            Console.WriteLine("Ведите своё имя пожалуйста: ");
            string name = Console.ReadLine();
            Console.WriteLine("Ведите свою фамилию пожалуйста: ");
            string surname = Console.ReadLine();
            Console.WriteLine("Ведите свой год рождения пожалуйста: ");
            var year = int.Parse(Console.ReadLine());
            Console.WriteLine($"Добавлен пользователь: {name} {surname} возраст: {2026-year}");
            
        }
    }
}
