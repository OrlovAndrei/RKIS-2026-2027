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
        TodoSaver.SaveTasks(TodoList);
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
        {
            TodoSaver.SaveTasks(TodoList);
            Console.WriteLine("Задача удалена");
        }
        else Console.WriteLine("Задачи с таким номером нет");
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
        TodoSaver.SaveTasks(TodoList);
        Console.WriteLine("Задача изменена");
    }
}

// Вспомогательный класс: команды используют общий способ сохранения.
internal static class TodoSaver
{
    public static void SaveTasks(TodoList todoList)
    {
        try { FileManager.SaveTodos(todoList, FileManager.TodoFilePath); }
        catch (System.IO.IOException ex) { Console.WriteLine("Ошибка сохранения: " + ex.Message); }
        catch (UnauthorizedAccessException ex) { Console.WriteLine("Нет доступа к файлу: " + ex.Message); }
    }
}
