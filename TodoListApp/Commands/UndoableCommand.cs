// Базовый класс для команд, которые меняют список задач и поэтому попадают в историю (undo/redo)
public abstract class UndoableCommand : ICommand
{
    // Новое действие пользователя: выполняем, запоминаем для undo и сохраняем данные.
    // После нового действия «повторить» то, что было отменено раньше, уже нельзя — redo-стек очищаем
    public void Execute()
    {
        if (!Apply())
            return; // не получилось (например, неверный номер) — в историю команда не попадает

        AppInfo.UndoStack.Push(this);
        AppInfo.RedoStack.Clear();
        FileManager.SaveCurrentTodos();
    }

    // Повторить отменённое действие (вызывается командой redo): та же команда возвращается в undo-стек
    public void Redo()
    {
        if (!Apply())
            return;

        AppInfo.UndoStack.Push(this);
        FileManager.SaveCurrentTodos();
    }

    // Само действие команды; возвращает false, если выполнить его не удалось
    protected abstract bool Apply();

    // Отмена действия: команда должна вернуть список в то состояние, которое было до Apply
    public abstract void Unexecute();
}
