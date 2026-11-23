namespace TodoListApp;

public sealed class TodoItem
{
    public string Text { get; private set; }

    public bool IsDone { get; private set; }

    public DateTime LastUpdate { get; private set; }

    public TodoItem(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Текст задачи не может быть пустым.", nameof(text));

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
        if (string.IsNullOrWhiteSpace(newText))
            throw new ArgumentException("Текст задачи не может быть пустым.", nameof(newText));

        Text = newText;
        LastUpdate = DateTime.Now;
    }

    public string GetShortInfo()
    {
        string preview = GetTextPreview();
        string status = IsDone ? "выполнена" : "не выполнена";
        return $"{preview} | {status} | {LastUpdate:dd.MM.yyyy HH:mm}";
    }

    public string GetFullInfo()
    {
        string status = IsDone ? "выполнена" : "не выполнена";
        return $"{Text}{Environment.NewLine}Статус: {status}{Environment.NewLine}Дата последнего изменения: {LastUpdate:dd.MM.yyyy HH:mm}";
    }

    internal string GetTextPreview()
    {
        string normalizedText = Text.Replace("\r\n", "\n", StringComparison.Ordinal)
            .Replace('\r', '\n')
            .Replace('\n', ' ');

        return normalizedText.Length > 30 ? normalizedText[..30] + "..." : normalizedText;
    }
}
