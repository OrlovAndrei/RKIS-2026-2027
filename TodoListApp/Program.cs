namespace TodoListApp
{
    internal class Program
    {
        private const char TaskTextSeparator = '"';
        private const char CommandArgumentSeparator = ' ';

        private const string CommandHelp = "help";
        private const string CommandProfile = "profile";
        private const string CommandAdd = "add";
        private const string CommandView = "view";
        private const string CommandRead = "read";
        private const string CommandDone = "done";
        private const string CommandDelete = "delete";
        private const string CommandUpdate = "update";
        private const string CommandExit = "exit";

        private const string FlagMultilineLong = "--multiline";
        private const string FlagMultilineShort = "-m";
        private const string MultilinePrompt = "> ";
        private const string MultilineEndMarker = "!end";
        private const string MultilineJoiner = "\n";

        private const string FlagIndexLong = "--index";
        private const string FlagStatusLong = "--status";
        private const string FlagDateLong = "--update-date";
        private const string FlagAllLong = "--all";
        private const string FlagPrefix = "-";
        private const string LongFlagPrefix = "--";
        private const char LetterIndex = 'i';
        private const char LetterStatus = 's';
        private const char LetterDate = 'd';
        private const char LetterAll = 'a';

        private const string AddFormatMessage = "Формат: add \"текст задачи\" или add --multiline";
        private const string DoneFormatMessage = "Формат: done <номер задачи>";
        private const string DeleteFormatMessage = "Формат: delete <номер задачи>";
        private const string UpdateFormatMessage = "Формат: update <номер задачи> \"новый текст\"";
        private const string ReadFormatMessage = "Формат: read <номер задачи>";
        private const string ViewFormatMessage = "Неизвестный флаг. Формат: view [--index|-i] [--status|-s] [--update-date|-d] [--all|-a]";

        private static TodoList todoList = new TodoList();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Работу выполнил Добрынин");

            Console.Write("Введите имя: ");
            string firstName = Console.ReadLine() ?? "";

            Console.Write("Введите фамилию: ");
            string lastName = Console.ReadLine() ?? "";

            Console.Write("Введите год рождения: ");
            string birthYearText = Console.ReadLine() ?? "";

            int birthYear = int.Parse(birthYearText);
            int age = DateTime.Now.Year - birthYear;

            Console.WriteLine($"Добавлен пользователь {firstName} {lastName}, возраст - {age}");

            Profile profile = new Profile(firstName, lastName, birthYear);

            while (true)
            {
                Console.Write("Введите команду: ");
                string command = Console.ReadLine() ?? "";

                if (command == CommandExit)
                {
                    break;
                }

                if (command == CommandHelp)
                {
                    ShowHelp();
                    continue;
                }

                if (command == CommandProfile)
                {
                    ShowProfile(profile);
                    continue;
                }

                if (command.StartsWith(CommandAdd + " "))
                {
                    AddTodo(command);
                    continue;
                }

                if (command == CommandView || command.StartsWith(CommandView + " "))
                {
                    ShowTodos(command);
                    continue;
                }

                if (command.StartsWith(CommandRead + " "))
                {
                    ReadTodo(command);
                    continue;
                }

                if (command.StartsWith(CommandDone + " "))
                {
                    MarkTodoDone(command);
                    continue;
                }

                if (command.StartsWith(CommandDelete + " "))
                {
                    DeleteTodo(command);
                    continue;
                }

                if (command.StartsWith(CommandUpdate + " "))
                {
                    UpdateTodo(command);
                    continue;
                }

                Console.WriteLine("Неизвестная команда.");
            }
        }

        private static TodoItem? FindTodo(string numberText)
        {
            if (!int.TryParse(numberText, out int todoNumber))
            {
                return null;
            }

            return todoList.GetItem(todoNumber - 1);
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help - вывести список команд");
            Console.WriteLine("profile - показать данные пользователя");
            Console.WriteLine("add \"текст задачи\" - добавить задачу в одну строку");
            Console.WriteLine("add --multiline (или -m) - добавить многострочную задачу, ввод заканчивается строкой !end");
            Console.WriteLine("view - показать задачи в виде таблицы (только текст)");
            Console.WriteLine("view [флаги] - флаги можно комбинировать, например -is:");
            Console.WriteLine("  --index, -i - показать индекс задачи");
            Console.WriteLine("  --status, -s - показать статус задачи");
            Console.WriteLine("  --update-date, -d - показать дату последнего изменения");
            Console.WriteLine("  --all, -a - показать все данные");
            Console.WriteLine("read <номер> - показать полный текст задачи, статус и дату");
            Console.WriteLine("done <номер> - отметить задачу выполненной");
            Console.WriteLine("delete <номер> - удалить задачу");
            Console.WriteLine("update <номер> \"новый текст\" - изменить текст задачи");
            Console.WriteLine("exit - завершить программу");
        }

        private static void ShowProfile(Profile profile)
        {
            Console.WriteLine(profile.GetInfo());
        }

        private static bool IsMultilineAdd(string command)
        {
            string[] parts = command.Split(CommandArgumentSeparator, StringSplitOptions.RemoveEmptyEntries);
            return parts.Length == 2
                && parts[0] == CommandAdd
                && (parts[1] == FlagMultilineLong || parts[1] == FlagMultilineShort);
        }

        private static void AddTodo(string command)
        {
            if (IsMultilineAdd(command))
            {
                AddMultilineTodo();
                return;
            }

            AddSingleLineTodo(command);
        }

        private static void AddSingleLineTodo(string command)
        {
            string[] parts = command.Split(TaskTextSeparator);
            if (parts.Length < 2 || parts[1] == "")
            {
                Console.WriteLine(AddFormatMessage);
                return;
            }

            todoList.Add(new TodoItem(parts[1]));
            Console.WriteLine("Задача добавлена.");
        }

        private static void AddMultilineTodo()
        {
            Console.WriteLine($"Вводите строки задачи, для завершения введите {MultilineEndMarker}");

            List<string> lines = new List<string>();
            while (true)
            {
                Console.Write(MultilinePrompt);
                string? line = Console.ReadLine();
                if (line == null || line == MultilineEndMarker)
                {
                    break;
                }

                lines.Add(line);
            }

            if (lines.Count == 0)
            {
                Console.WriteLine("Задача не добавлена: не введено ни одной строки.");
                return;
            }

            todoList.Add(new TodoItem(string.Join(MultilineJoiner, lines)));
            Console.WriteLine("Задача добавлена.");
        }

        private static string ToShortFlag(string flag)
        {
            switch (flag)
            {
                case FlagIndexLong:
                    return FlagPrefix + LetterIndex;
                case FlagStatusLong:
                    return FlagPrefix + LetterStatus;
                case FlagDateLong:
                    return FlagPrefix + LetterDate;
                case FlagAllLong:
                    return FlagPrefix + LetterAll;
                default:
                    return flag;
            }
        }

        private static bool TryParseViewFlags(string command, out bool showIndex, out bool showStatus, out bool showDate)
        {
            showIndex = false;
            showStatus = false;
            showDate = false;

            string[] parts = command.Split(CommandArgumentSeparator, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 1; i < parts.Length; i++)
            {
                string flag = ToShortFlag(parts[i]);
                if (!flag.StartsWith(FlagPrefix) || flag.StartsWith(LongFlagPrefix) || flag.Length < 2)
                {
                    return false;
                }

                foreach (char letter in flag.Substring(1))
                {
                    if (letter == LetterIndex)
                    {
                        showIndex = true;
                    }
                    else if (letter == LetterStatus)
                    {
                        showStatus = true;
                    }
                    else if (letter == LetterDate)
                    {
                        showDate = true;
                    }
                    else if (letter == LetterAll)
                    {
                        showIndex = true;
                        showStatus = true;
                        showDate = true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static void ShowTodos(string command)
        {
            if (!TryParseViewFlags(command, out bool showIndex, out bool showStatus, out bool showDate))
            {
                Console.WriteLine(ViewFormatMessage);
                return;
            }

            todoList.View(showIndex, showStatus, showDate);
        }

        private static void ReadTodo(string command)
        {
            string[] parts = command.Split(CommandArgumentSeparator, StringSplitOptions.RemoveEmptyEntries);
            TodoItem? item = parts.Length == 2 ? FindTodo(parts[1]) : null;
            if (item == null)
            {
                Console.WriteLine(ReadFormatMessage);
                return;
            }

            Console.WriteLine(item.GetFullInfo());
        }

        private static void MarkTodoDone(string command)
        {
            string[] parts = command.Split(CommandArgumentSeparator, StringSplitOptions.RemoveEmptyEntries);
            TodoItem? item = parts.Length == 2 ? FindTodo(parts[1]) : null;
            if (item == null)
            {
                Console.WriteLine(DoneFormatMessage);
                return;
            }

            item.MarkDone();
            Console.WriteLine("Задача отмечена выполненной.");
        }

        private static void DeleteTodo(string command)
        {
            string[] parts = command.Split(CommandArgumentSeparator, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2 || !int.TryParse(parts[1], out int todoNumber) || !todoList.Delete(todoNumber - 1))
            {
                Console.WriteLine(DeleteFormatMessage);
                return;
            }

            Console.WriteLine("Задача удалена.");
        }

        private static void UpdateTodo(string command)
        {
            string[] quoteParts = command.Split(TaskTextSeparator);
            if (quoteParts.Length < 2 || quoteParts[1] == "")
            {
                Console.WriteLine(UpdateFormatMessage);
                return;
            }

            string[] headParts = quoteParts[0].Split(CommandArgumentSeparator, StringSplitOptions.RemoveEmptyEntries);
            TodoItem? item = headParts.Length == 2 ? FindTodo(headParts[1]) : null;
            if (item == null)
            {
                Console.WriteLine(UpdateFormatMessage);
                return;
            }

            item.UpdateText(quoteParts[1]);
            Console.WriteLine("Задача обновлена.");
        }
    }
}