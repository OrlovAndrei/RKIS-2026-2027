using System;

namespace TodoListApp
{
    public class TodoItem
    {
        public string Text { get; private set; }
        public bool IsDone { get; private set; }
        public DateTime LastUpdate { get; private set; }

        public TodoItem(string text)
        {
            Text = text;
            IsDone = false;
            LastUpdate = DateTime.Now;
        }

        public void MarkDone()
        {
            IsDone = true;
            LastUpdate = DateTime.Now;
        }

        public void UpdateText(string newText)
        {
            Text = newText;
            LastUpdate = DateTime.Now;
        }

        public string GetShortInfo()
        {
            string cleanText = Text.Replace("\n", " ");
            if (cleanText.Length > 30)
            {
                cleanText = cleanText.Substring(0, 30) + "...";
            }

            string status = IsDone ? "выполнена" : "не выполнена";
            return $"{cleanText,-34} | {status,-14} | {LastUpdate:dd.MM.yyyy HH:mm}";
        }

        public string GetFullInfo()
        {
            string status = IsDone ? "выполнена" : "не выполнена";
            return $"Полный текст задачи:\n{Text}\nСтатус: {status}\nДата изменения: {LastUpdate:dd.MM.yyyy HH:mm}";
        }
    }
}
