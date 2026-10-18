public class RemoveCommand : TaskCommand
{
    protected override void ExecuteOnItem(TodoItem item)
    {
        TodoList.Delete(Index);
        Console.WriteLine($"Задача №{Index + 1} удалена.");
    }
}
