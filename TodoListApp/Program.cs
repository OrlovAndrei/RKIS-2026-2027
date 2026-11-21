using System;

namespace TodoListApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнила Резниченко");
            Console.WriteLine("Введите ваше имя");
            string name = Console.ReadLine();
            Console.WriteLine("Введите вашу фамилию");
            string surname = Console.ReadLine();
            Console.WriteLine("Введите ваш год рождения");
            int yearOfBirth = int.Parse(Console.ReadLine());
            int age = 2026-yearOfBirth;
            Console.WriteLine($"Добавлен пользователь {name} {surname}, возраст - {age}");

        }
    }
}
