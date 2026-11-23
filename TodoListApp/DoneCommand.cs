namespace TodoListApp;

public class DoneCommand : ICommand
{
    public int Index { get; set; }
    public TodoList TodoList { get; set; } = null!;

    public void Execute()
    {
        if (!CommandHelper.TryGetTask(TodoList, Index, out TodoItem? task))
            return;

        task!.MarkDone();
        FileManager.SaveTodos(TodoList, FileManager.TodoPath);
        Console.WriteLine($"Задача #{Index} отмечена как выполненная.");
    }
}