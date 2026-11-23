using System;

namespace TodoListApp;

public class DoneCommand : IndexedCommand
{
    public override void Execute()
    {
        if (!TryGetItem(out TodoItem item, out _)) return;
        item.MarkDone();
         FileManager.SaveTodos(TodoList, FilePath);
        Console.WriteLine($"Задача №{Index} отмечена выполненной.");
    }
}
