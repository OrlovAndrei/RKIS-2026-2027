namespace TodoListApp;

public class ViewCommand : ICommand
{
    public bool ShowIndex { get; set; }
    public bool ShowStatus { get; set; }
    public bool ShowDate { get; set; }
    public bool ShowAll { get; set; }
    public TodoList TodoList { get; set; } = null!;

    public void Execute()
    {
        bool showIndex = ShowIndex;
        bool showStatus = ShowStatus;
        bool showDate = ShowDate;

        if (ShowAll)
        {
            showIndex = true;
            showStatus = true;
            showDate = true;
        }

        TodoList.View(showIndex, showStatus, showDate);
    }
}