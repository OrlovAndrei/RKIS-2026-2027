using System;

namespace TodoListApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Серюбин и Виноградов");
            
            Console.Write("Введите ваше имя: ");
            string firstName = Console.ReadLine();

            Console.Write("Введите вашу фамилию: ");
            string lastName = Console.ReadLine();

            Console.Write("Введите вашу дату рождения: ");
            string birthDate = Console.ReadLine();

            int birthYear = int.Parse(birthDate);
            int currentYear = DateTime.Now.Year;
            int age = currentYear - birthYear;

            Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");
        }
    }
}
