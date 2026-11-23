using System;

namespace TodoListApp;

public abstract class IndexedCommand : ICommand
{
    public TodoList TodoList { get; set; } = null!;
    public int Index { get; set; }

    public abstract void Execute();

    protected bool TryGetItem(out TodoItem item, out int actualIndex)
    {
        actualIndex = Index - 1;
        item = TodoList.GetItem(actualIndex)!;
        if (item == null)
        {
            Console.WriteLine("Ошибка: задачи с таким индексом не существует.");
            return false;
        }
        return true;
    }
}
