using System;
using System.Data;
namespace TodoListApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] todos = new string[2];
            int task_amount = 0;
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
                if (command == "")
                {
                    Console.WriteLine("Выберите команду. Введите help для просмотра доступных команд");
                    continue;
                } 
                string[] parsed_command = command.Split();
                switch (parsed_command[0].ToLower())
                {
                    case "help":
                        Console.WriteLine("Доступные команды:");
                        Console.WriteLine("help - помощь");
                        Console.WriteLine("profile - получить данные пользователя");
                        Console.WriteLine("add \"Текст задачи\" - добавить новую задачу");
                        Console.WriteLine("view - посмотреть текущие задачи");
                        Console.WriteLine("exit - выход");
                    break;

                    case "add":
                        if (parsed_command.Length == 1)
                        {
                            Console.WriteLine("Использование: add \"Текст задачи\"");
                            continue;
                        }
                        if (todos.Length == task_amount)
                        {
                            string[] todos_backup = new string[todos.Length];
                            for(int i = 0;i< todos.Length; i++)
                            {
                                todos_backup[i] = todos[i];
                            }
                            todos = new string[todos.Length * 2];
                            for(int i = 0;i< todos_backup.Length; i++)
                            {
                                todos[i] = todos_backup[i];
                            }
                        }
                        todos[task_amount] = parsed_command[1].Trim('"');
                        task_amount++;
                        Console.WriteLine($"Задача под номером {task_amount} добавлена!");
                        break;

                    case "view":
                        Console.WriteLine("Текущие задачи:");
                        int count = 1;
                        foreach(string task in todos)
                        {
                            if (task != "" && task != null){
                            Console.WriteLine($"{count} - {task}");
                            count++;
                            }
                        }
                        break;

                    case "profile":
                        Console.WriteLine("Текущий пользователь:");
                        Console.WriteLine($"{firstName} {lastName}, {birthYear}");
                        break;

                    case "exit":
                        return;

                    default:
                        Console.WriteLine("Неизвестная команда. Введите help для просмотра доступных команд");
                    break;
                }
            }
        }
    }
}

