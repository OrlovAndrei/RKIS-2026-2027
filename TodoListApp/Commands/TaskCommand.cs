// Базовый класс для изменяющих команд, которые работают с одной задачей по номеру (status, update, delete)
public abstract class TaskCommand : UndoableCommand
{
    // Индекс в списке (с 0); пользователь вводит номер с 1, перевод делает CommandParser
    public int Index { get; set; }

    // Проверяет, что задача с таким номером есть; если нет — сообщает об этом
    protected bool CheckIndex()
    {
        if (AppInfo.Todos.GetItem(Index) is not null)
            return true;

        Console.WriteLine($"Задачи с номером {Index + 1} не существует.");
        return false;
    }
}
