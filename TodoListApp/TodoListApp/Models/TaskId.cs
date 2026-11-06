namespace TodoListApp.Models;

internal readonly record struct TaskId(int Value) : IComparable<TaskId>
{
    public int CompareTo(TaskId other) => Value.CompareTo(other.Value);

    public override string ToString() => Value.ToString();
}
