using System;

namespace TodoListApp;

public class TodoItem
{
    private const int MaxShortTextLength = 30;

    public string Text { get; private set; }
    public bool IsDone { get; private set; }
    public DateTime LastUpdate { get; private set; }

    public TodoItem(string text)
    {
        Text = text;
        IsDone = false;
        LastUpdate = DateTime.Now;
    }

    public TodoItem(string text, bool isDone, DateTime lastUpdate)
    {
        Text = text;
        IsDone = isDone;
        LastUpdate = lastUpdate;
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
        string cleanText = Text.Replace('\n', ' ');
        if (cleanText.Length > MaxShortTextLength)
        {
            cleanText = cleanText.Substring(0, MaxShortTextLength - 3) + "...";
        }
        string status = IsDone ? "выполнена" : "не выполнена";
        return $"{cleanText} | {status} | {LastUpdate:dd.MM.yyyy HH:mm:ss}";
    }

    public string GetFullInfo()
    {
        string status = IsDone ? "выполнена" : "не выполнена";
        return $"Статус: {status}\nДата последнего изменения: {LastUpdate:dd.MM.yyyy HH:mm:ss}\nПолный текст:\n{Text}";
    }
}