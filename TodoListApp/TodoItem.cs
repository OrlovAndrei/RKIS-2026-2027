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
            // Заменяем переносы строк на пробелы, чтобы таблица не сломалась
            string cleanText = _text.Replace("\n", " ");
            string shortText = cleanText.Length > 30 ? cleanText.Substring(0, 30) + "..." : cleanText;
            string status = _isDone ? "[V] Выполнено" : "[ ] Не выполнено";
            return $"{shortText,-35} | {status} | {_lastUpdate:dd.MM.yyyy HH:mm}";
        }

        public string GetFullInfo()
        {
            string status = _isDone ? "Выполнено" : "Не выполнено";
            return $"=== Полный текст задачи ===\n{_text}\n===========================\n" +
                   $"Статус: {status}\n" +
                   $"Дата изменения: {_lastUpdate:dd.MM.yyyy HH:mm}";
        }
    }
}