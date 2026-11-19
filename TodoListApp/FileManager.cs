using System;
using System.IO;
using System.Collections.Generic;

namespace TodoList
{
    public static class FileManager
    {
        // 1. Проверка и создание папки
        public static void EnsureDataDirectory(string dirPath)
        {
            Directory.CreateDirectory(dirPath);
        }

        // 2. Сохранение профиля
        public static void SaveProfile(Profile profile, string filePath)
        {
            string data = $"{profile.FirstName};{profile.LastName};{profile.BirthYear}";
            File.WriteAllText(filePath, data);
        }

        // 3. Загрузка профиля
        public static Profile? LoadProfile(string filePath)
        {
            if (!File.Exists(filePath)) return null;
            
            string data = File.ReadAllText(filePath);
            string[] parts = data.Split(';');
            
            if (parts.Length >= 3 && int.TryParse(parts[2], out int year))
            {
                return new Profile(parts[0], parts[1], year);
            }
            return null;
        }

        // 4. Сохранение задач в CSV
        public static void SaveTodos(TodoList todos, string filePath)
        {
            List<string> lines = new List<string>();
            
            for (int i = 0; i < todos.Count; i++)
            {
                var item = todos.GetItem(i);
                if (item == null) continue;
                
                string escapedText = EscapeCsv(item.Text);
                // Формат: Index; Text; IsDone; LastUpdate
                string line = $"{i};{escapedText};{item.IsDone.ToString().ToLower()};{item.LastUpdate:yyyy-MM-ddTHH:mm:ss}";
                lines.Add(line);
            }
            
            File.WriteAllLines(filePath, lines);
        }

        // 5. Загрузка задач из CSV
        public static TodoList LoadTodos(string filePath)
        {
            TodoList list = new TodoList();
            if (!File.Exists(filePath)) return list;

            string[] lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                
                string[] parts = line.Split(';');
                if (parts.Length >= 4)
                {
                    string text = UnescapeCsv(parts[1]);
                    bool isDone = bool.Parse(parts[2]);
                    DateTime lastUpdate = DateTime.Parse(parts[3]);
                    
                    list.Add(new TodoItem(text, isDone, lastUpdate));
                }
            }
            return list;
        }

        // Вспомогательные методы для экранирования
        private static string EscapeCsv(string text)
        {
            return "\"" + text.Replace("\"", "\"\"").Replace("\n", "\\n").Replace("\r", "") + "\"";
        }

        private static string UnescapeCsv(string text)
        {
            return text.Trim('"').Replace("\\n", "\n").Replace("\"\"", "\"");
        }
    }
}