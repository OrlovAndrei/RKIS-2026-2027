using System;
using System.IO;

namespace TodoListApp
{
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
            string text = $"{profile.FirstName};{profile.LastName};{profile.BirthYear}";
            File.WriteAllText(filePath, text);
        }

        public static Profile LoadProfile(string filePath)
        {
            if (!File.Exists(filePath)) return null;

            string text = File.ReadAllText(filePath);
            string[] parts = text.Split(';');
            if (parts.Length == 3)
            {
                return new Profile(parts[0], parts[1], parts[2]);
            }
            return null;
        }

        public static void SaveTodos(TodoList todos, string filePath)
        {
            string csv = "";
            for (int i = 0; i < todos.Count; i++)
            {
                TodoItem item = todos.GetItem(i);
                string escapedText = EscapeCsv(item.Text);
                string status = item.IsDone ? "true" : "false";
                csv += $"{i};{escapedText};{status};{item.LastUpdate:O}\n";
            }
            File.WriteAllText(filePath, csv);
        }

        public static TodoList LoadTodos(string filePath)
        {
            TodoList list = new TodoList();
            if (!File.Exists(filePath)) return list;

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                
                int firstSemi = line.IndexOf(';');
                int lastSemi = line.LastIndexOf(';');
                
                if (firstSemi == -1 || lastSemi == firstSemi) continue;

                string indexStr = line.Substring(0, firstSemi);
                string rest = line.Substring(firstSemi + 1, lastSemi - firstSemi - 1);
                string text = UnescapeCsv(rest);
                string statusStr = line.Substring(lastSemi + 1);
                
                bool isDone = statusStr == "true" || statusStr == "True";
                
                TodoItem item = new TodoItem(text);
                if (isDone) item.MarkDone();
                
                list.Add(item);
            }
            return list;
        }

        private static string EscapeCsv(string text)
        {
            return "\"" + text.Replace("\"", "\"\"").Replace("\n", "\\n") + "\"";
        }

        private static string UnescapeCsv(string text)
        {
            return text.Trim('"').Replace("\\n", "\n").Replace("\"\"", "\"");
        }
    }
}