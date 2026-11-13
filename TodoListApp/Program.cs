using System;
using System.Text; // Для работы с кодировками

namespace TodoList
{
    class Program
    {
        // Константы (убираем магические числа)
        private const int InitialCapacity = 2;
        private const string DateFormat = "dd.MM.yyyy HH:mm"; //формат даты
        // Массивы и счетчик вынесены на уровень класса для доступа из методов
        private static string[] todos = new string[InitialCapacity];
        private static bool[] statuses = new bool[InitialCapacity];     // массив статусов
        private static DateTime[] dates = new DateTime[InitialCapacity]; // массив дат
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

                // Разделяем команду и аргументы
                string[] parts = input.Trim().Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
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
                        // Проверяем, что флаг передан именно как аргумент, а не внутри текста
                        if (parts.Length > 1 && (parts[1] == "-m" || parts[1] == "--multiline"))
                        {
                            AddMultilineTask();
                        }
                        else
                        {
                            if (parts.Length > 1) AddTask(parts[1].Trim('"'));
                            else Console.WriteLine("Ошибка! Используйте: add \"текст\" или add -m");
                        }
                        break;
                    case "view":
                        ShowTasks(input); // Передаем строку ввода для анализа флагов
                        break;
                    case "done":
                        // Проверяем правильность индекса и вызываем метод
                        if (TryParseIndex(parts, out int doneIdx)) 
                            MarkAsDone(doneIdx);
                        break;
                    case "delete":
                        if (TryParseIndex(parts, out int delIdx)) 
                            DeleteTask(delIdx);
                        break;
                    case "update":
                        // Для update парсим строку отдельно, так как нужно отделить индекс от текста
                        if (parts.Length > 1)
                        {
                            string[] updateArgs = parts[1].Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
                            if (updateArgs.Length == 2 && int.TryParse(updateArgs[0], out int updIdx))
                            {
                                int realIndex = updIdx - 1;
                                if (realIndex >= 0 && realIndex < count)
                                    UpdateTask(realIndex, updateArgs[1].Trim('"'));
                                else
                                    Console.WriteLine("Ошибка! Задачи с таким номером не существует.");
                            }
                            else
                            {
                                Console.WriteLine("Ошибка! Используйте: update <индекс> \"новый текст\"");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ошибка! Используйте: update <индекс> \"новый текст\"");
                        }
                        break;
                    case "read":
                        // Используем метод проверки индекса
                        if (TryParseIndex(parts, out int readIdx)) 
                            ReadTask(readIdx);
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

        // === МЕТОДЫ ===
        
        private static void ShowHelp()
        {
            Console.WriteLine("  help - список команд");
            Console.WriteLine("  profile - данные пользователя");
            Console.WriteLine("  add \"текст\" - добавить задачу");
            Console.WriteLine("  add -m / --multiline - добавить задачу в несколько строк");
            Console.WriteLine("  view [флаги] - показать задачи в виде таблицы");
            Console.WriteLine("         -i, --index       показать индексы");
            Console.WriteLine("         -s, --status      показать статусы");
            Console.WriteLine("         -d, --update-date показать даты изменения");
            Console.WriteLine("         -a, --all         показать все данные");
            Console.WriteLine("  read <idx> - прочитать полный текст задачи");
            Console.WriteLine("  done <idx> - отметить задачу выполненной");
            Console.WriteLine("  delete <idx> - удалить задачу");
            Console.WriteLine("  update <idx> \"текст\" - обновить текст задачи");
            Console.WriteLine("  exit - выйти");
        }

        // Проверка, что ввели корректный номер задачи (защита от падения)
        private static bool TryParseIndex(string[] parts, out int index)
        {
            index = -1;
            if (parts.Length < 2 || !int.TryParse(parts[1], out int userIdx))
            {
                Console.WriteLine("Ошибка! Укажите корректный номер задачи.");
                return false;
            }
            index = userIdx - 1; // Перевод в 0-индексацию (пользователь вводит с 1)
            if (index < 0 || index >= count)
            {
                Console.WriteLine("Ошибка! Задачи с таким номером не существует.");
                return false;
            }
            return true;
        }

         private static void ResizeArrays()
        {   
            int newSize = todos.Length * 2;

            string[] newTodos = new string[newSize];
            bool[] newStatuses = new bool[newSize];
            DateTime[] newDates = new DateTime[newSize];

            for (int i = 0; i < todos.Length; i++)
            {
                newTodos[i] = todos[i];
                newStatuses[i] = statuses[i];
                newDates[i] = dates[i];
            }
            todos = newTodos;
            statuses = newStatuses;
            dates = newDates;

            Console.WriteLine($"[Массив расширен до {newSize} элементов]");
        }
         
