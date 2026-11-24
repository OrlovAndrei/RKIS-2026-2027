// Базовый класс для команд, которые работают с одной задачей по номеру (read, update, status, remove)
public abstract class TaskCommand : ICommand
{
    public required TodoList TodoList { get; set; }

    // Индекс в списке (с 0); пользователь вводит номер с 1, перевод делает CommandParser
    public int Index { get; set; }

    // Шаблонный метод: проверка номера общая для всех наследников,
    // а что делать с задачей, решает конкретная команда
    public void Execute()
    {
        if (TodoList.GetItem(Index) is null)
        {
            Console.WriteLine($"Задачи с номером {Index + 1} не существует.");
            return;
        }
        Run();
    }

    protected abstract void Run();
}
