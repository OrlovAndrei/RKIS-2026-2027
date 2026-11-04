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

        static bool[] statues = new bool[2];
        static DateTime[] dates = new DateTime[2];

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
                else if (inputLine.StartsWith("done "))
                {
                    Done(inputLine);
                }
                else if (inputLine.StartsWith("delete "))
                {
                    Delete(inputLine);
                }
                else if (inputLine.StartsWith("update "))
                {
                    Update(inputLine);
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
            Console.WriteLine("done \"номер\" - пометить задачу выполненной");
            Console.WriteLine("update \"номер\" \"текст\" - обновляет текст задачи");
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
                Arr();
            }
            todos[todoCount] = task;
            statues[todoCount] = false;
            dates[todoCount] = DateTime.Now;
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
                Console.WriteLine($"Задача {i + 1}: {todos[i]}, статус: {statues[i]}, дата изменения: {dates[i]}");
            }
        }

        static void Arr()
        {
            string[] newTodos = new string[todos.Length * 2];
            for (int i = 0; i < todos.Length; i++)
            {
                newTodos[i] = todos[i];
            }
            todos = newTodos;

            bool[] newStatues = new bool[statues.Length * 2];
            for (int i = 0; i < statues.Length; i++)
            {
                newStatues[i] = statues[i];
            }
            statues = newStatues;

            DateTime[] newDates = new DateTime[dates.Length * 2];
            for (int i = 0; i < dates.Length; i++)
            {
                newDates[i] = dates[i];
            }
            dates = newDates;
            Console.WriteLine("Массивы расширены");
        }

        //Практическая работа №4
        static void Done(string inputLine)
        {
            string[] parts = inputLine.Split(' ', 2);
            if (!int.TryParse(parts[1], out int taskNumber))
            {
                Console.WriteLine("Номер задачи должен быть числом");
                return;
            }
            int index = taskNumber - 1;
            statues[index] = true;
            dates[index] = DateTime.Now;
            Console.WriteLine("Задача помечена, как выполненная");
        }

        static void Delete(string inputLine)
        {
            string[] parts = inputLine.Split(' ', 2);
            if (!int.TryParse(parts[1], out int taskNumber))
            {
                Console.WriteLine("Номер задачи должен быть числом");
                return;
            }
            int index = taskNumber - 1;
            for (int i = index; i < todoCount - 1; i++)
            {
                todos[i] = todos[i+1];
                statues[i] = statues[i+1];
                dates[i] = dates[i+1];
            }
            todos[todoCount - 1] = null;
            statues[todoCount - 1] = false;
            dates[todoCount - 1] = DateTime.MinValue;
            todoCount--;
            Console.WriteLine($"Задача {taskNumber} удалена.");
        }

        static void Update(string inputLine)
        {
            string[] parts = inputLine.Split(' ', 3);
            if (parts.Length < 3)
            {
                Console.WriteLine("Укажите номер задачи и новый текст");
                return;
            }
            if (!int.TryParse(parts[1], out int taskNumber))
            {
                Console.WriteLine("Номер задачи должен быть числом");
                return;
            }

            int index = taskNumber - 1;
            if (index < 0 || index >= todoCount)
            {
                Console.WriteLine("Задачи с таким номером не существует");
                return;
            }

            string newText = parts[2].Trim('"');
            todos[index] = newText;
            dates[index] = DateTime.Now;
            Console.WriteLine($"Задача {taskNumber} обновлена.");
        }
    }
}