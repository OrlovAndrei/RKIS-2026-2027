using System;
using System.Text;

namespace TodoListApp
{
    class Program
    {
        static string[] todos = new string[2];
        static bool[] isCompleted = new bool[2];
        static DateTime[] updateDates = new DateTime[2];
        static int count = 0;

        static string name = "";
        static string lastname = "";
        static int yearOfBirth = 0;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("Сделала Боярова и Горелова");
            Console.WriteLine("Введите ваше имя");
            name = Console.ReadLine() ?? "";
            Console.WriteLine("Введите вашу фамилию");
            lastname = Console.ReadLine() ?? "";
            Console.WriteLine("Введите ваш год рождения");
            if (int.TryParse(Console.ReadLine(), out int yob))
            {
                yearOfBirth = yob;
            }
            int age = 2026 - yearOfBirth;
            Console.WriteLine($"Добавлен пользователь {name} {lastname}, возраст- {age}");

            PrintHelp();

            while (true)
            {
                Console.Write("\n> ");
                string? input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }

                if (input == "exit")
                {
                    Console.WriteLine("Выход из программы...");
                    break;
                }

                string[] parts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                string command = parts[0].ToLower();
                string argsText = parts.Length > 1 ? parts[1] : string.Empty;

                switch (command)
                {
                    case "help":
                        PrintHelp();
                        break;
                    case "profile":
                        ShowProfile(age);
                        break;
                    case "add":
                        AddTask(argsText);
                        break;
                    case "view":
                        ViewTasks(argsText);
                        break;
                    case "read":
                        ReadTask(argsText);
                        break;
                    default:
                        Console.WriteLine("Неизвестная команда. Введите 'help' для справки.");
                        break;
                }
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine("\nhelp - список команд");
            Console.WriteLine("profile - данные пользователя");
            Console.WriteLine("add <текст задачи> - добавить задачу (однострочный)");
            Console.WriteLine("add --multiline (или -m) - добавить многострочную задачу (завершение: !end)");
            Console.WriteLine("view [флаги] - вывести список задач (--index/-i, --status/-s, --update-date/-d, --all/-a)");
            Console.WriteLine("read <idx> - посмотреть полный текст задачи по индексу");
            Console.WriteLine("exit - выход из программы");
        }

        static void ShowProfile(int age)
        {
            Console.WriteLine($"{name} {lastname}, {yearOfBirth} (возраст: {age})");
        }

        static void AddTask(string args)
        {
            bool isMultiline = args.Contains("--multiline") || args.Contains("-m");
            string fullTaskText = "";

            if (isMultiline)
            {
                Console.WriteLine("Многострочный режим. Вводите строки задачи. Для завершения введите '!end':");
                StringBuilder sb = new StringBuilder();

                while (true)
                {
                    Console.Write("> ");
                    string? line = Console.ReadLine();

                    if (line == "!end")
                    {
                        break;
                    }

                    if (line != null)
{
                        if (sb.Length > 0)
                        {
                            sb.AppendLine();
                        }
                        sb.Append(line);
                    }
                }

                fullTaskText = sb.ToString();
            }
            else
            {
                if (string.IsNullOrWhiteSpace(args))
                {
                    Console.Write("Введите текст задачи: ");
                    fullTaskText = Console.ReadLine() ?? "";
                }
                else
                {
                    fullTaskText = args;
                }
            }

            if (string.IsNullOrWhiteSpace(fullTaskText))
            {
                Console.WriteLine("Ошибка: задача не может быть пустой.");
                return;
            }

            if (count >= todos.Length)
            {
                string[] newTodos = new string[todos.Length * 2];
                bool[] newCompleted = new bool[isCompleted.Length * 2];
                DateTime[] newDates = new DateTime[updateDates.Length * 2];

                for (int i = 0; i < todos.Length; i++)
                {
                    newTodos[i] = todos[i];
                    newCompleted[i] = isCompleted[i];
                    newDates[i] = updateDates[i];
                }

                todos = newTodos;
                isCompleted = newCompleted;
                updateDates = newDates;
            }

            todos[count] = fullTaskText;
            isCompleted[count] = false;
            updateDates[count] = DateTime.Now;
            count++;
            Console.WriteLine("Задача добавлена.");
        }

        static void ViewTasks(string args)
        {
            if (count == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }

            if (string.IsNullOrEmpty(args))
            {
                Console.WriteLine("\nСписок задач:");
                for (int i = 0; i < count; i++)
                {
                    string text = todos[i] ?? "";
                    if (text.Length > 30)
                    {
                        text = text.Substring(0, 30) + "...";
                    }
                    Console.WriteLine($"- {text}");
                }
                return;
            }

            bool showIndex = args.Contains("--index") || args.Contains("-i") || args.Contains("--all") || args.Contains("-a");
            bool showStatus = args.Contains("--status") || args.Contains("-s") || args.Contains("--all") || args.Contains("-a");
            bool showDate = args.Contains("--update-date") || args.Contains("-d") || args.Contains("--all") || args.Contains("-a");

            Console.WriteLine("\nСписок задач (таблица):");
            for (int i = 0; i < count; i++)
            {
                string row = "| ";

                if (showIndex) row += $"ID: {i + 1,-3} | ";

                if (showStatus)
                {
                    string statusStr = isCompleted[i] ? "[Выполнена]" : "[Не выполнена]";
                    row += $"{statusStr,-14} | ";
                }

                if (showDate)
                {
                    row += $"Изменено: {updateDates[i]:dd.MM.yyyy HH:mm} | ";
                }

                string taskText = todos[i] ?? "";
                if (taskText.Length > 30)
                {
                    taskText = taskText.Substring(0, 30) + "...";
                }
                taskText = taskText.Replace("\r\n", " ").Replace("\n", " ");

                row += $"Задача: {taskText,-30} |";
                Console.WriteLine(row);
            }
        }

        static void ReadTask(string args)
        {
            if (string.IsNullOrWhiteSpace(args))
            {
                Console.WriteLine("Ошибка: укажите индекс задачи. Пример: read 1");
                return;
            }
            if (int.TryParse(args.Trim(), out int idx))
            {
                int realIdx = idx - 1;

                if (realIdx < 0 || realIdx >= count || todos[realIdx] == null)
                {
                    Console.WriteLine("Ошибка: задачи с таким номером не существует.");
                    return;
                }

                Console.WriteLine($"\n--- Задача #{idx} ---");
                Console.WriteLine($"Полный текст:\n{todos[realIdx]}");
                Console.WriteLine($"Статус: {(isCompleted[realIdx] ? "Выполнена" : "Не выполнена")}");
                Console.WriteLine($"Дата последнего изменения: {updateDates[realIdx]:dd.MM.yyyy HH:mm:ss}");
            }
            else
            {
                Console.WriteLine("Ошибка: введите целое число для индекса.");
            }
        }
    }
}
