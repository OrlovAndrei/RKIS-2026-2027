using System;
using System.Collections.Generic;
using System.IO;

namespace TodoListApp;

public static class FileManager
{
    // Создание папки, если её нет
    public static void EnsureDataDirectory(string dirPath)
    {
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);
    }

    // Сохранение задач в CSV
    public static void SaveTodos(List<TodoItem> todos, string filePath)
    {
        var lines = new List<string>();
        for (int i = 0; i < todos.Count; i++)
        {
            var t = todos[i];
            // Экранируем кавычки и переносы строк для корректного CSV
            string escapedText = "\"" + t.Text.Replace("\"", "\"\"").Replace("\n", "\\n") + "\"";
            
            // Формат: Index; Text; IsCompleted; LastModified
            lines.Add($"{i};{escapedText};{t.IsCompleted};{t.LastModified:O}");
        }
        File.WriteAllLines(filePath, lines);
    }

    // Загрузка задач из CSV
    public static List<TodoItem> LoadTodos(string filePath)
    {
        var list = new List<TodoItem>();
        if (!File.Exists(filePath)) return list;

        var lines = File.ReadAllLines(filePath);
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            
            var parts = line.Split(';');
            if (parts.Length < 4) continue;

            // Обратное экранирование
            string text = parts[1].Trim('"').Replace("\\n", "\n").Replace("\"\"", "\"");
            
            list.Add(new TodoItem
            {
                Text = text,
                IsCompleted = bool.Parse(parts[2]),
                LastModified = DateTime.Parse(parts[3])
            });
        }
        return list;
    }

    // Заглушки для профиля (требуются по заданию)
    public static void SaveProfile(string profileData, string filePath)
    {
        File.WriteAllText(filePath, profileData);
    }

    public static string LoadProfile(string filePath)
    {
        if (!File.Exists(filePath)) return "";
        return File.ReadAllText(filePath);
    }
}