namespace TodoListApp;

public class TodoItem
{
    public string Text { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime LastModified { get; set; } = DateTime.Now;
}