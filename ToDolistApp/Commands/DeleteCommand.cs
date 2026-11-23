using System;

namespace TodoListApp;

public class DeleteCommand : IndexedCommand
{
    public override void Execute()
    {
        if (!TryGetItem(out _, out int actualIndex)) return;
        TodoList.Delete(actualIndex);
        Console.WriteLine($"Задача №{Index} успешно удалена со сдвигом.");
    }
}
