using System;

namespace TodoListApp
{
    internal class TodoItem
    {
        private string _text;
        private bool _isDone;
        private DateTime _lastUpdate;

        public string Text
        {
            get { return _text; }
            private set { _text = value; }
        }

        public bool IsDone
        {
            get { return _isDone; }
            private set { _isDone = value; }
        }

        public DateTime LastUpdate
        {
            get { return _lastUpdate; }
            private set { _lastUpdate = value; }
        }

        public TodoItem(string text)
        {
            this._text = text;
            this._isDone = false;
            this._lastUpdate = DateTime.Now;
        }

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
            string shortText = _text.Replace("\n", " ").Replace("\r", " ");
            if (shortText.Length > 30 )
            {
                shortText = shortText.Substring(0, 27) + "...";
            }
            string status = _isDone ? "Выполнена" : "Не выполнена";
            return $"{shortText}   {status}   {_lastUpdate:dd.MM.yyyy HH.mm}";
        }

        public string GetFullInfo()
        {
            string status = _isDone ? "Выполнена" : "Не выполнена";
            return $"Текст: {_text}\nСтатус: {status}\nДата изменения: {_lastUpdate:dd.MM.yyyy HH.mm}";
        }
    }
}
