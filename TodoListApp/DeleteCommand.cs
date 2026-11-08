namespace TodoListApp
{
    public class DeleteCommand : ICommand
    {
        public int Index { get; set; }
        public TodoList TodoList { get; set; }

        public void Execute()
        {
            if (TodoList != null)
            {
                TodoList.Delete(Index);
            }
        }
    }
}