using System;
using System.IO;
using System.Text;

namespace TodoListApp;

public static class FileManager
{
    public static void EnsureDataDirectory(string dirPath)
    {
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
    }

    public static void SaveProfile(Profile profile, string filePath)
    {
        string content = $"{profile.FirstName};{profile.LastName};{profile.BirthYear}";
        File.WriteAllText(filePath, content, Encoding.UTF8);
    }

    public static Profile LoadProfile(string filePath)
    {
        if (!File.Exists(filePath)) return null!;

        string content = File.ReadAllText(filePath, Encoding.UTF8);
        string[] parts = content.Split(';');
        if (parts.Length < 3) return null!;

        int.TryParse(parts[2], out int birthYear);
        return new Profile(parts[0], parts[1], birthYear);
    }

    public static void SaveTodos(TodoList todos, string filePath)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Index;Text;IsDone;LastUpdate");

        for (int i = 0; i < todos.Count; i++)
        {
            TodoItem? item = todos.GetItem(i);
            if (item == null) continue;

            string escapedText = EscapeCsv(item.Text);
            string isDoneStr = item.IsDone.ToString().ToLower();
            string dateStr = item.LastUpdate.ToString("yyyy-MM-ddTHH:mm:ss");

            sb.AppendLine($"{i};{escapedText};{isDoneStr};{dateStr}");
        }

        File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
    }

    public static TodoList LoadTodos(string filePath)
    {
        TodoList list = new TodoList();
        if (!File.Exists(filePath)) return list;

        string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;
            string[] parts = SplitCsvLine(lines[i]);
            if (parts.Length < 4) continue;

            string text = UnescapeCsv(parts[1]);
            bool.TryParse(parts[2], out bool isDone);
            DateTime.TryParse(parts[3], out DateTime lastUpdate);

            list.Add(new TodoItem(text, isDone, lastUpdate));
        }

        return list;
    }

    private static string EscapeCsv(string text) =>
        "\"" + text.Replace("\"", "\"\"").Replace("\n", "\\n") + "\"";

    private static string UnescapeCsv(string text) =>
        text.Trim('"').Replace("\\n", "\n").Replace("\"\"", "\"");

    private static string[] SplitCsvLine(string line)
    {
        int firstSemi = line.IndexOf(';');
        int lastSemi = line.LastIndexOf(';');
        int secondLastSemi = lastSemi > 0 ? line.LastIndexOf(';', lastSemi - 1) : -1;

        if (firstSemi == -1 || secondLastSemi <= firstSemi) return Array.Empty<string>();

        return new[]
        {
            line.Substring(0, firstSemi),
            line.Substring(firstSemi + 1, secondLastSemi - firstSemi - 1),
            line.Substring(secondLastSemi + 1, lastSemi - secondLastSemi - 1),
            line.Substring(lastSemi + 1)
        };
    }
}
