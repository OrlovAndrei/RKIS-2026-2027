using System;

class DeleteCommand : ICommand
{
    public TodoList? TodoList { get; set; }
    public int Index { get; set; }

    public void Execute()
    {
        if (TodoList == null)
        {
            return;
        }

        TodoItem? item = TodoList.GetItem(Index);
        if (item != null && TodoList.Delete(Index))
        {
            Console.WriteLine($"Задача \"{item.GetShortText()}\" успешно удалена!");
        }
        else
        {
            Console.WriteLine("Неверный номер задачи. Используйте: delete <номер>");
        }
    }
}

