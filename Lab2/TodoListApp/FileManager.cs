using System;
using System.IO;
using System.Collections.Generic;
namespace TodoListApp
{
    // Пункт 2: Статический класс для работы с файловой системой
    public static class FileManager
    {
        // Проверяет и создает папку при необходимости (Пункт 2, дефис 1)
        public static void EnsureDataDirectory(string dirPath)
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
        }
            // Пункт 3: Сохранение списка всех профилей в profile.csv
        public static void SaveAllProfiles(List<Profile> profiles, string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                foreach (var p in profiles)
                {
                    sw.WriteLine($"{p.Id};{p.Login};{p.Password};{p.FirstName};{p.LastName};{p.BirthYear}");
                }
            }
        }
        // Пункт 3: Загрузка списка всех профилей из profile.csv
        public static List<Profile> LoadAllProfiles(string filePath)
        {
            List<Profile> list = new List<Profile>();
            if (!File.Exists(filePath)) return list;

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] parts = line.Split(';');
                if (parts.Length == 6 && Guid.TryParse(parts[0], out Guid id) && int.TryParse(parts[5], out int year))
                {
                    list.Add(new Profile(id, parts[1], parts[2], parts[3], parts[4], year));
                }
            }
            return list;
        }

        // Пункт 3: Сохранение задач в CSV-формат (Пункт 2, дефис 4)
        public static void SaveTodos(TodoList todos, string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                int index = 0;
                while (true)
                {
                    TodoItem? item = todos.GetItem(index);
                    if (item == null) break;

                    // Заменяем переносы строк на строку "\\n" (Пункт 3, совет)
                    string safeText = item.Text.Replace("\"", "\"\"").Replace(Environment.NewLine, "\\n").Replace("\n", "\\n");
                    sw.WriteLine($"{index};\"{safeText}\";{item.Status.ToString().ToLower()};{item.LastUpdate:yyyy-MM-ddTHH:mm:ss}");
                    index++;
                }
            }
        }
        // Загрузка задач из CSV-файла (Пункт 2, дефис 5)
        public static TodoList LoadTodos(string filePath)
        {
            TodoList list = new TodoList();
            if (!File.Exists(filePath)) return list;

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                string[] parts = line.Split(';');
                if (parts.Length >= 4)
                {
                    string text = parts[1].Trim('"').Replace("\\n", Environment.NewLine);
                    bool isDone = parts[2].ToLower() == "true";
                    DateTime date = DateTime.TryParse(parts[3], out DateTime d) ? d : DateTime.Now;

                                        // Сначала парсим статус из CSV, а затем создаем объект задачи
                    TodoStatus status = Enum.TryParse(parts[2], true, out TodoStatus parsedStatus) ? parsedStatus : TodoStatus.NotStarted;

                    TodoItem item = new TodoItem(text)
                    {
                        Status = status,
                        LastUpdate = date
                    };
                    list.Add(item);
                }
            }
            return list;
        }
    }
}
