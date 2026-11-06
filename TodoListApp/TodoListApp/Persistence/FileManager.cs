using System.Globalization;
using TodoListApp.Models;

namespace TodoListApp.Persistence;

internal static class FileManager
{
    private const string ProfileHeader = "Id;Login;Password;FirstName;LastName;BirthYear";
    private const string TodoHeader = "Index;Text;Status;LastUpdate";

    public static string DataDirectory =>
        Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../user-data"));

    private static string ProfilePath => Path.Combine(DataDirectory, "profile.csv");

    public static void EnsureDataDirectory()
    {
        if (!Directory.Exists(DataDirectory))
        {
            Directory.CreateDirectory(DataDirectory);
        }
    }

    public static List<Profile> LoadProfiles()
    {
        EnsureDataDirectory();
        if (!File.Exists(ProfilePath))
        {
            File.WriteAllText(ProfilePath, ProfileHeader + Environment.NewLine);
            return new List<Profile>();
        }

        List<Profile> profiles = new();
        string[] lines = File.ReadAllLines(ProfilePath);
        for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
        {
            string line = lines[lineNumber];
            if (string.IsNullOrWhiteSpace(line)
                || (lineNumber == 0 && line.StartsWith("Id;", StringComparison.Ordinal)))
            {
                continue;
            }

            List<string> fields = CsvRow.Parse(line);
            if (fields.Count != 6
                || !Guid.TryParse(fields[0], out Guid id)
                || !int.TryParse(fields[5], NumberStyles.Integer, CultureInfo.InvariantCulture, out int year))
            {
                throw new FormatException($"Некорректная запись профиля в {ProfilePath}, строка {lineNumber + 1}.");
            }

            profiles.Add(Profile.FromStorage(id, fields[1], fields[2], fields[3], fields[4], year));
        }

        return profiles;
    }

    public static void SaveProfiles(IEnumerable<Profile> profiles)
    {
        EnsureDataDirectory();
        List<string> lines = new() { ProfileHeader };
        lines.AddRange(profiles.Select(profile => CsvRow.Format(
            profile.Id.ToString("D"),
            profile.Login,
            profile.Password,
            profile.FirstName,
            profile.LastName,
            profile.BirthYear.ToString(CultureInfo.InvariantCulture))));
        File.WriteAllLines(ProfilePath, lines);
    }

    public static TodoList LoadTodos(Guid profileId)
    {
        EnsureDataDirectory();
        string path = GetTodoPath(profileId);
        if (!File.Exists(path))
        {
            File.WriteAllText(path, TodoHeader + Environment.NewLine);
            return new TodoList();
        }

        TodoList todos = new();
        string[] lines = File.ReadAllLines(path);
        for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
        {
            string line = lines[lineNumber];
            if (string.IsNullOrWhiteSpace(line)
                || (lineNumber == 0 && line.StartsWith("Index;", StringComparison.Ordinal)))
            {
                continue;
            }

            List<string> fields = CsvRow.Parse(line);
            if (fields.Count != 4
                || !int.TryParse(fields[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out int id)
                || !Enum.TryParse(fields[2], ignoreCase: true, out TodoStatus status)
                || !Enum.IsDefined(status)
                || !DateTime.TryParse(fields[3], CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime updatedAt))
            {
                throw new FormatException($"Некорректная запись задачи в {path}, строка {lineNumber + 1}.");
            }

            todos.Add(new TodoItem(new TaskId(id), fields[1], status, updatedAt));
        }

        return todos;
    }

    public static void SaveTodos(TodoList todos, Guid profileId)
    {
        EnsureDataDirectory();
        List<string> lines = new() { TodoHeader };
        lines.AddRange(todos.Select(item => CsvRow.Format(
            item.Id.Value.ToString(CultureInfo.InvariantCulture),
            item.Text,
            item.Status.ToString(),
            item.LastUpdate.ToString("O", CultureInfo.InvariantCulture))));
        File.WriteAllLines(GetTodoPath(profileId), lines);
    }

    public static void SaveTodoList(TodoItem _)
    {
        if (AppInfo.CurrentProfileId is not Guid profileId)
        {
            throw new InvalidOperationException("Нельзя сохранить задачи без активного профиля.");
        }

        SaveTodos(AppInfo.Todos, profileId);
    }

    private static string GetTodoPath(Guid profileId) =>
        Path.Combine(DataDirectory, $"todos_{profileId:D}.csv");
}

internal static class CsvRow
{
    public static string Format(params string[] values)
    {
        return string.Join(";", values.Select(Escape));
    }

    public static List<string> Parse(string row)
    {
        List<string> fields = new();
        System.Text.StringBuilder field = new();
        bool quoted = false;

        for (int i = 0; i < row.Length; i++)
        {
            char current = row[i];
            if (current == '"')
            {
                if (quoted && i + 1 < row.Length && row[i + 1] == '"')
                {
                    field.Append('"');
                    i++;
                }
                else
                {
                    quoted = !quoted;
                }
            }
            else if (current == ';' && !quoted)
            {
                fields.Add(Unescape(field.ToString()));
                field.Clear();
            }
            else
            {
                field.Append(current);
            }
        }

        if (quoted)
        {
            throw new FormatException("В CSV-записи не закрыты кавычки.");
        }

        fields.Add(Unescape(field.ToString()));
        return fields;
    }

    private static string Escape(string value)
    {
        string normalized = value
            .Replace("\r\n", "\n")
            .Replace('\r', '\n')
            .Replace("\\", "\\\\", StringComparison.Ordinal)
            .Replace("\n", "\\n", StringComparison.Ordinal);
        return "\"" + normalized.Replace("\"", "\"\"") + "\"";
    }

    private static string Unescape(string value)
    {
        System.Text.StringBuilder result = new();
        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] == '\\' && i + 1 < value.Length)
            {
                char next = value[++i];
                result.Append(next == 'n' ? '\n' : next);
            }
            else
            {
                result.Append(value[i]);
            }
        }

        return result.ToString();
    }
}
