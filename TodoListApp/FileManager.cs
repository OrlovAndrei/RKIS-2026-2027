using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

// Сохраняет профиль в TXT, задачи в CSV и восстанавливает их при запуске.
public static class FileManager
{
    public static string TodoFilePath { get; set; } = "";

    public static void EnsureDataDirectory(string dirPath)
    {
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);
    }

    public static void SaveProfile(Profile profile, string filePath)
    {
        if (profile == null) throw new ArgumentNullException(nameof(profile));
        string? dir = Path.GetDirectoryName(Path.GetFullPath(filePath));
        if (dir != null) EnsureDataDirectory(dir);
        if (!File.Exists(filePath)) File.WriteAllText(filePath, "");

        File.WriteAllLines(filePath, new[]
        {
            profile.FirstName,
            profile.LastName,
            profile.BirthYear.ToString(CultureInfo.InvariantCulture)
        });
    }

    public static Profile LoadProfile(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("Файл профиля не найден", filePath);

        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length != 3 ||
            !int.TryParse(lines[2], NumberStyles.Integer,
                CultureInfo.InvariantCulture, out int year))
            throw new InvalidDataException("Неверный формат profile.txt");

        return new Profile(lines[0], lines[1], year);
    }

    public static void SaveTodos(TodoList todos, string filePath)
    {
        if (todos == null) throw new ArgumentNullException(nameof(todos));
        string? dir = Path.GetDirectoryName(Path.GetFullPath(filePath));
        if (dir != null) EnsureDataDirectory(dir);
        if (!File.Exists(filePath)) File.WriteAllText(filePath, "");

        List<string> lines = new List<string> { "Index;Text;IsDone;LastUpdate" };
        for (int i = 0; i < todos.Count; i++)
        {
            TodoItem? item = todos.GetItem(i);
            if (item == null) continue;
            string text = EscapeCsv(item.Text);
            string status = item.IsDone ? "true" : "false";
            string updated = item.LastUpdate.ToString("O", CultureInfo.InvariantCulture);
            lines.Add($"{i};{text};{status};{updated}");
        }
        File.WriteAllLines(filePath, lines);
    }

    public static TodoList LoadTodos(string filePath)
    {
        TodoList todos = new TodoList();
        if (!File.Exists(filePath)) return todos;

        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length == 0)
            return todos;
        if (lines[0] != "Index;Text;IsDone;LastUpdate")
            throw new InvalidDataException("Неверный заголовок todo.csv");

        for (int lineNo = 1; lineNo < lines.Length; lineNo++)
        {
            List<string> parts = SplitCsvLine(lines[lineNo]);
            if (parts.Count != 4 ||
                !int.TryParse(parts[0], out int index) || index != todos.Count ||
                !bool.TryParse(parts[2], out bool isDone) ||
                !DateTime.TryParse(parts[3], CultureInfo.InvariantCulture,
                    DateTimeStyles.RoundtripKind, out DateTime updated))
                throw new InvalidDataException($"Ошибка формата todo.csv, строка {lineNo + 1}");

            string text = UnescapeCsv(parts[1]);
            todos.Add(new TodoItem(text, isDone, updated));
        }
        return todos;
    }

    // Защита текста с ;, кавычками, обратным слэшем и переносами строк.
    private static string EscapeCsv(string text)
    {
        return "\"" + text.Replace("\\", "\\\\")
            .Replace("\r", "\\r")
            .Replace("\n", "\\n")
            .Replace("\"", "\"\"") + "\"";
    }

    // Поочерёдно восстанавливаем \n, \r и \\; обычный текст не меняем.
    private static string UnescapeCsv(string text)
    {
        StringBuilder result = new StringBuilder();
        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '\\' && i + 1 < text.Length)
            {
                if (text[i + 1] == 'n') { result.Append('\n'); i++; continue; }
                if (text[i + 1] == 'r') { result.Append('\r'); i++; continue; }
                if (text[i + 1] == '\\') { result.Append('\\'); i++; continue; }
            }
            result.Append(text[i]);
        }
        return result.ToString();
    }

    // Делим CSV по ; только за пределами кавычек.
    private static List<string> SplitCsvLine(string line)
    {
        List<string> parts = new List<string>();
        StringBuilder value = new StringBuilder();
        bool inQuotes = false;
        for (int i = 0; i < line.Length; i++)
        {
            char ch = line[i];
            if (ch == '"')
            {
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    value.Append('"');
                    i++;
                }
                else inQuotes = !inQuotes;
            }
            else if (ch == ';' && !inQuotes)
            {
                parts.Add(value.ToString());
                value.Clear();
            }
            else value.Append(ch);
        }
        if (inQuotes) throw new InvalidDataException("Незакрытая кавычка в todo.csv");
        parts.Add(value.ToString());
        return parts;
    }
}
