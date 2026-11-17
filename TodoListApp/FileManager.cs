using System;
using System.IO;
using System.Text;

static class FileManager
{
    public static void EnsureDataDirectory(string dirPath)
    {
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
    }

    public static void SaveProfile(Profile? profile, string filePath)
    {
        if (profile == null)
        {
            return;
        }

        string? dir = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(dir))
        {
            EnsureDataDirectory(dir);
        }

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "");
        }

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
        {
            return null;
        }

        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length < 3)
        {
            return null;
        }

        if (int.TryParse(lines[2], out int birthYear))
        {
            return new Profile(lines[0], lines[1], birthYear);
        }

        return null;
    }

    public static void SaveTodos(TodoList todos, string filePath)
    {
        string? dir = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(dir))
        {
            EnsureDataDirectory(dir);
        }

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "");
        }

        string[] lines = new string[todos.Count + 1];
        lines[0] = "Index;Text;IsDone;LastUpdate";

        for (int i = 0; i < todos.Count; i++)
        {
            TodoItem? item = todos.GetItem(i);
            if (item == null)
            {
                lines[i + 1] = "";
                continue;
            }

            string escapedText = EscapeCsv(item.Text);
            string isDone = item.IsDone ? "true" : "false";
            string date = item.LastUpdate.ToString("yyyy-MM-ddTHH:mm:ss");
            lines[i + 1] = $"{i};{escapedText};{isDone};{date}";
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
        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("Index;"))
            {
                continue;
            }

            string[] parts = ParseCsvLine(line);
            if (parts.Length < 4 || string.IsNullOrEmpty(parts[3]))
            {
                continue;
            }

            string text = UnescapeCsv(parts[1]);
            bool isDone = bool.TryParse(parts[2], out bool parsedDone) && parsedDone;
            DateTime lastUpdate = DateTime.TryParse(parts[3], out DateTime parsedDate)
                ? parsedDate
                : DateTime.Now;

            todos.Add(new TodoItem(text, isDone, lastUpdate));
        }

        return todos;
    }

    private static string EscapeCsv(string text)
    {
        string normalized = text.Replace("\r\n", "\n");
        return "\"" + normalized.Replace("\"", "\"\"").Replace("\n", "\\n") + "\"";
    }

    private static string UnescapeCsv(string text)
    {
        string value = text;
        if (value.StartsWith("\"") && value.EndsWith("\"") && value.Length >= 2)
        {
            value = value.Substring(1, value.Length - 2);
        }
        return value.Replace("\\n", "\n").Replace("\"\"", "\"");
    }

    private static string[] ParseCsvLine(string line)
    {
        string[] parts = { "", "", "", "" };
        bool inQuotes = false;
        int partIndex = 0;
        StringBuilder current = new StringBuilder();

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                inQuotes = !inQuotes;
                current.Append(c);
            }
            else if (c == ';' && !inQuotes && partIndex < 3)
            {
                parts[partIndex] = current.ToString();
                partIndex++;
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }

        parts[partIndex] = current.ToString();
        return parts;
    }
}
