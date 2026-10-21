using System;

namespace TodoListApp
{
    class Program                                                                                                                     
    {
        private const int InitialArraySize = 2;
        private const string DateFormat = "yyyy-MM-dd HH:mm";
        private const string CommandPrefix = "> ";

        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили, Погиба и Гуляев!");
            
            string firstName = ReadUserInput("Введите свое имя: ");
            string lastName = ReadUserInput("Введите свою фамилию: ");
            int birthYear = ReadIntInput("Введите год рождения: ");
            int years = DateTime.Now.Year - birthYear;

            Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, лет - {years}.");

            string[] todos = new string[InitialArraySize];
            bool[] statuses = new bool[InitialArraySize];
            DateTime[] dates = new DateTime[InitialArraySize];
            int taskCount = 0;

            while (true)
            {
                Console.Write(CommandPrefix);
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                string[] parts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                string command = parts[0].ToLower();
                string argument = parts.Length > 1 ? parts[1] : string.Empty;

                switch (command)
                {
                    case "help":
                        ShowHelp();
                        break;
                    case "profile":
                        ShowProfile(firstName, lastName, years);
                        break;
                    case "add":
                        AddTask(ref todos, ref statuses, ref dates, ref taskCount, argument);
                        break;
                    case "view":
                        ViewTasks(todos, statuses, dates, taskCount);
                        break;
                    case "done":
                        MarkTaskDone(ref statuses, ref dates, taskCount, argument);
                        break;
                    case "delete":
                        DeleteTask(ref todos, ref statuses, ref dates, ref taskCount, argument);
                        break;
                    case "update":
                        UpdateTask(ref todos, ref dates, taskCount, argument);
                        break;
                    case "exit":
                        Console.WriteLine("Выход из программы.");
                        return;
                    default:
                        Console.WriteLine("Неизвестная команда. Введите help для просмотра команд.");
                        break;
                }
            }
        }

        static string ReadUserInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine() ?? string.Empty;
        }

        static int ReadIntInput(string prompt)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int result))
                return result;
            
            Console.WriteLine("Некорректный ввод. Попробуйте снова.");
            return ReadIntInput(prompt);
        }

        static void ShowHelp()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("  help                  - показать список команд.");
            Console.WriteLine("  profile               - показать данные пользователя.");
            Console.WriteLine("  add \"текст\"         - добавить задачу.");
            Console.WriteLine("  view                  - показать все задачи.");
            Console.WriteLine("  done <индекс>         - отметить задачу выполненной.");
            Console.WriteLine("  delete <индекс>       - удалить задачу.");
            Console.WriteLine("  update <индекс> \"текст\" - обновить текст задачи.");
            Console.WriteLine("  exit                  - выйти из программы.");
        }

        static void ShowProfile(string firstName, string lastName, int years)
        {
            Console.WriteLine($"{firstName} {lastName}, {years}.");
        }

        static void ResizeArrays(ref string[] todos, ref bool[] statuses, ref DateTime[] dates)
        {
            int newSize = todos.Length * 2;
            
            Array.Resize(ref todos, newSize);
            Array.Resize(ref statuses, newSize);
            Array.Resize(ref dates, newSize);
        }

        static void AddTask(ref string[] todos, ref bool[] statuses, ref DateTime[] dates, ref int taskCount, string argument)
        {
            string taskText = argument.Replace("\"", "").Trim();

            if (string.IsNullOrWhiteSpace(taskText))
            {
                Console.WriteLine("Укажите текст задачи: add \"текст\"");
                return;
            }

            if (taskCount == todos.Length)
            {
                ResizeArrays(ref todos, ref statuses, ref dates);
            }

            todos[taskCount] = taskText;
            statuses[taskCount] = false;
            dates[taskCount] = DateTime.Now;
            taskCount++;

            Console.WriteLine($"Задача добавлена: {taskText}");
        }

        static void ViewTasks(string[] todos, bool[] statuses, DateTime[] dates, int taskCount)
        {
            if (taskCount == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }

            for (int i = 0; i < taskCount; i++)
            {
                string status = statuses[i] ? "сделано" : "не сделано";
                Console.WriteLine($"{i + 1}. {todos[i]} [{status}] ({dates[i]:yyyy-MM-dd HH:mm})");
            }
        }

        static void MarkTaskDone(ref bool[] statuses, ref DateTime[] dates, int taskCount, string argument)
        {
            if (!int.TryParse(argument, out int index) || index < 1 || index > taskCount)
            {
                Console.WriteLine("Укажите корректный индекс задачи.");
                return;
            }

            int arrayIndex = index - 1;
            statuses[arrayIndex] = true;
            dates[arrayIndex] = DateTime.Now;
            Console.WriteLine($"Задача {index} отмечена как выполненная.");
        }

        static void DeleteTask(ref string[] todos, ref bool[] statuses, ref DateTime[] dates, ref int taskCount, string argument)
        {
            if (!int.TryParse(argument, out int index) || index < 1 || index > taskCount)
            {
                Console.WriteLine("Укажите корректный индекс задачи.");
                return;
            }

            int arrayIndex = index - 1;

            for (int i = arrayIndex; i < taskCount - 1; i++)
            {
                todos[i] = todos[i + 1];
                statuses[i] = statuses[i + 1];
                dates[i] = dates[i + 1];
            }

            todos[taskCount - 1] = null;
            statuses[taskCount - 1] = false;
            dates[taskCount - 1] = default;

            taskCount--;
            Console.WriteLine($"Задача {index} удалена.");
        }

        static void UpdateTask(ref string[] todos, ref DateTime[] dates, int taskCount, string argument)
        {
            string[] updateArgs = argument.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

            if (updateArgs.Length < 2 || !int.TryParse(updateArgs[0], out int index) || index < 1 || index > taskCount)
            {
                Console.WriteLine("Использование: update <индекс> \"новый текст\"");
                return;
            }

            string newText = updateArgs[1].Replace("\"", "").Trim();
            if (string.IsNullOrWhiteSpace(newText))
            {
                Console.WriteLine("Новый текст не может быть пустым.");
                return;
            }

            int arrayIndex = index - 1;
            todos[arrayIndex] = newText;
            dates[arrayIndex] = DateTime.Now;

            Console.WriteLine($"Задача {index} обновлена.");
        }
    }
}