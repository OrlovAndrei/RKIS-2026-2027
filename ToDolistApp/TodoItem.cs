using System;

namespace TodoListApp;

public class TodoItem
{
    private const int MaxShortTextLength = 30;

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
        IsDone = isDone;
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

    public string GetShortInfo()
    {
        string cleanText = Text.Replace('\n', ' ');
        if (cleanText.Length > MaxShortTextLength)
        {
            cleanText = cleanText.Substring(0, MaxShortTextLength - 3) + "...";
        }
        string status = IsDone ? "выполнена" : "не выполнена";
        return $"{cleanText} | {GetStatusText(Status)} | {LastUpdate:dd.MM.yyyy HH:mm:ss}";
    }

    public string GetFullInfo()
    {
        return $"Статус: {GetStatusText(Status)}\nДата последнего изменения: {LastUpdate:dd.MM.yyyy HH:mm:ss}\nПолный текст:\n{Text}";
    }
    public static string GetStatusText(TodoStatus status) => status switch
    {
        TodoStatus.NotStarted => "не начато",
        TodoStatus.InProgress => "в процессе",
        TodoStatus.Completed => "выполнено",
        TodoStatus.Postponed => "отложено",
        TodoStatus.Failed => "провалено",
        _ => "неизвестно"
    };
}