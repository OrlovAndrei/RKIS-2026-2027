using System;

class ReadCommand : ICommand
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
        if (item != null)
        {
            Console.WriteLine($"Задача {Index + 1}:");
            Console.WriteLine(item.GetFullInfo());
        }
        else
        {
            Console.WriteLine("Неверный номер задачи. Используйте: read <номер>");
        }
    }
}

