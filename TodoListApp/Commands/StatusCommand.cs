// Заменяет команду done: теперь у задачи не два состояния, а пять
public class StatusCommand : TaskCommand
{
    public TodoStatus Status { get; set; }

    // Прежний статус нужен, чтобы undo мог его вернуть
    private TodoStatus _oldStatus;

    protected override bool Apply()
    {
        if (!CheckIndex())
            return false;

        _oldStatus = AppInfo.Todos[Index].Status;
        AppInfo.Todos.SetStatus(Index, Status);
        Console.WriteLine($"Задача №{Index + 1}: статус изменён на «{AppInfo.Todos[Index].StatusText}».");
        return true;
    }

    public override void Unexecute()
    {
        AppInfo.Todos.SetStatus(Index, _oldStatus);
        Console.WriteLine($"Изменение статуса задачи №{Index + 1} отменено.");
    }
}
