// Базовый класс для команд, которые работают с одной задачей по номеру (read, done, update, remove)
public abstract class TaskCommand : ICommand
{
    public required TodoList TodoList { get; set; }

    // Индекс в списке (с 0); пользователь вводит номер с 1, перевод делает CommandParser
    public int Index { get; set; }

    // Шаблонный метод: поиск задачи и проверка, что она существует, общие для всех наследников,
    // а что делать с найденной задачей, решает конкретная команда
    public void Execute()
    {
        TodoItem? item = TodoList.GetItem(Index);
        if (item is null)
        {
            Console.WriteLine($"Задачи с номером {Index + 1} не существует.");
            return;
        }
        ExecuteOnItem(item);
    }

    protected abstract void ExecuteOnItem(TodoItem item);
}
