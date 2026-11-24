public class RemoveCommand : TaskCommand
{
    protected override void ExecuteOnItem(TodoItem item)
    {
        TodoList.Delete(Index);
        FileManager.SaveTodos(TodoList, FileManager.TodoFilePath);
        Console.WriteLine($"Задача №{Index + 1} удалена.");
    }
}
