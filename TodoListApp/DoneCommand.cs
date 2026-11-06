using System;

class DoneCommand : ICommand
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
            item.MarkDone();
            Console.WriteLine($"Задача \"{item.GetShortText()}\" отмечена как выполненная!");
        }
        else
        {
            Console.WriteLine("Неверный номер задачи. Используйте: done <номер>");
        }
    }
}

