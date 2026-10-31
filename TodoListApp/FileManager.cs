using System;
using System.IO;

// Отвечает за создание папки и сохранение/загрузку профиля.
// На следующих этапах добавим работу с CSV-файлом задач.
public static class FileManager
{
    // Путь заполняется в Program.cs после создания папки data.
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
            profile.BirthYear.ToString()
        });
    }

    public static Profile LoadProfile(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("Файл профиля не найден", filePath);

        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length != 3 || !int.TryParse(lines[2], out int year))
            throw new InvalidDataException("Неверный формат profile.txt");
        return new Profile(lines[0], lines[1], year);
    }
}
