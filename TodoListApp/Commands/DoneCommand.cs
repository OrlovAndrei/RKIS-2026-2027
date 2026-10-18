public class DoneCommand : TaskCommand
{
    protected override void ExecuteOnItem(TodoItem item)
    {
        item.MarkDone();
        Console.WriteLine($"Задача №{Index + 1} отмечена выполненной.");
    }
}
