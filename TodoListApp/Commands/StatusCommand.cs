// Заменяет команду done: теперь у задачи не два состояния, а пять
public class StatusCommand : TaskCommand
{
    public TodoStatus Status { get; set; }

    protected override void Run()
    {
        TodoList.SetStatus(Index, Status);
        FileManager.SaveTodos(TodoList, FileManager.TodoFilePath);
        Console.WriteLine($"Задача №{Index + 1}: статус изменён на «{TodoList[Index].StatusText}».");
    }
}
