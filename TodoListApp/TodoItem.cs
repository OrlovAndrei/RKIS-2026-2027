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
            string rawText = Text.Replace("\n", " ");
            string truncatedText = rawText.Length > 30 ? rawText.Substring(0, 27) + "..." : rawText;
            string statusText = IsDone ? "Выполнено" : "Не выполнено";
            return $"Задача: {truncatedText,-33} | Статус: {statusText,-13} | Дата: {LastUpdate:dd.MM.yyyy HH:mm}";
        }

        public string GetFullInfo()
        {
            return $"Полный текст:\n{Text}\nСтатус: {(IsDone ? "выполнена" : "не выполнена")}\nДата изменения: {LastUpdate:dd.MM.yyyy HH:mm:ss}";
        }
    }
}