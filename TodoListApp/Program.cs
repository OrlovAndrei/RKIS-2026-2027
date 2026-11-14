using System;
using System.Text; // Для работы с кодировками

namespace TodoList
{
    class Program
    {
        // Вместо массивов теперь используем объекты классов
        private static TodoList todoList = new TodoList();
        private static Profile? userProfile;

        static void Main(string[] args)
        {
            // Настраиваем кодировку, чтобы русские буквы корректно читались и выводились
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            // Приветствие
            Console.WriteLine("Работу выполнили: Осинцева и Малюкова");

            // Запрос имени
            Console.Write("Введите имя: ");
            // Читаем данные пользователя. Конструкция ?? "" защищает от null 
            // (если пользователь нажмет Ctrl+Z, программа не остановится, а запишет пустую строку).
            string firstName = Console.ReadLine() ?? "";

            // Запрос фамилии
            Console.Write("Введите фамилию: ");
            string lastName = Console.ReadLine() ?? "";

            // Запрос года рождения
            Console.Write("Введите год рождения: ");
            int.TryParse(Console.ReadLine(), out int birthYear);
            userProfile = new Profile(firstName, lastName, birthYear);
            
            Console.WriteLine($"\nДобавлен пользователь {firstName} {lastName}");
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
                        Console.WriteLine(userProfile?.GetInfo());
                        break;
                    case "add":
                        // Проверяем, что флаг передан именно как аргумент, а не внутри текста
                        if (parts.Length > 1 && (parts[1] == "-m" || parts[1] == "--multiline"))
                        {
                            AddMultilineTask();
                        }
                        else if (parts.Length > 1)
                        {
                            // Создаем объект TodoItem и добавляем в TodoList
                            todoList.Add(new TodoItem(parts[1].Trim('"')));
                            Console.WriteLine("Задача добавлена!");
                        }
                        else Console.WriteLine("Ошибка! Используйте: add \"текст\" или add -m");
                        break;
                    case "view":
                        string[] viewArgs = parts.Length > 1 ? parts[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) : new string[0];
                        
                        bool showIndex = false;
                        bool showDone = false;
                        bool showDate = false;

                        foreach (string arg in viewArgs)
                        {
                            if (arg == "-a" || arg == "--all") 
                            { 
                                showIndex = true; showDone = true; showDate = true; 
                            }
                            if (arg == "-i" || arg == "--index") showIndex = true;
                            if (arg == "-s" || arg == "--status") showDone = true;
                            if (arg == "-d" || arg == "--update-date") showDate = true;

                            if (arg.StartsWith("-") && !arg.StartsWith("--") && arg.Length > 2)
                            {
                                if (arg.Contains('i')) showIndex = true;
                                if (arg.Contains('s')) showDone = true;
                                if (arg.Contains('d')) showDate = true;
                            }
                        }
                        
                        todoList.View(showIndex, showDone, showDate);
                        break;
                    case "done":
                        // Проверяем правильность индекса и вызываем метод
                        if (TryParseIndex(parts, out int doneIdx)) 
                        {
                            // Берем задачу из списка и просим её саму стать выполненной
                            todoList.GetItem(doneIdx).MarkDone();
                            Console.WriteLine($"Задача #{doneIdx + 1} отмечена выполненной.");
                        }    
                        break;
                    case "delete":
                        if (TryParseIndex(parts, out int delIdx)) 
                        {
                            todoList.Delete(delIdx);
                            Console.WriteLine($"Задача #{delIdx + 1} удалена.");
                        }    
                        break;
                    case "update":
                        // Для update парсим строку отдельно, так как нужно отделить индекс от текста
                        if (parts.Length > 1)
                        {
                            string[] updateArgs = parts[1].Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
                            if (updateArgs.Length == 2 && int.TryParse(updateArgs[0], out int updIdx))
                            {
                                int realIndex = updIdx - 1;
                                if (realIndex >= 0 && realIndex < todoList.Count)
                                {
                                    // Получаем задачу и обновляем её текст
                                    todoList.GetItem(realIndex).UpdateText(updateArgs[1].Trim('"'));
                                    Console.WriteLine($"Задача #{updIdx} обновлена.");
                                }
                                else
                                    Console.WriteLine("Ошибка! Задачи с таким номером не существует.");
                            }
                        }
                        break;
                    case "read":
                        // Используем метод проверки индекса
                        if (TryParseIndex(parts, out int readIdx))
                        { 
                            // Вызываем метод GetFullInfo у самой задачи
                            Console.WriteLine(todoList.GetItem(readIdx).GetFullInfo());
                        }    
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
            if (index < 0 || index >= todoList.Count)
            {
                Console.WriteLine("Ошибка! Задачи с таким номером не существует.");
                return false;
            }
            return true;
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
        {
            // Создаем TodoItem и добавляем в TodoList
            todoList.Add(new TodoItem(sb.ToString()));
            Console.WriteLine("Задача добавлена!");
        }
        else
            Console.WriteLine("Задача не добавлена (пустой текст).");
        }
    }
}