        private static void AddTask(string taskText)
        {
            if (count >= todos.Length) ResizeArrays();

            todos[count] = taskText;
            statuses[count] = false;             // Новая задача по умолчанию не выполнена
            dates[count] = DateTime.Now;         // Записываем текущие дату и время
            count++;
            Console.WriteLine("Задача добавлена!");
        }

        private static void AddMultilineTask()
        {
          Console.WriteLine("Введите текст задачи (для завершения введите '! end'):");
          StringBuilder sb = new StringBuilder();
    
          while (true)
          {
            string? line = Console.ReadLine();
            if (line == null) break;

            if (line.Trim() == "! end" || line.Trim() == "!end")
               break;

            // Если это не первая строка, добавляем перенос
            if (sb.Length > 0) sb.Append("\n");
            sb.Append(line);
        }

        if (sb.Length > 0)
            AddTask(sb.ToString());
        else
            Console.WriteLine("Задача не добавлена (пустой текст).");
        }

        private static void ShowTasks(string input)
        {
            if (count == 0)
            {
                Console.WriteLine("(Список пуст)");
                return;
            }

            // Определяем, какие флаги включены. 
            // Метод Contains найдет флаг, даже если он в комбинации (например, в "-is" он найдет "-i")
            // Разбиваем ввод на отдельные слова, чтобы проверять флаги точно
            string[] words = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            
            bool showIndex = false;
            bool showStatus = false;
            bool showDate = false;

            foreach (string word in words)
            {
                // Проверяем длинные флаги
                if (word == "-a" || word == "--all") { showIndex = true; showStatus = true; showDate = true; }
                if (word == "-i" || word == "--index") showIndex = true;
                if (word == "-s" || word == "--status") showStatus = true;
                if (word == "-d" || word == "--update-date") showDate = true;

                // Проверяем короткие комбинированные флаги (например, -is или -si)
                if (word.StartsWith("-") && !word.StartsWith("--") && word.Length > 2)
                {
                    if (word.Contains('i')) showIndex = true;
                    if (word.Contains('s')) showStatus = true;
                    if (word.Contains('d')) showDate = true;
                }
            }

            // Формируем заголовок таблицы
            string header = "";
            if (showIndex) header += "| " + "№".PadRight(3) + " ";
            header += "| " + "Текст задачи".PadRight(33) + " "; // 30 символов текста + 3 точки
            if (showStatus) header += "| " + "Статус".PadRight(12) + " ";
            if (showDate) header += "| " + "Дата изменения".PadRight(16) + " ";
            header += "|";

            string separator = new string('-', header.Length);
            Console.WriteLine(separator);
            Console.WriteLine(header);
            Console.WriteLine(separator);

            // Выводим строки с данными
            for (int i = 0; i < count; i++)
            {
                string row = "";

                // 1. Индекс
                if (showIndex) 
                    row += "| " + (i + 1).ToString().PadRight(3) + " ";

                // 2. Текст задачи
                // Заменяем переносы строк на пробелы, чтобы таблица не поехала
                string displayText = todos[i].Replace("\n", " ");
                // Обрезаем текст до 30 символов
                if (displayText.Length > 30) 
                    displayText = displayText.Substring(0, 30) + "...";
                row += "| " + displayText.PadRight(33) + " ";

                // 3. Статус
                if (showStatus) 
                {
                    string statusStr = statuses[i] ? "сделано" : "не сделано";
                    row += "| " + statusStr.PadRight(12) + " ";
                }

                // 4. Дата
                if (showDate) 
                    row += "| " + dates[i].ToString(DateFormat).PadRight(16) + " ";

                row += "|";
                Console.WriteLine(row);
            }
            Console.WriteLine(separator);
        }

        private static void MarkAsDone(int index)
        {
            statuses[index] = true;          
            dates[index] = DateTime.Now;     
            Console.WriteLine($"Задача #{index + 1} отмечена выполненной.");
        }

        private static void DeleteTask(int index)
        {
            for (int i = index; i < count - 1; i++)
            {
                todos[i] = todos[i + 1];
                statuses[i] = statuses[i + 1];
                dates[i] = dates[i + 1];
            }
            count--; 
            Console.WriteLine($"Задача #{index + 1} удалена.");
        }

        private static void UpdateTask(int index, string newText)
        {
            todos[index] = newText;          // Меняем текст
            dates[index] = DateTime.Now;     // Обновляем дату изменения
            Console.WriteLine($"Задача #{index + 1} обновлена.");
        }
        private static void ReadTask(int index)
        {
            Console.WriteLine("=== Полный текст задачи ===");
            Console.WriteLine(todos[index]); // Выводим текст как есть, с переносами строк
            Console.WriteLine("===========================");
            Console.WriteLine($"Статус: {(statuses[index] ? "выполнена" : "не выполнена")}");
            Console.WriteLine($"Дата изменения: {dates[index].ToString(DateFormat)}");
        }
    }
}