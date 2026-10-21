using System;

namespace TodoListApp
{
    class Program
    {
        const string CommandHelp = "help";
        const string CommandProfile = "profile";
        const string CommandAdd = "add";
        const string CommandView = "view";
        const string CommandDone = "done";
        const string CommandDelete = "delete";
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
            bool[] statuses = new bool[InitialSize];
            DateTime[] dates = new DateTime[InitialSize];
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
                        HandleAdd(ref todos, ref statuses, ref dates, ref taskCount, argument);
                        break;
                    case CommandView:
                        HandleView(todos, statuses, dates, taskCount);
                        break;
                    case CommandDone:
                        HandleDone(statuses, dates, taskCount, argument);
                        break;
                    case CommandDelete:
                        HandleDelete(todos, statuses, dates, ref taskCount, argument);
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
        static void ExpandArrays(ref string[] oldStrings, ref bool[] oldBools, ref DateTime[] oldDates)
        {
            int newSize = oldStrings.Length * 2;
            string[] newStrings = new string[newSize];
            for (int i = 0; i < oldStrings.Length; i++)
                newStrings[i] = oldStrings[i];
            bool[] newBools = new bool[newSize];
            for (int i = 0; i < oldBools.Length; i++)
                newBools[i] = oldBools[i];
            DateTime[] newDates = new DateTime[newSize];
            for (int i = 0; i < oldDates.Length; i++)
                newDates[i] = oldDates[i];
            oldStrings = newStrings;
            oldBools = newBools;
            oldDates = newDates;
        }
        static void HandleHelp()
        {
            Console.WriteLine("help - список команд");
            Console.WriteLine("profile - данные пользователя");
            Console.WriteLine("add \"текст задачи\" - добавить задачу");
            Console.WriteLine("view - показать задачи");
            Console.WriteLine("done <номер задачи> - отметить задачу как выполненную");
            Console.WriteLine("delete <номер задачи> - удалить задачу");
            Console.WriteLine("exit - выход");
        }
        static void HandleProfile(string firstName, string lastName, int birthYear)
        {
            Console.WriteLine($"{firstName} {lastName}, {birthYear}");
        }
        static void HandleAdd(ref string[] todos, ref bool[] statuses, ref DateTime[] dates, ref int taskCount, string argument)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                Console.WriteLine("Введите текст задачи:");
                return;
            }
            string task = argument.Trim('"');
            if (taskCount == todos.Length)
            {
                ExpandArrays(ref todos, ref statuses, ref dates);
            }
            todos[taskCount] = task;
            statuses[taskCount] = false;
            dates[taskCount] = DateTime.Now;
            taskCount++;
            Console.WriteLine("Задача добавлена.");
        }
        static void HandleView(string[] todos, bool[] statuses, DateTime[] dates, int taskCount)
        {
            if (taskCount == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }
            for (int i = 0; i < taskCount; i++)
            {
                string statusText = "не сделано";
                if (statuses[i])
                {
                    statusText = "сделано";
                }
                Console.WriteLine($"{i + 1}. {todos[i]} ({statusText}) - {dates[i]}");
            }
        }
        static void HandleDone(bool[] statuses, DateTime[] dates, int taskCount, string argument)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                Console.WriteLine("Введите номер задачи.");
                return;
            }
            int taskNumber;
            if (!int.TryParse(argument, out taskNumber))
            {
                Console.WriteLine("Некорректный номер задачи.");
                return;
            }
            int index = taskNumber - 1;
            if (index < 0 || index >= taskCount)
            {
                Console.WriteLine("Задача с таким номером не найдена.");
                return;
            }
            statuses[index] = true;
            dates[index] = DateTime.Now;
            Console.WriteLine($"Задача №{taskNumber} отмечена как выполненная.");
        }
        static void HandleDelete(string[] todos, bool[] statuses, DateTime[] dates, ref int taskCount, string argument)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                Console.WriteLine("Введите номер задачи.");
                return;
            }
            int taskNumber;
            if (!int.TryParse(argument, out taskNumber))
            {
                Console.WriteLine("Некорректный номер задачи.");
                return;
            }
            int index = taskNumber - 1;
            if (index < 0 || index >= taskCount)
            {
                Console.WriteLine("Задача с таким номером не найдена.");
                return;
            }
            for (int i = index; i < taskCount - 1; i++)
            {
                todos[i] = todos[i + 1];
                statuses[i] = statuses[i + 1];
                dates[i] = dates[i + 1];
            }
            taskCount--;
            Console.WriteLine($"Задача №{taskNumber} успешно удалена.");
        }
    }
}
