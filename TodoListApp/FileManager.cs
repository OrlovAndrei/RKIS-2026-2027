using System.Globalization;

// Всё, что связано с чтением и записью файлов, собрано здесь
public static class FileManager
{
    // Формат даты в CSV, например 2025-08-21T14:30:00
    private const string DateFormat = "yyyy-MM-ddTHH:mm:ss";

    // Данные лежат в папке data рядом с исполняемым файлом (внутри bin), поэтому в git не попадают
    public static string DataDirectory { get; } = Path.Combine(AppContext.BaseDirectory, "data");
    public static string ProfilesFilePath { get; } = Path.Combine(DataDirectory, "profile.csv");

    // У каждого пользователя свой файл с задачами: todos_<Id>.csv
    public static string GetTodosFilePath(Guid profileId)
    {
        return Path.Combine(DataDirectory, $"todos_{profileId}.csv");
    }

    // Если папки для данных ещё нет — создаём
    public static void EnsureDataDirectory(string dirPath)
    {
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);
    }

    // Один профиль — одна строка: Id;Login;Password;FirstName;LastName;BirthYear
    // (поля не содержат ';', это проверяется при вводе)
    public static void SaveProfiles(List<Profile> profiles, string filePath)
    {
        var lines = new string[profiles.Count];
        for (int i = 0; i < profiles.Count; i++)
        {
            Profile p = profiles[i];
            lines[i] = $"{p.Id};{p.Login};{p.Password};{p.FirstName};{p.LastName};{p.BirthYear}";
        }
        File.WriteAllLines(filePath, lines);
    }

    // Если файла нет — возвращаем пустой список. Повреждённые строки пропускаем и говорим об этом
    public static List<Profile> LoadProfiles(string filePath)
    {
        var profiles = new List<Profile>();
        if (!File.Exists(filePath))
            return profiles;

        foreach (string line in File.ReadAllLines(filePath))
        {
            if (line.Trim().Length == 0)
                continue;

            string[] parts = line.Split(';');
            if (parts.Length != 6 || !Guid.TryParse(parts[0], out Guid id) || !int.TryParse(parts[5], out int birthYear))
            {
                Console.WriteLine($"Профиль пропущен (неверный формат): {line}");
                continue;
            }
            profiles.Add(new Profile(id, parts[1], parts[2], parts[3], parts[4], birthYear));
        }
        return profiles;
    }

    // Обработчик событий TodoList: сохраняет задачи текущего пользователя в его файл.
    // Сама задача из события здесь не нужна, но у обработчика должна быть та же сигнатура, что у события
    public static void SaveTodoList(TodoItem item)
    {
        if (AppInfo.CurrentProfileId is Guid id)
            SaveTodos(AppInfo.Todos, GetTodosFilePath(id));
    }

    // Одна задача — одна строка: Index;"Text";Status;LastUpdate
    public static void SaveTodos(IEnumerable<TodoItem> todos, string filePath)
    {
        var lines = new List<string>();
        int index = 0;
        foreach (TodoItem item in todos)
        {
            string date = item.LastUpdate.ToString(DateFormat, CultureInfo.InvariantCulture);
            lines.Add($"{index};{EscapeCsv(item.Text)};{item.Status};{date}");
            index++;
        }
        File.WriteAllLines(filePath, lines);
    }

    // Если файла нет — возвращаем пустой список. Повреждённые строки пропускаем и говорим об этом
    public static List<TodoItem> LoadTodos(string filePath)
    {
        var todos = new List<TodoItem>();
        if (!File.Exists(filePath))
            return todos;

        foreach (string line in File.ReadAllLines(filePath))
        {
            if (line.Trim().Length == 0)
                continue;

            TodoItem? item = ParseTodoLine(line);
            if (item is null)
                Console.WriteLine($"Строка пропущена (неверный формат): {line}");
            else
                todos.Add(item);
        }
        return todos;
    }

    // Текст задачи может содержать ';', поэтому режем строку с двух концов:
    // слева стоит индекс, а справа — Status и LastUpdate. Всё, что между ними, — текст
    private static TodoItem? ParseTodoLine(string line)
    {
        int first = line.IndexOf(';');
        int last = line.LastIndexOf(';');
        int middle = last > 0 ? line.LastIndexOf(';', last - 1) : -1;
        if (first < 0 || middle <= first)
            return null;

        string text = UnescapeCsv(line.Substring(first + 1, middle - first - 1));
        string statusText = line.Substring(middle + 1, last - middle - 1);
        string dateText = line.Substring(last + 1);

        if (text.Length == 0)
            return null;
        // TryParse принимает и числа ("7"), поэтому дополнительно проверяем, что такой статус есть
        if (!Enum.TryParse(statusText, out TodoStatus status) || !Enum.IsDefined(status))
            return null;
        if (!DateTime.TryParseExact(dateText, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime lastUpdate))
            return null;

        return new TodoItem(text, status, lastUpdate);
    }

    // Текст берём в кавычки, кавычки внутри удваиваем, а перенос строки заменяем на \n
    private static string EscapeCsv(string text)
    {
        return "\"" + text.Replace("\"", "\"\"").Replace("\n", "\\n") + "\"";
    }

    private static string UnescapeCsv(string text)
    {
        // Снимаем только одну внешнюю пару кавычек, чтобы не съесть кавычки самого текста
        if (text.Length >= 2 && text.StartsWith('"') && text.EndsWith('"'))
            text = text.Substring(1, text.Length - 2);

        return text.Replace("\\n", "\n").Replace("\"\"", "\"");
    }
}
