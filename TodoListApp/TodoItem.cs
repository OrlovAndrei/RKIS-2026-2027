using System;

// Одна задача: текст, статус (enum) и дата последнего изменения.
public class TodoItem
{
    public string Text { get; private set; }
    public TodoStatus Status { get; private set; }
    public DateTime LastUpdate { get; private set; }

    public TodoItem(string text) : this(text, TodoStatus.NotStarted, DateTime.Now) { }

    // Для загрузки задачи с её настоящими статусом и датой из CSV.
    public TodoItem(string text, TodoStatus status, DateTime lastUpdate)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Текст задачи не может быть пустым", nameof(text));
        if (!Enum.IsDefined(status))
            throw new ArgumentOutOfRangeException(nameof(status));
        if (lastUpdate == default)
            throw new ArgumentException("Некорректная дата задачи", nameof(lastUpdate));

        Text = text;
        Status = status;
        LastUpdate = lastUpdate;
    }

    public void SetStatus(TodoStatus status)
    {
        if (!Enum.IsDefined(status))
            throw new ArgumentOutOfRangeException(nameof(status));
        Status = status;
        LastUpdate = DateTime.Now;
    }

    public void UpdateText(string newText)
    {
        if (string.IsNullOrWhiteSpace(newText))
            throw new ArgumentException("Текст задачи не может быть пустым", nameof(newText));
        Text = newText;
        LastUpdate = DateTime.Now;
    }

    public string GetStatusText() => Status switch
    {
        TodoStatus.NotStarted => "не начато",
        TodoStatus.InProgress => "в процессе",
        TodoStatus.Completed => "выполнено",
        TodoStatus.Postponed => "отложено",
        TodoStatus.Failed => "провалено",
        _ => "неизвестно"
    };

    public string GetShortInfo() => GetShortInfo(true, true);

    // Показываем текст (до 30 символов), статус и дату по заданным флагам.
    public string GetShortInfo(bool showStatus, bool showDate)
    {
        string preview = Text.Replace("\r", " ").Replace("\n", " ");
        if (preview.Length > 30)
            preview = preview.Substring(0, 30) + "...";

        string info = preview.PadRight(33);
        if (showStatus)
            info += " | " + GetStatusText().PadRight(12);
        if (showDate)
            info += " | " + LastUpdate.ToString("dd.MM.yyyy HH:mm:ss");
        return info;
    }

    public string GetFullInfo()
    {
        return $"{Text}\nСтатус: {GetStatusText()}\nДата изменения: {LastUpdate:dd.MM.yyyy HH:mm:ss}";
    }
}
