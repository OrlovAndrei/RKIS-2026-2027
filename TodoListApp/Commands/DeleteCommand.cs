public class DeleteCommand : TaskCommand
{
    // Удалённую задачу запоминаем, чтобы undo мог вернуть её на прежнее место
    private TodoItem? _item;

    protected override bool Apply()
    {
        if (!CheckIndex())
            return false;

        _item = AppInfo.Todos[Index];
        AppInfo.Todos.Delete(Index);
        Console.WriteLine($"Задача №{Index + 1} удалена.");
        return true;
    }

    public override void Unexecute()
    {
        if (_item is null)
            return;

        AppInfo.Todos.Insert(Index, _item);
        Console.WriteLine($"Удаление задачи №{Index + 1} отменено.");
    }
}
