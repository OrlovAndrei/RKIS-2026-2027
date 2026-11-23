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
}