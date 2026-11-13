using System;

namespace TodoList
{
    public class TodoItem
    {
        // Приватные поля (требование инкапсуляции)
        private string _text;
        private bool _isDone;
        private DateTime _lastUpdate;

        // Публичные свойства для доступа извне
        public string Text { get { return _text; } }
        public bool IsDone { get { return _isDone; } }
        public DateTime LastUpdate { get { return _lastUpdate; } }

        // Конструктор
        public TodoItem(string text)
        {
            _text = text;
            _isDone = false;
            _lastUpdate = DateTime.Now;
        }

        // Методы
        public void MarkDone()
        {
            _isDone = true;
            _lastUpdate = DateTime.Now;
        }

        public void UpdateText(string newText)
        {
            _text = newText;
            _lastUpdate = DateTime.Now;
        }

        public string GetShortInfo()
        {
            string shortText = _text.Length > 30 ? _text.Substring(0, 30) + "..." : _text;
            string status = _isDone ? "[V] Выполнено" : "[ ] В процессе";
            return $"{shortText,-35} | {status} | {_lastUpdate:dd.MM.yyyy HH:mm}";
        }

        public string GetFullInfo()
        {
            string status = _isDone ? "Выполнено" : "Не выполнено";
            return $"Задача: {_text}\nСтатус: {status}\nПоследнее изменение: {_lastUpdate}";
        }
    }
}