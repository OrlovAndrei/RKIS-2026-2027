using System;

namespace TodoListApp
{
    class Program
    {
        const string CommandHelp = "help";
        const string CommandProfile = "profile";
        const string CommandAdd = "add";
        const string CommandView = "view";
        const string CommandExit = "exit";
        const int InitialSize = 2;

        static void Main(string[] args)
        {
            Console.WriteLine("Работу выполнили Катмаков и Науменко.");
            Console.Write("Введите имя: ");
            string firstName = Console.ReadLine();
            Console.Write("Введите фамилию: ");
            string lastName = Console.ReadLine();
            Console.Write("Введите год рождения: ");
            int birthYear = Convert.ToInt32(Console.ReadLine());
            int currentYear = DateTime.Now.Year;
            int age = currentYear - birthYear;
            Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}.");
            string[] todos = new string[InitialSize];
            int taskCount = 0;
            Console.WriteLine("Добро пожаловать в TodoList! Введите help для отображения списка команд.");
            while (true)
            {
                Console.Write("\n> ");
                string input = Console.ReadLine();
                string[] parts = input.Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0)
                    continue;
                string command = parts[0].ToLower();
                string argument = parts.Length > 1 ? parts[1] : "";
                switch (command)
                {
                    case CommandHelp:
                        HandleHelp();
                        break;
                    case CommandProfile:
                        HandleProfile(firstName, lastName, birthYear);
                        break;
                    case CommandAdd:
                        HandleAdd(ref todos, ref taskCount, argument);
                        break;
                    case CommandView:
                        HandleView(todos, taskCount);
                        break;
                    case CommandExit:
                        Console.WriteLine("Завершение работы программы.");
                        return;
                    default:
                        Console.WriteLine("Неизвестная команда. Введите help.");
                        break;
                }
            }
        }
        static string[] ExpandArray(string[] oldArray)
        {
            string[] newArray = new string[oldArray.Length * 2];
            for (int i = 0; i < oldArray.Length; i++)
            {
                newArray[i] = oldArray[i];
            }
            return newArray;
        }
        static void HandleHelp()
        {
            Console.WriteLine("help - список команд");
            Console.WriteLine("profile - данные пользователя");
            Console.WriteLine("add \"текст задачи\" - добавить задачу");
            Console.WriteLine("view - показать задачи");
            Console.WriteLine("exit - выход");
        }
        static void HandleProfile(string firstName, string lastName, int birthYear)
        {
            Console.WriteLine($"{firstName} {lastName}, {birthYear}");
        }
        static void HandleAdd(ref string[] todos, ref int taskCount, string argument)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                Console.WriteLine("Введите текст задачи:");
                return;
            }
            string task = argument.Trim('"');

            if (taskCount == todos.Length)
            {
                todos = ExpandArray(todos);
            }
            todos[taskCount] = task;
            taskCount++;
            Console.WriteLine("Задача добавлена.");
        }
        static void HandleView(string[] todos, int taskCount)
        {
            if (taskCount == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }
            for (int i = 0; i < taskCount; i++)
            {
                Console.WriteLine($"{i + 1}. {todos[i]}");
            }
        }
    }
}
