namespace TodoListApp;

public class StatusCommand : ICommand
{
    public int Index { get; set; }
    public TodoStatus Status { get; set; }
    public TodoList TodoList { get; set; } = null!;

    public void Execute()
    {
        if (!CommandHelper.TryGetTask(TodoList, Index, out TodoItem? task))
            return;

        task!.SetStatus(Status);
        FileManager.SaveTodos(TodoList, FileManager.TodoPath);

        Console.WriteLine($"Статус задачи #{Index} изменён на {Status}.");
    }
}