using System;

class TodoItem
{
    public string Text { get; private set; }
    public TodoStatus Status { get; private set; }
    public DateTime LastUpdate { get; private set; }

    public TodoItem(string text)
    {
        Text = text;
        Status = TodoStatus.NotStarted;
        LastUpdate = DateTime.Now;
    }

    public TodoItem(string text, TodoStatus status, DateTime lastUpdate)
    {
        Text = text;
        Status = status;
        LastUpdate = lastUpdate;
    }

    public void SetStatus(TodoStatus status)
    {
        Status = status;
        LastUpdate = DateTime.Now;
    }

    public void UpdateText(string newText)
    {
        Text = newText;
        LastUpdate = DateTime.Now;
    }

    public string GetShortText()
    {
        string oneLine = Text.Replace("\n", " ");
        if (oneLine.Length > 30)
        {
            return oneLine.Substring(0, 30) + "...";
        }
        return oneLine;
    }

    public string GetStatusText()
    {
        switch (Status)
        {
            case TodoStatus.NotStarted:
                return "Не начато";
            case TodoStatus.InProgress:
                return "В процессе";
            case TodoStatus.Completed:
                return "Выполнено";
            case TodoStatus.Postponed:
                return "Отложено";
            case TodoStatus.Failed:
                return "Провалено";
            default:
                return Status.ToString();
        }
    }

    public string GetShortInfo()
    {
        return $"{GetShortText(),-33} | {GetStatusText(),-12} | {LastUpdate:dd.MM.yyyy HH:mm:ss}";
    }

    public string GetFullInfo()
    {
        return $"{Text}\nСтатус: {GetStatusText()}\nДата последнего изменения: {LastUpdate:dd.MM.yyyy HH:mm:ss}";
    }
}
