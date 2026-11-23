using System;

namespace TodoListApp;

public class ReadCommand : IndexedCommand
{
    public override void Execute()
    {
        if (!TryGetItem(out TodoItem item, out _)) return;
        Console.WriteLine($"Задача №{Index}");
        Console.WriteLine(item.GetFullInfo());
    }
}
