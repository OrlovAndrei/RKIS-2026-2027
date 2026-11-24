public class RedoCommand : ICommand
{
    public void Execute()
    {
        if (AppInfo.RedoStack.Count == 0)
        {
            Console.WriteLine("Нечего повторять.");
            return;
        }

        // Берём последнюю отменённую команду и выполняем снова; Redo сам вернёт её в undo-стек
        UndoableCommand command = AppInfo.RedoStack.Pop();
        command.Redo();
    }
}
