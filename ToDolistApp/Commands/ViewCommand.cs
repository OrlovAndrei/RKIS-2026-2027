namespace TodoListApp;

public class ViewCommand : ICommand
{
    public TodoList TodoList { get; set; } = null!;
    public bool ShowIndex { get; set; }
    public bool ShowStatus { get; set; }
    public bool ShowDate { get; set; }

    public void Execute()
    {
        TodoList.View(ShowIndex, ShowStatus, ShowDate);
    }
}
