using System;

namespace TodoListApp
{
    class Program
    {
        // Константы для размера массива (Пункт 2, дефис 3)
        private const int INITIAL_CAPACITY = 2;
        private const int CAPACITY_MULTIPLIER = 2;

        // Понятные имена в едином стиле (Пункт 2, дефис 1)
        private static string[] _todos = new string[INITIAL_CAPACITY];
        private static int _todoCount = 0;

          // --- ДОБАВИЛИ ДВА МАССИВА ---
                private static bool[] _statuses = new bool[INITIAL_CAPACITY];
        private static DateTime[] _dates = new DateTime[INITIAL_CAPACITY];
                private static Profile _userProfile = new Profile("Анатолий", "Васильев", 2004);
                        private static TodoList _todoList = new TodoList();
         // Пункт 6: Формирование безопасных путей к файлам с помощью Path.Combine
        private static readonly string _dataDir = "data";
        private static readonly string _todoPath = Path.Combine("data", "todo.csv");
        private static readonly string _profilePath = Path.Combine("data", "profile.csv");
        static void Main(string[] args)
        {
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("--- Приложение TodoList ---");
            Console.WriteLine("Введите 'help' для просмотра команд.\n");
            // --- Пункт 5: Проверка директории и загрузка данных при запуске ---
            FileManager.EnsureDataDirectory(_dataDir);

            // Пункт 3: Автоматическая загрузка списка всех профилей из profile.csv при старте
            AppInfo.AllProfiles = FileManager.LoadAllProfiles(_profilePath);
             // Внешний бесконечный цикл работы приложения (для поддержки смены профилей)
            while (true)
            {
                // Пункт 5: Запуск стартового меню входа/регистрации
                if (!ShowAuthMenu())
                {
                    Console.WriteLine("Выход из приложения.");
                    break;
                }

                // Пункт 6: Формирование пути к персональному файлу задач на основе ID вошедшего пользователя
                string personalTodoPath = Path.Combine("data", $"todos_{AppInfo.CurrentProfileId}.csv");

                // Загружаем персональные задачи
                if (File.Exists(personalTodoPath))
                {
                    AppInfo.Todos = FileManager.LoadTodos(personalTodoPath);
                }
                else
                {
                    AppInfo.Todos = new TodoList();
                    FileManager.SaveTodos(AppInfo.Todos, personalTodoPath);
                }
                AppInfo.Todos.OnTodoAdded += (item) => FileManager.SaveTodos(AppInfo.Todos, personalTodoPath);
                AppInfo.Todos.OnTodoDeleted += (item) => FileManager.SaveTodos(AppInfo.Todos, personalTodoPath);
                AppInfo.Todos.OnTodoUpdated += (item) => FileManager.SaveTodos(AppInfo.Todos, personalTodoPath);
                AppInfo.Todos.OnStatusChanged += (item) => FileManager.SaveTodos(AppInfo.Todos, personalTodoPath);
                while (AppInfo.CurrentProfileId != null)
                {
                    Console.Write("Введите команду: ");
                    string? input = Console.ReadLine()?.Trim();
                    if (string.IsNullOrEmpty(input)) continue;

                    string lowerInput = input.ToLower();

                    if (lowerInput == "exit")
                    {
                        Console.WriteLine("Программа завершена.");
                        return; // Полный выход из приложения
                    }

                    if (lowerInput == "help")
                    {
                        ShowHelp();
                        continue;
                    }

                    // Передаем управление паттерну Команда с использованием глобального состояния AppInfo
                    ICommand command = CommandParser.Parse(input, AppInfo.Todos, AppInfo.CurrentProfile!);
                    command.Execute();

                    // Сохраняем в стек отмены только отменяемые команды (add и status)
                    if (command is AddCommand || command is StatusCommand)
                    {
                        AppInfo.UndoStack.Push(command);
                        AppInfo.RedoStack.Clear();
                    }
                }
            {
                Console.Write("Введите команду: ");
                string? input = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(input)) continue;

                string lowerInput = input.ToLower();

                if (lowerInput == "help")
                {
                    ShowHelp();
                }
                                // --- Пункт 5: Единая точка обработки ООП-команд через паттерн Команда ---
                            else if (lowerInput == "profile" || input.StartsWith("add ", StringComparison.OrdinalIgnoreCase) || lowerInput.StartsWith("view") || input.StartsWith("status ", StringComparison.OrdinalIgnoreCase))
                {
                     ICommand command = CommandParser.Parse(input, _todoList, _userProfile);

                    // Если ввели команду отмены (Undo)
                    if (command is UndoCommand)
                    {
                        if (AppInfo.UndoStack.Count > 0)
                        {
                            ICommand undoCmd = AppInfo.UndoStack.Pop();
                            undoCmd.Unexecute();
                            AppInfo.RedoStack.Push(undoCmd); // Запоминаем для возможного Redo
                        }
                        else
                        {
                            Console.WriteLine("История действий пуста. Нечего отменять.\n");
                        }
                    }
                    // Если ввели команду повтора (Redo)
                    else if (command is RedoCommand)
                    {
                        if (AppInfo.RedoStack.Count > 0)
                        {
                            ICommand redoCmd = AppInfo.RedoStack.Pop();
                            redoCmd.Execute();
                            AppInfo.UndoStack.Push(redoCmd); // Возвращаем в стек отмены
                        }
                        else
                        {
                            Console.WriteLine("Нет отменённых действий для повтора.\n");
                        }
                    }
                    // Обычная команда (add, status, profile, view)
                    else
                    {
                        command.Execute();
                        
                        // Если команда отменяемая (add или status) — сохраняем её в стек истории
                        if (command is AddCommand || command is StatusCommand)
                        {
                            AppInfo.UndoStack.Push(command);
                            AppInfo.RedoStack.Clear(); // Новое действие очищает стек повтора
                        }
                    }
                }
                else if (input.StartsWith("delete ", StringComparison.OrdinalIgnoreCase))
                {
                    ExecuteDeleteCommand(input);
                }
                else if (input.StartsWith("update ", StringComparison.OrdinalIgnoreCase))
                {
                    ExecuteUpdateCommand(input);
                }
                else if (input.StartsWith("read ", StringComparison.OrdinalIgnoreCase))
                {
                    ExecuteReadCommand(input);
                }
                else if (lowerInput == "exit")
                {
                    Console.WriteLine("До свидания!");
                    break;
                }
                else
                {
                    Console.WriteLine("Неизвестная команда. Введите 'help'.\n");
                }
            }
        }
        }
                // Метод view принимает строку с флагами и выводит таблицу
        private static void ExecuteViewCommand(string input)
        {
            Console.WriteLine("\n--- Список ваших задач ---");
            if (_todoCount == 0)
            {
                Console.WriteLine("(список задач пуст)\n");
                return;
            }

            // Парсинг флагов отображения (Пункт 4)
            bool showIndex = input.Contains("--index") || input.Contains(" -i") || input.Contains("--all") || input.Contains(" -a");
            bool showStatus = input.Contains("--status") || input.Contains(" -s") || input.Contains("--all") || input.Contains(" -a");
            bool showDate = input.Contains("--update-date") || input.Contains(" -d") || input.Contains("--all") || input.Contains(" -a");

            for (int i = 0; i < _todoCount; i++)
            {
                // Обрезка текста до 30 символов для таблицы (Пункт 4)
                string text = _todos[i]?.Replace(Environment.NewLine, " ") ?? "";
                if (text.Length > 30)
                {
                    text = text.Substring(0, 30) + "...";
                }

                string row = text;

                // Сборка строки по флагам
                if (showIndex) row = $"[{i}] " + row;
                if (showStatus) row += " | " + (_statuses[i] ? "сделано" : "не сделано");
                if (showDate) row += $" | {_dates[i]:dd.MM.yyyy HH:mm:ss}";

                Console.WriteLine(row);
            }
            Console.WriteLine("--------------------------\n");
        }
        private static void ShowHelp()
        {

            // Обновили справку, чтобы новые команды отображались в меню
            Console.WriteLine("\nДоступные команды:");
            Console.WriteLine("  help                     - показать эту справку");
            Console.WriteLine("  profile                  - показать профиль пользователя");
            Console.WriteLine("  add <текст> [--multiline] - добавить задачу (опционально многострочно)");
            Console.WriteLine("  view [-i] [-s] [-d] [-a]  - показать задачи таблицей с флагами фильтрации");
            Console.WriteLine("  read <индекс>            - открыть полный текст конкретной задачи");
            Console.WriteLine("  status <индекс> <статус> - изменить статус задачи (NotStarted, InProgress, Completed, Postponed, Failed)");
            Console.WriteLine("  delete <индекс>          - удалить задачу");
            Console.WriteLine("  update <индекс> <текст>  - обновить текст задачи");
            Console.WriteLine("  exit                     - выйти из программы\n");
        }
                // Пункт 5: Стартовое меню авторизации и регистрации пользователей
        private static bool ShowAuthMenu()
        {
            while (AppInfo.CurrentProfileId == null)
            {
                Console.WriteLine("=== СИСТЕМА АВТОРИЗАЦИИ ===");
                Console.WriteLine("1. Войти в профиль");
                Console.WriteLine("2. Зарегистрировать новый профиль");
                Console.WriteLine("3. Выйти из программы");
                Console.Write("\nВыберите действие (1-3): ");
                
                string? choice = Console.ReadLine();
                if (choice == "1")
                {
                    Console.Write("Введите логин: ");
                    string? login = Console.ReadLine()?.Trim().ToLower();
                    Console.Write("Введите пароль: ");
                    string? password = Console.ReadLine();

                    var profile = AppInfo.AllProfiles.Find(p => p.Login == login && p.Password == password);
                    if (profile != null)
                    {
                        AppInfo.CurrentProfileId = profile.Id;
                        Console.WriteLine($"\nУспешный вход! Добро пожаловать, {profile.FirstName}!\n");
                        return true;
                    }
                    Console.WriteLine("\nОшибка: неверный логин или пароль.\n");
                }
                else if (choice == "2")
                {
                    Console.Write("Введите имя: ");
                    string firstName = Console.ReadLine()?.Trim() ?? "Имя";
                    Console.Write("Введите фамилию: ");
                    string lastName = Console.ReadLine()?.Trim() ?? "Фамилия";
                    Console.Write("Введите год рождения: ");
                    int.TryParse(Console.ReadLine(), out int year);
                    Console.Write("Придумайте логин: ");
                    string login = Console.ReadLine()?.Trim().ToLower() ?? "user";
                    Console.Write("Придумайте пароль: ");
                    string password = Console.ReadLine() ?? "123";

                    if (AppInfo.AllProfiles.Exists(p => p.Login == login))
                    {
                        Console.WriteLine("\nОшибка: пользователь с таким логином уже существует.\n");
                        continue;
                    }

                    var newProfile = new Profile(Guid.NewGuid(), login, password, firstName, lastName, year == 0 ? 2004 : year);
                    AppInfo.AllProfiles.Add(newProfile);
                    FileManager.SaveAllProfiles(AppInfo.AllProfiles, _profilePath);
                    Console.WriteLine("\nПрофиль успешно создан! Теперь вы можете войти.\n");
                }
                else if (choice == "3")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("\nНекорректный выбор. Попробуйте снова.\n");
                }
            }
            return true;
        }

                // --- Добавлен многострочный режим ввода для команды add с флагами ---
                private static void ExecuteAddCommand(string input)
        {
            // Проверяем флаги многострочного ввода (Пункт 3)
            bool isMultiline = input.Contains("--multiline") || input.Contains(" -m");
            string taskText = "";

            if (isMultiline)
            {
                Console.WriteLine("Введите строки задачи (для окончания введите !end):");
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                while (true)
                {
                    Console.Write("> ");
                    string? line = Console.ReadLine();
                    if (line == "!end") break;
                    if (line != null) sb.AppendLine(line);
                }
                taskText = sb.ToString().TrimEnd();
            }
            else
            {
                // Обычный однострочный режим
                string[] parts = input.Split(new[] { ' ' }, 2);
                if (parts.Length < 2 || string.IsNullOrWhiteSpace(parts[1]))
                {
                    Console.WriteLine("Ошибка: введите текст задачи после 'add'.\n");
                    return;
                }
                taskText = parts[1];
            }

            if (string.IsNullOrWhiteSpace(taskText))
            {
                Console.WriteLine("Ошибка: текст задачи не может быть пустым.\n");
                return;
            }
        }
        // --- В КЛАСС ДОБАВИЛИ НОВЫЕ МЕТОДЫ ---
        private static void ExecuteDoneCommand(string input)
        {
            if (int.TryParse(input.Substring(5).Trim(), out int index) && index >= 0 && index < _todoCount)
            {
                _statuses[index] = true;
                _dates[index] = DateTime.Now;
                Console.WriteLine($"Задача [{index}] выполнена.\n");
            }
            else
            {
                Console.WriteLine("Ошибка: некорректный индекс задачи.\n");
            }
        }

        private static void ExecuteDeleteCommand(string input)
        {
            if (int.TryParse(input.Substring(7).Trim(), out int index) && index >= 0 && index < _todoCount)
            {
                // Сдвиг элементов влево
                for (int i = index; i < _todoCount - 1; i++)
                {
                    _todos[i] = _todos[i + 1];
                    _statuses[i] = _statuses[i + 1];
                    _dates[i] = _dates[i + 1];
                }
                _todoCount--;
                Console.WriteLine($"Задача [{index}] удалена.\n");
            }
            else
            {
                Console.WriteLine("Ошибка: некорректный индекс задачи.\n");
            }
        }

        private static void ExecuteUpdateCommand(string input)
        {
            string args = input.Substring(7).Trim();
            string[] parts = args.Split(new[] { ' ' }, 2);
            if (parts.Length == 2 && int.TryParse(parts[0], out int index) && index >= 0 && index < _todoCount)
            {
                _todos[index] = parts[1].Trim('\"');
                _dates[index] = DateTime.Now;
                Console.WriteLine($"Задача [{index}] обновлена.\n");
            }
            else
            {
                Console.WriteLine("Ошибка: используйте формат 'update <индекс> <текст>'.\n");
            }
        }
            // Пункт 5: Метод для просмотра полного текста задачи (без обрезки)
        private static void ExecuteReadCommand(string input)
        {
            if (int.TryParse(input.Substring(5).Trim(), out int index) && index >= 0 && index < _todoCount)
            {
                string status = _statuses[index] ? "выполнена" : "не выполнена";
                Console.WriteLine($"\nПолный текст задачи:\n{_todos[index]}");
                Console.WriteLine($"Статус: {status}");
                Console.WriteLine($"Дата последнего изменения: {_dates[index]:dd.MM.yyyy HH:mm:ss}\n");
            }
            else
            {
                Console.WriteLine("Ошибка: некорректный индекс задачи.\n");
            }
        }
    }
}