using System.Globalization;
using System.Text;

namespace TodoListApp;

public static class FileManager
{
    public static readonly string DataDirectory =
        Path.Combine(AppContext.BaseDirectory, "data");

    public static readonly string ProfilePath =
        Path.Combine(DataDirectory, "profile.txt");

    public static readonly string TodoPath =
        Path.Combine(DataDirectory, "todo.csv");

    public static void EnsureDataDirectory(string dirPath)
    {
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
    }

    private static string EscapeCsv(string text)
    {
        return "\"" + text
            .Replace("\r", "")
            .Replace("\"", "\"\"")
            .Replace("\n", "\\n") + "\"";
    }

    private static string UnescapeCsv(string text)
    {
        if (text.Length >= 2 && text.StartsWith("\"") && text.EndsWith("\""))
        {
            text = text.Substring(1, text.Length - 2);
        }

        return text.Replace("\\n", "\n").Replace("\"\"", "\"");
    }

    private static List<string> SplitCsvLine(string line)
    {
        var fields = new List<string>();
        var current = new StringBuilder();
        bool inQuotes = false;

        foreach (char c in line)
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
                current.Append(c);
            }
            else if (c == ';' && !inQuotes)
            {
                fields.Add(current.ToString());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }

        fields.Add(current.ToString());
        return fields;
    }

    public static void SaveProfile(Profile profile, string filePath)
    {
        EnsureDataDirectory(Path.GetDirectoryName(filePath) ?? DataDirectory);

        string[] lines =
        {
            profile.FirstName,
            profile.LastName,
            profile.BirthYear.ToString()
        };

        File.WriteAllLines(filePath, lines);
    }

    public static Profile? LoadProfile(string filePath)
    {
        if (!File.Exists(filePath))
            return null;

        string[] lines = File.ReadAllLines(filePath);

        if (lines.Length < 3 || !int.TryParse(lines[2], out int birthYear))
            return null;

        return new Profile(lines[0], lines[1], birthYear);
    }

    public static void SaveTodos(TodoList todos, string filePath)
    {
        EnsureDataDirectory(Path.GetDirectoryName(filePath) ?? DataDirectory);

        var lines = new List<string>();

        for (int i = 0; i < todos.Count; i++)
        {
            TodoItem? item = todos.GetItem(i);
            if (item == null)
                continue;

            string status = item.Status.ToString();
            string date = item.LastModified.ToString("s", CultureInfo.InvariantCulture);

            lines.Add($"{i};{EscapeCsv(item.Text)};{status};{date}");
        }

        File.WriteAllLines(filePath, lines);
    }

    public static TodoList LoadTodos(string filePath)
    {
        var todos = new TodoList();

        if (!File.Exists(filePath))
            return todos;

        foreach (string line in File.ReadAllLines(filePath))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            List<string> fields = SplitCsvLine(line);
            if (fields.Count < 4)
                continue;

            string text = UnescapeCsv(fields[1]);

            TodoStatus status;

            if (!Enum.TryParse(fields[2], true, out status))
            {
                // Поддержка старых CSV-файлов с true/false.
                status = bool.TryParse(fields[2], out bool done) && done
                    ? TodoStatus.Completed
                    : TodoStatus.NotStarted;
            }

            DateTime lastModified = DateTime.TryParse(
                fields[3],
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime parsed)
                ? parsed
                : DateTime.Now;

            todos.Add(new TodoItem(text, status, lastModified));
        }

        return todos;
    }
}