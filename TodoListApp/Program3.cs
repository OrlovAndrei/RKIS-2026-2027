using System;
using System.Text; // Для работы с кодировками

namespace TodoList
{
    class Program3
    {
        static void Main(string[] args)
        {
            // 1. Настраиваем кодировку, чтобы русские буквы корректно читались и выводились
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            // 2. Приветствие
            Console.WriteLine("Работу выполнили: Осинцева и Малюкова");

            // 3. Запрос имени
            Console.Write("Введите имя: ");
            // Знак "!" говорит компилятору, что мы уверены: здесь не будет null
            string firstName = Console.ReadLine()!;

            // 4. Запрос фамилии
            Console.Write("Введите фамилию: ");
            string lastName = Console.ReadLine()!;

            // 5. Запрос года рождения
            Console.Write("Введите год рождения: ");
            string birthYearStr = Console.ReadLine()!;

            // 6. Преобразование строки в целое число
            int birthYear = Convert.ToInt32(birthYearStr);

            // 7. Вычисление возраста
            int currentYear = DateTime.Now.Year;
            int age = currentYear - birthYear;

            // 8. Вывод результата
            Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");

            // Массив на 2 элемента
            string[] todos = new string[2];
            int count = 0; // Счетчик добавленных задач 
            bool isRunning = true; 

            Console.WriteLine($"\nДобро пожаловать, {firstName} {lastName}! Введите 'help'.");

            while (isRunning)
            {
                Console.Write("> ");
                string input = Console.ReadLine();

                // Защита от пустого ввода (если пользователь просто нажал Enter)
                if (string.IsNullOrWhiteSpace(input)) continue;

                // Берем только первое слово для switch, добавление .Trim() уберет лишние пробелы по краям ввода
                // Split(' ') делит строку по пробелам
                // [0] берет только первое слово (саму команду)
                // ToLower() переводит в нижний регистр (чтобы ADD и add работали одинаково)
                string command = input.Trim().Split(' ')[0].ToLower();

                switch (command)
                {
                    case "help":
                        Console.WriteLine("  help - список команд");
                        Console.WriteLine("  profile - данные пользователя");
                        Console.WriteLine("  add \"текст\" - добавить задачу");
                        Console.WriteLine("  view - показать задачи");
                        Console.WriteLine("  exit - выйти");
                        break;
                    case "profile":
                        Console.WriteLine($"{firstName} {lastName}, {birthYearStr}");
                        break;
                    case "add":
                        // Разделяем строку ввода максимум на 2 части (по первому пробелу).
                        // parts[0] будет "add", а parts[1] - сам текст задачи (даже если в нем есть пробелы)
                        string[] parts = input.Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);

                        // Если пользователь ввел просто "add" без текста - выдаем ошибку
                        if (parts.Length < 2)
                        {
                            Console.WriteLine("Ошибка! Используйте формат: add \"текст задачи\"");
                            break;
                        }
                        // Извлекаем текст задачи из второй части и убираем кавычки
                        string taskText = parts[1].Trim('"');
                        //Проверка на расширение массива
                        if (count >= todos.Length)
                        {
                            string[] newTodos = new string[todos.Length * 2];
                            for (int i = 0; i < todos.Length; i++)
                            {
                                newTodos[i] = todos[i];
                            }
                            todos = newTodos; 
                            Console.WriteLine($"[Массив расширен до {todos.Length} элементов]");
                        }
                        // Добавляем задачу
                        todos[count] = taskText;
                        count++;
                        Console.WriteLine("Задача добавлена!");
                        break;
                    case "view":
                        Console.WriteLine("Список задач:");
                        // Флаг, чтобы понять, есть ли вообще задачи (для вывода сообщения о пустом списке)
                        bool hasTasks = false;

                        // Проходим циклом только по заполненным ячейкам (от 0 до count)
                        // Это позволяет не выводить пустые строки (null)
                        for (int i = 0; i < count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {todos[i]}");
                            hasTasks = true;
                        }
                        if (!hasTasks) Console.WriteLine("(Список пуст)");
                        break; 
                    case "exit":
                        isRunning = false;
                        Console.WriteLine("Завершение работы...");
                        break;
                    default:
                        Console.WriteLine("Неизвестная команда.");
                        break;
                }
            }
        }
    }
}