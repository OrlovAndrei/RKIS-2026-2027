namespace TodoListApp.Commands;

internal interface ICommand
{
    bool CanUndo { get; }
    void Execute();
    void Unexecute();
}

internal abstract class CommandBase : ICommand
{
    public virtual bool CanUndo => false;

    public abstract void Execute();

    public virtual void Unexecute()
    {
        throw new InvalidOperationException("Эта команда не поддерживает отмену.");
    }
}

internal static class CommandHistory
{
    public static void Execute(ICommand command)
    {
        command.Execute();
        if (command.CanUndo)
        {
            AppInfo.UndoStack.Push(command);
            AppInfo.RedoStack.Clear();
        }
    }
}
