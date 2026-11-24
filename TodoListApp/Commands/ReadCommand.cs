// Ничего не меняет, поэтому не наследует UndoableCommand и в историю не попадает
public class ReadCommand : ICommand
{
    // Индекс в списке (с 0); пользователь вводит номер с 1, перевод делает CommandParser
    public int Index { get; set; }

    public void Execute()
    {
        TodoItem? item = AppInfo.Todos.GetItem(Index);
        if (item is null)
        {
            Console.WriteLine($"Задачи с номером {Index + 1} не существует.");
            return;
        }

        Console.WriteLine($"Задача №{Index + 1}:");
        Console.WriteLine(item.GetFullInfo());
    }
}
