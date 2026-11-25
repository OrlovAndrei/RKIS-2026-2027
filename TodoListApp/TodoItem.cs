using System;

namespace TodoListApp
{
    class TodoItem
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
            string text = Text.Replace("\n", " ");

            if (text.Length > 30)
            {
                text = text.Substring(0, 30) + "...";
            }

            string status = IsDone
                ? "Выполнена" : "Не выполнена";

            return $"{text} {status} {LastUpdate:dd.MM.yyyy HH:mm}";
        }

        public string GetFullInfo()
        {
            string status = IsDone
                ? "Выполнена" : "Не выполнена";

            return $"Текст: {Text}\nСтатус: {status}\nДата: {LastUpdate:dd.MM.yyyy HH:mm}";
        }
    }
}