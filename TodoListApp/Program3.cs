using System;
using System.Text; // Для работы с кодировками

namespace TodoList
{
    class Program3
    {
        // Константы (убираем магические числа)
        private const int InitialCapacity = 2;
        
        // Массивы и счетчик вынесены на уровень класса для доступа из методов
        private static string[] todos = new string[InitialCapacity];
        private static int count = 0;

        // Данные пользователя
        private static string firstName = "";
        private static string lastName = "";
        private static string birthYearStr = "";
        private static int age = 0;

        static void Main(string[] args)
        {
            // 1. Настраиваем кодировку, чтобы русские буквы корректно читались и выводились
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            // 2. Приветствие
            Console.WriteLine("Работу выполнили: Осинцева и Малюкова");

            // 3. Запрос имени
            Console.Write("Введите имя: ");
            // Читаем данные пользователя. Конструкция ?? "" защищает от null 
            // (если пользователь нажмет Ctrl+Z, программа не остановится, а запишет пустую строку).
            firstName = Console.ReadLine() ?? "";

            // 4. Запрос фамилии
            Console.Write("Введите фамилию: ");
            lastName = Console.ReadLine() ?? "";

            // 5. Запрос года рождения
            Console.Write("Введите год рождения: ");
            birthYearStr = Console.ReadLine() ?? "";

            if (int.TryParse(birthYearStr, out int birthYear))
            {
                age = DateTime.Now.Year - birthYear;
                Console.WriteLine($"\nДобавлен пользователь {firstName} {lastName}, возраст - {age}");
            }

            Console.WriteLine($"\nДобро пожаловать, {firstName} {lastName}! Введите 'help'.");

            bool isRunning = true;
            while (isRunning)
            {
                Console.Write("> ");
                string? input = Console.ReadLine();

                // Защита от пустого ввода (если пользователь просто нажал Enter)
                if (string.IsNullOrWhiteSpace(input)) continue;

                // Разделяем команду и аргументы (максимум на 3 части для update)
                string[] parts = input.Trim().Split(new char[] { ' ' }, 3, StringSplitOptions.RemoveEmptyEntries);
                string command = parts[0].ToLower();

                switch (command)
                {
                    case "help":
                        ShowHelp(); // Вызов метода
                        break;
                    case "profile":
                        Console.WriteLine($"{firstName} {lastName}, {birthYearStr} (возраст: {age})");
                        break;
                    case "add":
                        if (parts.Length > 1) 
                            AddTask(parts[1].Trim('"')); // Вызов метода
                        else 
                            Console.WriteLine("Ошибка! Используйте: add \"текст\"");
                        break;
                    case "view":
                        ShowTasks(); // Вызов метода
                        break;
                    case "exit":
                        isRunning = false;
                        Console.WriteLine("Завершение работы...");
                        break;
                    default:
                        Console.WriteLine("Неизвестная команда. Введите 'help'.");
                        break;
                }
            }
        }

        // === МЕТОДЫ КОМАНД ===
        
        private static void ShowHelp()
        {
            Console.WriteLine("  help - список команд");
            Console.WriteLine("  profile - данные пользователя");
            Console.WriteLine("  add \"текст\" - добавить задачу");
            Console.WriteLine("  view - показать задачи");
            Console.WriteLine("  exit - выйти");
        }
                        
        private static void AddTask(string taskText)
        {
            if (count >= todos.Length) ResizeArray();

            todos[count] = taskText;
            count++;
            Console.WriteLine("Задача добавлена!");
        }

        private static void ShowTasks()
        {
            if (count == 0)
            {
                Console.WriteLine("(Список пуст)");
                return;
            }

            Console.WriteLine("Список задач:");
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"{i + 1}. {todos[i]}");
            }
        }
      
        private static void ResizeArray()
        {
            string[] newTodos = new string[todos.Length * 2];
            for (int i = 0; i < todos.Length; i++)
            {
                newTodos[i] = todos[i];
            }
            todos = newTodos;
            Console.WriteLine($"[Массив расширен до {todos.Length} элементов]");
        }
    }
}