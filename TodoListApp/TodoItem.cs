using System;

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
        string shortText = Text.Replace("\n", " ");

        if (shortText.Length > 30)
        {
            shortText = shortText.Substring(0, 30) + "...";
        }

        string status = IsDone ? "сделано" : "не сделано";

        return $"{shortText} | {status} | {LastUpdate:dd.MM.yyyy HH:mm}";
    }

    public string GetFullInfo()
    {
        string status = IsDone ? "выполнена" : "не выполнена";

        return $"Текст: {Text}\n" +
               $"Статус: {status}\n" +
               $"Последнее изменение: {LastUpdate:dd.MM.yyyy HH:mm}";
    }
}