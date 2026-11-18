using System;
namespace TodoListApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] todos = new string[2];
            Console.WriteLine("Работу выполнили Шевченко и Заиченко");
            Console.Write("Введите имя: ");
            string firstName = Console.ReadLine();
            Console.Write("Введите фамилию: ");
            string lastName = Console.ReadLine();
            Console.Write("Введите год рождения: ");
            int birthYear = Convert.ToInt32(Console.ReadLine());

            int currentYear = DateTime.Now.Year;
            int age = currentYear - birthYear;

            Console.WriteLine($"Новый пользователь {firstName} {lastName}, его возраст {age} лет.");

            while (true)
            {
                Console.Write("Введите команду: ");
                string command = Console.ReadLine();
                switch (command)
                {
                    case "help":
                        Console.WriteLine("Доступные команды:");
                        Console.WriteLine("help - помощь");
                        Console.WriteLine("profile - получить данные пользователя");
                        Console.WriteLine("add \"Текст задачи\" - добавить новую задачу");
                        Console.WriteLine("view - посмотреть текущие задачи");
                        Console.WriteLine("exit - выход");
                    break;
                    default:
                        Console.WriteLine("Неизвестная команда. Введите help для просмотра доступных команд");
                    break;

                }
            }
        }
    }
}

