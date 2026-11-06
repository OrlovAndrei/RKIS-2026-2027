using System.Globalization;

namespace TodoListApp.Models;

internal sealed class TodoItem : IComparable<TodoItem>, IEquatable<TodoItem>
{
    private const int ShortTextLength = 30;

    public TaskId Id { get; }
    public string Text { get; private set; }
    public TodoStatus Status { get; private set; }
    public DateTime LastUpdate { get; private set; }
    public bool IsDone => Status == TodoStatus.Completed;

    public TodoItem(TaskId id, string text)
        : this(id, text, TodoStatus.NotStarted, DateTime.Now)
    {
    }

    public TodoItem(TaskId id, string text, TodoStatus status, DateTime lastUpdate)
    {
        Id = id;
        Text = text;
        Status = status;
        LastUpdate = lastUpdate;
    }

    public void MarkDone() => SetStatus(TodoStatus.Completed);

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

    public void Restore(TaskItemSnapshot snapshot)
    {
        Text = snapshot.Text;
        Status = snapshot.Status;
        LastUpdate = snapshot.LastUpdate;
    }

    public TaskItemSnapshot Capture() => new(Id, Text, Status, LastUpdate);

    public string GetShortInfo()
    {
        string displayText = Text.Replace("\r", " ").Replace("\n", " ");
        if (displayText.Length > ShortTextLength)
        {
            displayText = displayText[..ShortTextLength] + "...";
        }

        return $"{displayText} | {Status} | {LastUpdate.ToString("g", CultureInfo.CurrentCulture)}";
    }

    public string GetFullInfo()
    {
        return $"#{Id.Value}: {Text}{Environment.NewLine}" +
            $"Статус: {Status}{Environment.NewLine}" +
            $"Последнее изменение: {LastUpdate.ToString("g", CultureInfo.CurrentCulture)}";
    }

    public int CompareTo(TodoItem? other)
    {
        if (other is null)
        {
            return 1;
        }

        return Id.CompareTo(other.Id);
    }

    public bool Equals(TodoItem? other) => other is not null && Id == other.Id;

    public override bool Equals(object? obj) => obj is TodoItem other && Equals(other);

    public override int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(TodoItem? left, TodoItem? right) => object.Equals(left, right);

    public static bool operator !=(TodoItem? left, TodoItem? right) => !object.Equals(left, right);
}

internal readonly record struct TaskItemSnapshot(
    TaskId Id,
    string Text,
    TodoStatus Status,
    DateTime LastUpdate);
