public class UndoCommand : ICommand
{
    public void Execute()
    {
        if (AppInfo.UndoStack.Count == 0)
        {
            Console.WriteLine("Нечего отменять.");
            return;
        }

        // Берём последнюю выполненную команду, отменяем её и кладём в redo-стек
        UndoableCommand command = AppInfo.UndoStack.Pop();
        command.Unexecute();
        AppInfo.RedoStack.Push(command);
    }
}
