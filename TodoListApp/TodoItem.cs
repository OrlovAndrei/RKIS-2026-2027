namespace TodoListApp
{
    internal class TodoItem
    {
        private const int ShortTextLength = 30;
        private const string TruncationSuffix = "...";
        private const string NewLine = "\n";
        private const string DateFormat = "dd.MM.yyyy HH:mm";
        private const string InfoSeparator = " | ";

        private const string StatusDone = "сделано";
        private const string StatusNotDone = "не сделано";
        private const string FullStatusDone = "выполнена";
        private const string FullStatusNotDone = "не выполнена";

        private string text;
        private bool isDone;
        private DateTime lastUpdate;

        public TodoItem(string text)
        {
            this.text = text ?? "";
            isDone = false;
            lastUpdate = DateTime.Now;
        }
        
	public TodoItem(string text, bool isDone, DateTime lastUpdate)
        {
            this.text = text ?? "";
            this.isDone = isDone;
            this.lastUpdate = lastUpdate;
        }

        public string Text => text;

        public bool IsDone => isDone;

        public DateTime LastUpdate => lastUpdate;

        public void MarkDone()
        {
            isDone = true;
            lastUpdate = DateTime.Now;
        }

        public void UpdateText(string newText)
        {
            text = newText ?? "";
            lastUpdate = DateTime.Now;
        }

        public string GetShortText()
        {
            string singleLine = text.Replace(NewLine, " ");
            if (singleLine.Length <= ShortTextLength)
            {
                return singleLine;
            }

            return singleLine.Substring(0, ShortTextLength) + TruncationSuffix;
        }

        public string GetStatusText()
        {
            return isDone ? StatusDone : StatusNotDone;
        }

        public string GetDateText()
        {
            return lastUpdate.ToString(DateFormat);
        }

        public string GetShortInfo()
        {
            return GetShortText() + InfoSeparator + GetStatusText() + InfoSeparator + GetDateText();
        }

        public string GetFullInfo()
        {
            string fullStatus = isDone ? FullStatusDone : FullStatusNotDone;

            return $"Задача: {text}{Environment.NewLine}"
                + $"Статус: {fullStatus}{Environment.NewLine}"
                + $"Дата последнего изменения: {GetDateText()}";
        }
    }
}