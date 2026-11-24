using System;

namespace TodoListApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнил Туваев");
            Console.Write("Введите ваше имя: ");
            string firstName = Console.ReadLine();
            Console.Write("Введите вашу фамилию: ");
            string lastName = Console.ReadLine();
            Console.Write("Введите вашу дату рождения: ");
            string birthDate = Console.ReadLine();

            int birthYear = int.Parse(birthDate);
            int curentYear = DateTime.Now.Year;

            int age = curentYear - birthYear;

            Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");
        }
    }
}
