namespace TodoListApp;

public class DeleteCommand : ICommand
{
    public int Index { get; set; }
    public TodoList TodoList { get; set; } = null!;

    public void Execute()
    {
        if (!CommandHelper.TryGetTask(TodoList, Index, out _))
            return;

        TodoList.Delete(Index - 1);
        FileManager.SaveTodos(TodoList, FileManager.TodoPath);
        Console.WriteLine($"Задача #{Index} удалена.");
    }
}