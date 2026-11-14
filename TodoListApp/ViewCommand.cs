namespace TodoListApp
{
    public class ViewCommand : ICommand
    {
        public bool ShowIndex { get; set; } = false;
        public bool ShowStatus { get; set; } = false;
        public bool ShowDate { get; set; } = false;
        public TodoList TodoList { get; set; }

        public void Execute()
        {
            if (TodoList != null)
            {
                TodoList.View(ShowIndex, ShowStatus, ShowDate);
            }
        }
    }
}