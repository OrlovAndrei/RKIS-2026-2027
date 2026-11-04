using System;

namespace TodoList
{
    class Program
    {
        static string name1;
        static string name2;
        static int year;
        static string[] todos = new string[2];
        static int todoCount = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Сипач и Пасалиди");

            //Практическая работа №2
            Console.Write("Введите имя: ");
            name1 = Console.ReadLine();

            Console.Write("Введите фамилию: ");
            name2 = Console.ReadLine();

            Console.Write("Введите год рождения: ");
            string yearInput = Console.ReadLine();
            year = int.Parse(yearInput);

            int curYear = DateTime.Now.Year;
            int age = curYear - year;
            Console.WriteLine($"Добавлен пользователь {name2} {name1}, возраст - {age}");
            Console.WriteLine();

            //Практическая работа №3
            todos = new string[2];
            todoCount = 0;

            while (true)
            {
                Console.Write("> ");
                string inputLine = Console.ReadLine();
                if (inputLine == "exit")
                {
                    Console.WriteLine("Завершение программы");
                    break;
                }
                else if (inputLine == "help")
                {
                    Help();
                }
                else if (inputLine == "profile")
                {
                    Profile();
                }
                else if (inputLine.StartsWith("add "))
                {
                    AddTask(inputLine);
                }
                else if (inputLine == "view")
                {
                    View();
                }
                else
                {
                    Console.WriteLine("Неизвестная команда. Введите help");
                }
            }
        }

        static void Help()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help - список команд");
            Console.WriteLine("profile - данные пользователя");
            Console.WriteLine("add \"текст\" - добавить задачу");
            Console.WriteLine("view - показывать задачи");
            Console.WriteLine("exit - выход");
        }

        static void Profile()
        {
            Console.WriteLine($"{name1} {name2}, {year}");
        }

        static void AddTask(string inputLine)
        {
            string[] parts = inputLine.Split(' ', 2);
            string task = parts[1].Trim('"');
            if (todoCount >= todos.Length)
            {
                TodosArr();
            }
            todos[todoCount] = task;
            todoCount++;
            Console.WriteLine($"Задача добавлена: {task}");
        }

        static void View()
        {
            if (todoCount == 0)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }
            for (int i = 0; i < todoCount; i++)
            {
                Console.WriteLine($"Задача {i + 1}: {todos[i]}");
            }
        }

        static void TodosArr()
        {
            string[] newTodos = new string[todos.Length * 2];
            for (int i = 0; i < todos.Length; i++)
            {
                newTodos[i] = todos[i];
            }
            todos = newTodos;
            Console.WriteLine("Массив расширен");
        }
    }
}