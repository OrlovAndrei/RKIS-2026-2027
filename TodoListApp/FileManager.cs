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
}