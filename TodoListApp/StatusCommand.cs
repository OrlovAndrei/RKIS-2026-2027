using System;

class StatusCommand : ICommand
{
    public TodoList? TodoList { get; set; }
    public int Index { get; set; }
    public TodoStatus Status { get; set; }
    public string? TodoFilePath { get; set; }

    public void Execute()
    {
        if (TodoList == null)
        {
            return;
        }

        if (TodoList.SetStatus(Index, Status))
        {
            if (!string.IsNullOrEmpty(TodoFilePath))
            {
                FileManager.SaveTodos(TodoList, TodoFilePath);
            }

            TodoItem item = TodoList[Index];
            Console.WriteLine($"Статус задачи \"{item.GetShortText()}\" изменён на \"{item.GetStatusText()}\".");
        }
        else
        {
            Console.WriteLine("Неверный номер задачи. Используйте: status <номер> <статус>");
        }
    }
}
