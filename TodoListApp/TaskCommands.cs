using System;

public class ReadCommand : ICommand
{
    public TodoList TodoList { get; set; } = null!;
    public int Index { get; set; } // Номер задачи для пользователя (с 1).

    public void Execute()
    {
        TodoItem? item = TodoList.GetItem(Index - 1);
        if (item == null)
        {
            Console.WriteLine("Задачи с таким номером нет");
            return;
        }
        Console.WriteLine("Задача №" + Index);
        Console.WriteLine(item.GetFullInfo());
    }
}

public class DoneCommand : ICommand
{
    public TodoList TodoList { get; set; } = null!;
    public int Index { get; set; }

    public void Execute()
    {
        TodoItem? item = TodoList.GetItem(Index - 1);
        if (item == null)
        {
            Console.WriteLine("Задачи с таким номером нет");
            return;
        }
        item.MarkDone();
        Console.WriteLine("Задача выполнена");
    }
}

public class DeleteCommand : ICommand
{
    public TodoList TodoList { get; set; } = null!;
    public int Index { get; set; }

    public void Execute()
    {
        if (TodoList.Delete(Index - 1))
            Console.WriteLine("Задача удалена");
        else
            Console.WriteLine("Задачи с таким номером нет");
    }
}

public class UpdateCommand : ICommand
{
    public TodoList TodoList { get; set; } = null!;
    public int Index { get; set; }
    public string Text { get; set; } = "";

    public void Execute()
    {
        TodoItem? item = TodoList.GetItem(Index - 1);
        if (item == null)
        {
            Console.WriteLine("Задачи с таким номером нет");
            return;
        }
        item.UpdateText(Text);
        Console.WriteLine("Задача изменена");
    }
}
