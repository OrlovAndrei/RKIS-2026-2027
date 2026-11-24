using System.Globalization;

// Всё, что связано с чтением и записью файлов, собрано здесь
public static class FileManager
{
    // Формат даты в CSV, например 2025-08-21T14:30:00
    private const string DateFormat = "yyyy-MM-ddTHH:mm:ss";

    // Данные лежат в папке data рядом с исполняемым файлом (внутри bin), поэтому в git не попадают
    public static string DataDirectory { get; } = Path.Combine(AppContext.BaseDirectory, "data");
    public static string ProfileFilePath { get; } = Path.Combine(DataDirectory, "profile.txt");
    public static string TodoFilePath { get; } = Path.Combine(DataDirectory, "todo.csv");

    // Если папки для данных ещё нет — создаём
    public static void EnsureDataDirectory(string dirPath)
    {
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);
    }

    // Профиль хранится тремя строками: имя, фамилия, год рождения
    public static void SaveProfile(Profile profile, string filePath)
    {
        string[] lines = { profile.FirstName, profile.LastName, profile.BirthYear.ToString() };
        File.WriteAllLines(filePath, lines);
    }

    // Возвращает null, если файла нет или он пустой/повреждён — тогда программа попросит зарегистрироваться
    public static Profile? LoadProfile(string filePath)
    {
        if (!File.Exists(filePath))
            return null;

        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length < 3 || !int.TryParse(lines[2], out int birthYear))
            return null;

        return new Profile(lines[0], lines[1], birthYear);
    }

    // Одна задача — одна строка: Index;"Text";Status;LastUpdate
    public static void SaveTodos(TodoList todos, string filePath)
    {
        var lines = new string[todos.Count];
        int i = 0;
        foreach (TodoItem item in todos) // foreach работает благодаря IEnumerable в TodoList
        {
            string date = item.LastUpdate.ToString(DateFormat, CultureInfo.InvariantCulture);
            lines[i] = $"{i};{EscapeCsv(item.Text)};{item.Status};{date}";
            i++;
        }
        File.WriteAllLines(filePath, lines);
    }

    // Если файла нет — возвращаем пустой список. Повреждённые строки пропускаем и говорим об этом
    public static TodoList LoadTodos(string filePath)
    {
        var todos = new TodoList();
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
