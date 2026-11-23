using System;

namespace TodoListApp;

public class StatusCommand : IndexedCommand
{
    public TodoStatus Status { get; set; }
    public override void Execute()
    {
        if (!TryGetItem(out _, out int actualIndex)) return;

        TodoList.SetStatus(actualIndex, Status);
        FileManager.SaveTodos(TodoList, FilePath);
        Console.WriteLine($"Статус задачи №{Index} изменён на «{TodoItem.GetStatusText(Status)}».");
    }
}
