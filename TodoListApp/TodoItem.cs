namespace TodoListApp;

public class TodoItem
{
    public string Text { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime LastModified { get; private set; }

    public TodoItem(string text)
    {
        Text = text;
        IsCompleted = false;
        LastModified = DateTime.Now;
    }

    public void MarkDone()
    {
        IsCompleted = true;
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

        string status = IsCompleted ? "Выполнена" : "Не выполнена";

        return $"{shortText} | {status} | {LastModified}";
    }

    public string GetFullInfo()
    {
        string status = IsCompleted ? "Выполнена" : "Не выполнена";

        return $"Задача: {Text}\nСтатус: {status}\nПоследнее изменение: {LastModified}";
    }
}