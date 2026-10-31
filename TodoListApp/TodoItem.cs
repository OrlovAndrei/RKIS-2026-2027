using System;

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
        return IsDone ? "Выполнена" : "Не выполнена";
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
