namespace TodoListApp
{
    internal class Program
    {
        private const int InitialTodosCapacity = 2;
        private const int CapacityGrowthFactor = 2;
        private const char TaskTextSeparator = '"';
        private const char CommandArgumentSeparator = ' ';

        private const string CommandHelp = "help";
        private const string CommandProfile = "profile";
        private const string CommandAdd = "add";
        private const string CommandView = "view";
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

        private const string ColumnSeparator = " | ";
        private const string DateFormat = "dd.MM.yyyy HH:mm";
        private const string ViewFormatMessage = "Неизвестный флаг. Формат: view [--index|-i] [--status|-s] [--update-date|-d] [--all|-a]";

        private const int MaxTextLength = 30;
        private const string TruncationSuffix = "...";
        private const string HeaderIndex = "№";
        private const string HeaderText = "Задача";
        private const string HeaderStatus = "Статус";
        private const string HeaderDate = "Дата изменения";
        private const char HeaderLineChar = '-';

        private const string StatusDone = "сделано";
        private const string StatusNotDone = "не сделано";

        private const string UpdateFormatMessage = "Формат: update <номер задачи> \"новый текст\"";

        private static string[] todos = new string[InitialTodosCapacity];
        private static bool[] statuses = new bool[InitialTodosCapacity];
        private static DateTime[] dates = new DateTime[InitialTodosCapacity];
        private static int todoCount = 0;

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
                    ShowProfile(firstName, lastName, birthYear);
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

        private static T[] ExpandArray<T>(T[] array)
        {
            T[] expandedArray = new T[array.Length * CapacityGrowthFactor];
            for (int i = 0; i < array.Length; i++)
            {
                expandedArray[i] = array[i];
            }
            return expandedArray;
        }

        private static void ExpandAllArrays()
        {
            todos = ExpandArray(todos);
            statuses = ExpandArray(statuses);
            dates = ExpandArray(dates);
        }

        private static bool TryParseTodoIndex(string numberText, out int index)
        {
            index = -1;
            if (!int.TryParse(numberText, out int todoNumber) || todoNumber < 1 || todoNumber > todoCount)
            {
                return false;
            }

            index = todoNumber - 1;
            return true;
        }

        private static void SaveTodo(string text)
        {
            if (todoCount == todos.Length)
            {
                ExpandAllArrays();
            }

            todos[todoCount] = text;
            statuses[todoCount] = false;
            dates[todoCount] = DateTime.Now;
            todoCount++;
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("help - вывести список команд");
            Console.WriteLine("profile - показать данные пользователя");
            Console.WriteLine("add \"текст задачи\" - добавить задачу");
            Console.WriteLine("view - показать все задачи");
            Console.WriteLine("done <номер> - отметить задачу выполненной");
            Console.WriteLine("delete <номер> - удалить задачу");
            Console.WriteLine("update <номер> \"новый текст\" - изменить текст задачи");
            Console.WriteLine("exit - завершить программу");
        }

        private static void ShowProfile(string firstName, string lastName, int birthYear)
        {
            Console.WriteLine($"{firstName} {lastName}, {birthYear}");
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
                Console.WriteLine("Формат: add \"текст задачи\" или add --multiline");
                return;
            }

            SaveTodo(parts[1]);
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

            SaveTodo(string.Join(MultilineJoiner, lines));
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

                private static string TruncateText(string text)
        {
            string singleLine = text.Replace(MultilineJoiner, " ");
            if (singleLine.Length <= MaxTextLength)
            {
                return singleLine;
            }

            return singleLine.Substring(0, MaxTextLength) + TruncationSuffix;
        }

        private static void PrintRow(List<string> cells, int[] widths)
        {
            List<string> paddedCells = new List<string>();
            for (int column = 0; column < cells.Count; column++)
            {
                paddedCells.Add(cells[column].PadRight(widths[column]));
            }

            Console.WriteLine(string.Join(ColumnSeparator, paddedCells));
        }

        private static void PrintTable(List<string> headers, List<List<string>> rows)
        {
            int[] widths = new int[headers.Count];
            for (int column = 0; column < headers.Count; column++)
            {
                widths[column] = headers[column].Length;
                foreach (List<string> row in rows)
                {
                    if (row[column].Length > widths[column])
                    {
                        widths[column] = row[column].Length;
                    }
                }
            }

            PrintRow(headers, widths);

            int totalWidth = widths.Sum() + ColumnSeparator.Length * (widths.Length - 1);
            Console.WriteLine(new string(HeaderLineChar, totalWidth));

            foreach (List<string> row in rows)
            {
                PrintRow(row, widths);
            }
        }

        private static void ShowTodos(string command)
        {
            if (!TryParseViewFlags(command, out bool showIndex, out bool showStatus, out bool showDate))
            {
                Console.WriteLine(ViewFormatMessage);
                return;
            }

            if (todoCount == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }

            List<string> headers = new List<string>();
            if (showIndex)
            {
                headers.Add(HeaderIndex);
            }

            headers.Add(HeaderText);

            if (showStatus)
            {
                headers.Add(HeaderStatus);
            }

            if (showDate)
            {
                headers.Add(HeaderDate);
            }

            List<List<string>> rows = new List<List<string>>();
            for (int i = 0; i < todoCount; i++)
            {
                List<string> cells = new List<string>();
                if (showIndex)
                {
                    cells.Add((i + 1).ToString());
                }

                cells.Add(TruncateText(todos[i]));

                if (showStatus)
                {
                    cells.Add(statuses[i] ? StatusDone : StatusNotDone);
                }

                if (showDate)
                {
                    cells.Add(dates[i].ToString(DateFormat));
                }

                rows.Add(cells);
            }

            PrintTable(headers, rows);
        }

        private static void MarkTodoDone(string command)
        {
            string[] parts = command.Split(CommandArgumentSeparator);
            if (parts.Length != 2 || !TryParseTodoIndex(parts[1], out int index))
            {
                Console.WriteLine("Формат: done <номер задачи>");
                return;
            }

            statuses[index] = true;
            dates[index] = DateTime.Now;
            Console.WriteLine("Задача отмечена выполненной.");
        }

        private static void DeleteTodo(string command)
        {
            string[] parts = command.Split(CommandArgumentSeparator);
            if (parts.Length != 2 || !TryParseTodoIndex(parts[1], out int index))
            {
                Console.WriteLine("Формат: delete <номер задачи>");
                return;
            }

            for (int i = index; i < todoCount - 1; i++)
            {
                todos[i] = todos[i + 1];
                statuses[i] = statuses[i + 1];
                dates[i] = dates[i + 1];
            }

            todos[todoCount - 1] = "";
            statuses[todoCount - 1] = false;
            dates[todoCount - 1] = default;
            todoCount--;
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

            string[] headParts = quoteParts[0].Trim().Split(CommandArgumentSeparator);
            if (headParts.Length != 2 || !TryParseTodoIndex(headParts[1], out int index))
            {
                Console.WriteLine(UpdateFormatMessage);
                return;
            }

            todos[index] = quoteParts[1];
            dates[index] = DateTime.Now;
            Console.WriteLine("Задача обновлена.");
        }
    }
}