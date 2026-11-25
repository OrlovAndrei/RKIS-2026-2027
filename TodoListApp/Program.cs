using System;

namespace TodoListApp
{
    class Program
    {
        const int InitialCapacity = 2;
        const int GrowthFactor = 2;
        const int IndexOffset = 1;
        const char Quote = '"';            
        const char Separator = ' '; 

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

            string[] todos = new string[InitialCapacity];
            bool[] statuses = new bool[InitialCapacity];
            DateTime[] dates = new DateTime[InitialCapacity];

            int todoCount = 0;

            while (true)
            {   
                Console.Write("> ");
                var line = Console.ReadLine();

                if (input == null)
                     break;

                string[] commandParts = input.Trim().Split(Separator, 2);
                string command = commandParts[0].ToLower();
            
                switch (command)
                {
                    case "help":
                        Console.WriteLine("Доступные команды:");
                        Console.WriteLine("help - список команд");
                        Console.WriteLine("profile - данные пользователя");
                        Console.WriteLine("add \"текст задачи\" - добавить задачу");
                        Console.WriteLine("view - показать задачи");
                        Console.WriteLine("exit - выход");
                        break;
                    case "profile":
                        Console.WriteLine($"{firstName} {secondName}, {birthYear}.");
                        break;
                      case "add":
            if (todoCount >= todos.Length)
            {
                ExpandArrays(ref todos, ref statuses, ref dates);
            }

            string taskWithQuotes = commandParts[1].Trim();

            if (taskWithQuotes.Length < 2 ||
                taskWithQuotes[0] != '"' ||
                taskWithQuotes[taskWithQuotes.Length - 1] != '"')
            {
                Console.WriteLine("Заключите задачу в кавычки.");
                break;
            }

            string task = taskWithQuotes.Substring(
                1,
                taskWithQuotes.Length - 2
            ).Trim();

            if (string.IsNullOrWhiteSpace(task))
            {
                Console.WriteLine("Задача не может быть пустой.");
                break;
            }

            if (todoCount >= todos.Length)
            {
                string[] newTodos = new string[todos.Length * 2];

                for (int i = 0; i < todos.Length; i++)
                {
                    newTodos[i] = todos[i];
                }

                todos = newTodos;
            }

            todos[todoCount] = task;
            todoCount++;

            Console.WriteLine("Задача добавлена!");
            break;
            
             case "view":
            if (todoCount == 0)
            {
                Console.WriteLine("Список задач пуст.");
                break;
            }

            Console.WriteLine("Список задач:");

            for (int i = 0; i < todos.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(todos[i]))
                {
                    Console.WriteLine($"{i + 1}. {todos[i]}");
                }
            }

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
static void ExpandArrays(ref string[] todos,
    ref bool[] statuses, ref DateTime[] dates)
{
    int newSize = todos.Length * GrowthFactor;

    Array.Resize(ref todos, newSize);
    Array.Resize(ref statuses, newSize);
    Array.Resize(ref dates, newSize);
}