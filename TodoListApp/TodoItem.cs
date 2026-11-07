namespace TodoListApp
{
    internal class TodoItem
    {
        private const int ShortTextLength = 30;
        private const string TruncationSuffix = "...";
        private const string NewLine = "\n";
        private const string DateFormat = "dd.MM.yyyy HH:mm";
        private const string InfoSeparator = " | ";

        private const string StatusNotStartedText = "не начато";
        private const string StatusInProgressText = "в процессе";
        private const string StatusCompletedText = "выполнено";
        private const string StatusPostponedText = "отложено";
        private const string StatusFailedText = "провалено";

        private string text;
        private TodoStatus status;
        private DateTime lastUpdate;

        public TodoItem(string text)
        {
            this.text = text ?? "";
            status = TodoStatus.NotStarted;
            lastUpdate = DateTime.Now;
        }

        public TodoItem(string text, TodoStatus status, DateTime lastUpdate)
        {
            this.text = text ?? "";
            this.status = status;
            this.lastUpdate = lastUpdate;
        }

        public string Text => text;

        public TodoStatus Status => status;


        public DateTime LastUpdate => lastUpdate;

        public void SetStatus(TodoStatus newStatus)
        {
            status = newStatus;
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
            switch (status)
            {
                case TodoStatus.NotStarted:
                    return StatusNotStartedText;
                case TodoStatus.InProgress:
                    return StatusInProgressText;
                case TodoStatus.Completed:
                    return StatusCompletedText;
                case TodoStatus.Postponed:
                    return StatusPostponedText;
                case TodoStatus.Failed:
                    return StatusFailedText;
                default:
                    return status.ToString();
            }
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
            return $"Задача: {text}{Environment.NewLine}"
                + $"Статус: {GetStatusText()}{Environment.NewLine}"
                + $"Дата последнего изменения: {GetDateText()}";
        }
    }
}