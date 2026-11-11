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
            if (commandParts.Length < 2)
            {
                Console.WriteLine("Используйте: add \"текст задачи\"");
                break;
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
        }
    }
}  