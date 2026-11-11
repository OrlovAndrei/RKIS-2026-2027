using System;

namespace TodoListApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнил Омельчук Зайцев и Сальников");

            Console.WriteLine("Введите Имя:");
            string firstName = Console.ReadLine();

            Console.WriteLine("Введите Фамилию:");
            string secondName = Console.ReadLine();

            Console.WriteLine("Введите Год Рождения");
            int birthYear = Convert.ToInt32(Console.ReadLine());
            int currentYear = DateTime.Now.Year;
            int age = currentYear - birthYear;
            
            Console.WriteLine($"Добавлен новый пользователь {firstName} {secondName}, возраст - {age}");

            string[] todos = new string[2];
            int todoCount = 0;

            while (true)
            {   
                Console.Write("> ");
                var line = Console.ReadLine();
                if (line == null || line == "exit") break;
                Console.WriteLine("Echo: " + line);
        }
    }
}  