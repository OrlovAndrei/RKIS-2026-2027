using System;

// Новая команда: status 2 inprogress / status 1 completed.
public class StatusCommand : ICommand
{
    public TodoList TodoList { get; set; } = null!;
    public int Index { get; set; } // Номер задачи у пользователя, с 1.
    public TodoStatus Status { get; set; }

    public void Execute()
    {
        if (!TodoList.SetStatus(Index - 1, Status))
        {
            Console.WriteLine("Задачи с таким номером нет");
            return;
        }
        TodoSaver.SaveTasks(TodoList);
        Console.WriteLine("Статус изменён: " + TodoList[Index - 1].GetStatusText());
    }
}
