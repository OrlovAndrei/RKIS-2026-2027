public class ReadCommand : TaskCommand
{
    protected override void ExecuteOnItem(TodoItem item)
    {
        Console.WriteLine($"Задача №{Index + 1}:");
        Console.WriteLine(item.GetFullInfo());
    }
}
