using System.Globalization;

namespace TodoListApp
{
    internal static class FileManager
    {
        private const string DataDirectoryName = "data";
        private const string ProfileFileName = "profile.txt";
        private const string TodoFileName = "todo.csv";
        private const int ProfileLinesCount = 3;

        private const string TodoHeader = "Index;Text;IsDone;LastUpdate";
        private const int HeaderLinesCount = 1;
        private const char FieldSeparator = ';';
        private const string Quote = "\"";
        private const string EscapedQuote = "\"\"";
        private const string NewLine = "\n";
        private const string EscapedNewLine = "\\n";
        private const string DateTimeFormat = "yyyy-MM-dd'T'HH:mm:ss";

        public static string DataDirectoryPath => Path.Combine(AppContext.BaseDirectory, DataDirectoryName);

        public static string ProfileFilePath => Path.Combine(DataDirectoryPath, ProfileFileName);

        public static string TodoFilePath => Path.Combine(DataDirectoryPath, TodoFileName);

        public static void EnsureDataDirectory(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }

        public static void SaveProfile(Profile profile, string filePath)
        {
            if (profile == null)
            {
                return;
            }

            string[] lines = { profile.FirstName, profile.LastName, profile.BirthYear.ToString() };
            File.WriteAllLines(filePath, lines);
        }

        public static Profile? LoadProfile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length < ProfileLinesCount || !int.TryParse(lines[2], out int birthYear))
            {
                return null;
            }

            return new Profile(lines[0], lines[1], birthYear);
        }

        public static void SaveTodos(TodoList todos, string filePath)
        {
            if (todos == null)
            {
                return;
            }

            List<string> lines = new List<string> { TodoHeader };
            for (int i = 0; i < todos.Count; i++)
            {
                TodoItem? item = todos.GetItem(i);
                if (item == null)
                {
                    continue;
                }

                string text = item.Text.Replace(Quote, EscapedQuote).Replace(NewLine, EscapedNewLine);
                string isDone = item.IsDone.ToString().ToLowerInvariant();
                string lastUpdate = item.LastUpdate.ToString(DateTimeFormat, CultureInfo.InvariantCulture);

                lines.Add($"{i}{FieldSeparator}{Quote}{text}{Quote}{FieldSeparator}{isDone}{FieldSeparator}{lastUpdate}");
            }

            File.WriteAllLines(filePath, lines);
        }

        public static TodoList LoadTodos(string filePath)
        {
            TodoList todos = new TodoList();
            if (!File.Exists(filePath))
            {
                return todos;
            }

            string[] lines = File.ReadAllLines(filePath);
            for (int i = HeaderLinesCount; i < lines.Length; i++)
            {
                TodoItem? item = ParseTodoLine(lines[i]);
                if (item != null)
                {
                    todos.Add(item);
                }
            }

            return todos;
        }

        private static TodoItem? ParseTodoLine(string line)
        {
            int firstSeparator = line.IndexOf(FieldSeparator);
            int lastSeparator = line.LastIndexOf(FieldSeparator);
            if (firstSeparator < 0 || lastSeparator <= firstSeparator)
            {
                return null;
            }

            int middleSeparator = line.LastIndexOf(FieldSeparator, lastSeparator - 1);
            if (middleSeparator <= firstSeparator)
            {
                return null;
            }

            string textPart = line.Substring(firstSeparator + 1, middleSeparator - firstSeparator - 1);
            string isDonePart = line.Substring(middleSeparator + 1, lastSeparator - middleSeparator - 1);
            string datePart = line.Substring(lastSeparator + 1);

            if (textPart.Length < 2 || !textPart.StartsWith(Quote) || !textPart.EndsWith(Quote))
            {
                return null;
            }

            if (!bool.TryParse(isDonePart, out bool isDone))
            {
                return null;
            }

            if (!DateTime.TryParseExact(datePart, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime lastUpdate))
            {
                return null;
            }

            string text = textPart.Substring(1, textPart.Length - 2)
                .Replace(EscapedQuote, Quote)
                .Replace(EscapedNewLine, NewLine);

            return new TodoItem(text, isDone, lastUpdate);
        }
    }
}