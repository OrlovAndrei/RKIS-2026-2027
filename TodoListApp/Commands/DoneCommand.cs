public class DoneCommand : TaskCommand
{
    protected override void ExecuteOnItem(TodoItem item)
    {
        item.MarkDone();
        FileManager.SaveTodos(TodoList, FileManager.TodoFilePath);
        Console.WriteLine($"Задача №{Index + 1} отмечена выполненной.");
    }
}
