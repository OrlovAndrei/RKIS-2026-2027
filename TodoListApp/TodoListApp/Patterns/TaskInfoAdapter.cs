using TodoListApp.Models;

namespace TodoListApp.Patterns;

internal interface ITaskInfoView
{
    string Format();
}

internal sealed class TaskInfoAdapter(TodoItem item) : ITaskInfoView
{
    public string Format() => item.GetFullInfo();
}
