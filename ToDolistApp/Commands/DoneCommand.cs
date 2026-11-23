using System;

namespace TodoListApp;

public class DoneCommand : IndexedCommand
{
    public override void Execute()
    {
        if (!TryGetItem(out TodoItem item, out _)) return;
        item.MarkDone();
        Console.WriteLine($"Задача №{Index} отмечена выполненной.");
    }
}
