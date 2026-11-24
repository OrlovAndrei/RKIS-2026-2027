namespace TodoListApp;

public class TodoItem
{
    public string Text { get; private set; }
    public TodoStatus Status { get; private set; }
    public DateTime LastModified { get; private set; }

    public TodoItem(string text)
    {
        Text = text;
        Status = TodoStatus.NotStarted;
        LastModified = DateTime.Now;
    }

    public TodoItem(string text, TodoStatus status, DateTime lastModified)
    {
        Text = text;
        Status = status;
        LastModified = lastModified;
    }

    public void SetStatus(TodoStatus status)
    {
        Status = status;
        LastModified = DateTime.Now;
    }

    public void UpdateText(string newText)
    {
        Text = newText;
        LastModified = DateTime.Now;
    }

    public string GetShortInfo()
    {
        string shortText = Text.Length > 30
            ? Text.Substring(0, 30) + "..."
            : Text;

        return $"{shortText} | {Status} | {LastModified}";
    }

    public string GetFullInfo()
    {
        return $"Задача: {Text}\nСтатус: {Status}\nПоследнее изменение: {LastModified}";
    }
}