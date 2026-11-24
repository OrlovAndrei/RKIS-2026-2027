namespace TodoListApp;

public class ReadCommand : ICommand
{
    public int Index { get; set; }
    public TodoList TodoList { get; set; } = null!;

    public void Execute()
    {
        if (!CommandHelper.TryGetTask(TodoList, Index, out TodoItem? task))
            return;

        Console.WriteLine("==================================");
        Console.WriteLine($"Задача #{Index}:");
        Console.WriteLine("----------------------------------");
        Console.WriteLine(task!.GetFullInfo());
        Console.WriteLine("==================================");
    }
}