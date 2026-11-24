public class ViewCommand : ICommand
{
    public bool ShowIndex { get; set; }
    public bool ShowStatus { get; set; }
    public bool ShowDate { get; set; }

    public void Execute()
    {
        AppInfo.Todos.View(ShowIndex, ShowStatus, ShowDate);
    }
}
